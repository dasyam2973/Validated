using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Validated.Generator.Constants;
using Validated.Generator.Emitters;
using Validated.Generator.Extensions;
using Validated.Generator.Models;
using Validated.Generator.Parsers;

namespace Validated.Generator;

[Generator(LanguageNames.CSharp)]
public class ValidatorGenerator : IIncrementalGenerator
{
    record GenerationTarget
    {
        public ValidatableTypeModel? Model { get; set; }
        public EquatableArray<Diagnostic> Diagnostics { get; set; }
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var extracted = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: TypeNames.ValidatableAttributeFqn,
                predicate: static (node, _) => node is TypeDeclarationSyntax,
                transform: static (ctx, ct) => ExtractModel(ctx, ct));

        var diagnostics = extracted
            .Select(static (target, _) => target.Diagnostics);

        context.RegisterSourceOutput(diagnostics, static (spc, diagnostics) =>
        {
            foreach (var diagnostic in diagnostics)
            {
                spc.ReportDiagnostic(diagnostic);
            }
        });

        var models = extracted
            .Select(static (target, _) => target.Model)
            .Where(static model => model is not null)!;

        context.RegisterSourceOutput(models, static (spc, model) =>
        {
            string code = ValidationCodeEmitter.Generate(model!);
            spc.AddSource($"{model!.TypeName}.g.cs", SourceText.From(code, Encoding.UTF8));
        });
    }

    private static GenerationTarget ExtractModel(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        List<Diagnostic> diagnostics = new();

        if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
            return new() { Model = null, Diagnostics = diagnostics.ToEquatableArray() };

        var propertiesBuilder = ImmutableArray.CreateBuilder<ValidatablePropertyModel>();

        foreach (var member in typeSymbol.GetMembers())
        {
            ct.ThrowIfCancellationRequested();

            if (member is not IPropertySymbol and not IFieldSymbol)
                continue;

            var rulesBuilder = ImmutableArray.CreateBuilder<ValidationRule>();

            foreach (var attribute in member.GetAttributes())
            {
                if (member is IPropertySymbol propertySymbol)
                {
                    var rule = ValidationRuleParser.ParseAttribute(attribute, propertySymbol, propertySymbol.Type, context.SemanticModel.Compilation, diagnostics);
                    if (rule is not null)
                    {
                        rulesBuilder.Add(rule);
                    }
                }
                else if (member is IFieldSymbol fieldSymbol)
                {
                    var rule = ValidationRuleParser.ParseAttribute(attribute, fieldSymbol, fieldSymbol.Type, context.SemanticModel.Compilation, diagnostics);
                    if (rule is not null)
                    {
                        rulesBuilder.Add(rule);
                    }
                }
            }

            if (rulesBuilder.Count > 0)
            {
                var memberType = member switch
                {
                    IPropertySymbol p => p.Type.ToDisplayString(),
                    IFieldSymbol f => f.Type.ToDisplayString(),
                    _ => "object"
                };

                propertiesBuilder.Add(new ValidatablePropertyModel(
                    name: member.Name,
                    typeName: memberType,
                    rules: rulesBuilder.ToEquatableArray()
                ));
            }
        }

        if (propertiesBuilder.Count == 0)
            return new() { Model = null, Diagnostics = diagnostics.ToEquatableArray() };

        ValidatableTypeModel typeModel = new(
            namespaceName: typeSymbol.ContainingNamespace.IsGlobalNamespace ? string.Empty : typeSymbol.ContainingNamespace.ToDisplayString(),
            typeName: typeSymbol.Name,
            declarationKeyword: typeSymbol.GetTypeDeclarationKeyword(),
            properties: propertiesBuilder.ToEquatableArray()
        );

        return new() { Model = typeModel, Diagnostics = diagnostics.ToEquatableArray() };
    }
}