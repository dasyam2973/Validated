# Validated
`Validated`는 C#용 유효성 검사 라이브러리입니다. 리플렉션과 런타임 할당을 최소화하여 개발되었으며, 컴파일 타임에 C# 소스 코드를 생성해 높은 퍼포먼스를 보여줍니다.

## Usage
유효성 검사가 필요한 `partial` 구조체, 클래스 및 레코드에 `Validatable` 어트리뷰트를 부여하면 소스 생성기가 `Validate()` 및 `TryValidate()` 등의 메서드를 자동으로 내보냅니다.

```cs
using Validated;
using Validated.Annotations;

    [Validatable]
public partial record UserRegistrationRequest(
    [property: VLength(3, 20)] string Username,

    [property: VRange(18, 100)] int Age,

    DateTime StartDate,

    [property: VGreaterThanOrEqual("StartDate")] DateTime EndDate
);

UserRegistrationRequest request = new("dev_user", 25, DateTime.Now, DateTime.Now.AddDays(1));
ValidationResult result = request.Validate();

if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"[{error.PropertyName}] {error.Message}");
    }
}

if (request.TryValidate(out var validationResult))
{
    // 성공 시 로직...
}

// 오류 목록이 필요하지 않다면 IsValid 이용
if (request.IsValid)
{
    // 성공 시 로직...
}
```