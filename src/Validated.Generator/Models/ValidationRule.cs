using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Validated.Generator.Models;

public abstract class ValidationRule : IEquatable<ValidationRule>
{
    /// <summary>
    /// 클래스 레벨에 추가되어야 하는 static 필드 및 메서드 구문을 반환합니다.
    /// </summary>
    public virtual IEnumerable<string> EmitStaticDeclarations(string propertyName)
    {
        return Enumerable.Empty<string>();
    }

    /// <summary>
    /// 유효성 검사 성공 조건식을 반환합니다.
    /// </summary>
    public abstract string BuildCondition(string targetProperty, string propertyName);

    /// <summary>
    /// 유효성 검사 실패 조건식과 실패 시 VaildationError 객체 생성 표현식을 반환합니다.
    /// </summary>
    public abstract (string FailCondition, string ErrorExpression) BuildErrorCheck(string targetProperty, string propertyName);

    public abstract bool Equals(ValidationRule other);

    protected static string GetErrorMessageExpression(string defaultTemplateFqn, string? customErrorMessage)
    {
        return string.IsNullOrEmpty(customErrorMessage) ? defaultTemplateFqn : SymbolDisplay.FormatLiteral(customErrorMessage!, quote: true);
    }
}
