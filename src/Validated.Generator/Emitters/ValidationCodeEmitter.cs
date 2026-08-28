using System;
using System.Collections.Generic;
using System.Linq;
using Validated.Generator.Models;

namespace Validated.Generator.Emitters;

internal static class ValidationCodeEmitter
{
    public static string Generate(ValidatableTypeModel typeModel)
    {
        IndentedStringBuilder builder = new();

        HashSet<string> staticDeclarations = new();
        foreach (var prop in typeModel.Properties)
        {
            foreach (var rule in prop.Rules)
            {
                foreach (var decl in rule.EmitStaticDeclarations(prop.Name))
                {
                    staticDeclarations.Add(decl);
                }
            }
        }

        using (builder.Block($"namespace {typeModel.Namespace}"))
        {
            List<IDisposable> parentBlocks = new();
            try
            {
                foreach (var parent in typeModel.ContainingTypes)
                {
                    parentBlocks.Add(builder.Block($"partial {parent.DeclarationKeyword} {parent.TypeName}"));
                }

                using (builder.Block($"partial {typeModel.DeclarationKeyword} {typeModel.TypeName} : global::Validated.IValidatable<{typeModel.TypeName}>"))
                {
                    #region bool IsValid
                    foreach (var decl in staticDeclarations)
                    {
                        builder.Line(decl);
                    }

                    if (staticDeclarations.Count > 0)
                        builder.Line();

                    var allConditions = typeModel.Properties
                        .SelectMany(p => p.Rules.Select(r => r.BuildCondition($"this.{p.Name}", p.Name)))
                        .ToList();

                    if (allConditions.Count > 0)
                    {
                        builder.Line($"public bool IsValid =>");
                        for (int i = 0; i < allConditions.Count; i++)
                        {
                            var condition = allConditions[i];
                            builder.Line($"    {condition}{(i + 1 < allConditions.Count ? " &&" : ";")}");
                        }
                    }
                    else
                    {
                        builder.Line("public bool IsValid => true;");
                    }
                    #endregion

                    builder.Line();

                    #region ValidationResult Validate()
                    using (builder.Block("public global::Validated.ValidationResult Validate()"))
                    {
                        builder.Line("var errors = new global::System.Collections.Generic.List<global::Validated.ValidationError>();");

                        foreach (var property in typeModel.Properties)
                        {
                            string targetProperty = $"this.{property.Name}";

                            foreach (var rule in property.Rules)
                            {
                                var (failCondition, errorExpression) = rule.BuildErrorCheck(targetProperty, property.Name);

                                using (builder.Block($"if ({failCondition})"))
                                {
                                    builder.Line($"errors.Add({errorExpression});");
                                }

                                builder.Line();
                            }
                        }

                        builder.Line("return new global::Validated.ValidationResult(errors);");
                    }
                    #endregion

                    builder.Line();

                    #region bool TryValidate(out ValidationError error)
                    using (builder.Block("public bool TryValidate(out global::Validated.ValidationError error)"))
                    {
                        foreach (var property in typeModel.Properties)
                        {
                            string targetProperty = $"this.{property.Name}";

                            foreach (var rule in property.Rules)
                            {
                                var (failCondition, errorExpression) = rule.BuildErrorCheck(targetProperty, property.Name);

                                using (builder.Block($"if ({failCondition})"))
                                {
                                    builder.Line($"error = {errorExpression};");
                                    builder.Line("return false;");
                                }

                                builder.Line();
                            }
                        }

                        builder.Line("error = default;");
                        builder.Line("return true;");
                    }
                    #endregion

                    builder.Line();

                    #region bool TryValidateProperty(string propertyName, out ValidationError error)
                    using (builder.Block("public bool TryValidateProperty(string propertyName, out global::Validated.ValidationError error)"))
                    {
                        using (builder.Block("switch (propertyName)"))
                        {
                            foreach (var property in typeModel.Properties)
                            {
                                if (property.Rules.IsEmpty) continue;

                                string targetProperty = $"this.{property.Name}";

                                builder.Line($"case nameof({targetProperty}):");

                                using (builder.Indent())
                                {
                                    foreach (var rule in property.Rules)
                                    {
                                        var (failCondition, errorExpression) = rule.BuildErrorCheck(targetProperty, property.Name);

                                        using (builder.Block($"if ({failCondition})"))
                                        {
                                            builder.Line($"error = {errorExpression};");
                                            builder.Line("return false;");
                                        }
                                    }

                                    builder.Line("break;");
                                }

                                builder.Line();
                            }

                            builder.Line("default:");
                            using (builder.Indent())
                            {
                                builder.Line("break;");
                            }
                        }

                        builder.Line();
                        builder.Line("error = default;");
                        builder.Line("return true;");
                    }
                    #endregion
                }
            }
            finally
            {
                for (int i = parentBlocks.Count - 1; i >= 0; i--)
                {
                    parentBlocks[i].Dispose();
                }
            }
        }

        return builder.ToString();
    }
}
