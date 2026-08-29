# Validated
`Validated`는 C#용 유효성 검사 라이브러리입니다. 리플렉션과 런타임 할당을 최소화하여 개발되었으며, 컴파일 타임에 C# 소스 코드를 생성해 높은 퍼포먼스를 보여줍니다.

## Usage
유효성 검사가 필요한 `partial` 구조체, 클래스 및 레코드에 `Validatable` 어트리뷰트를 부여하면 소스 생성기가 `Validate()` 및 `TryValidate()` 등의 메서드를 자동으로 내보냅니다.

```cs
using Validated.Annotations;

var user = new UserRegistration("a", 15);
var result = user.Validate();

Console.WriteLine($"IsValid: {result.IsValid}"); // -> False

foreach (var error in result.Errors)
{
    Console.WriteLine($"- [{error.PropertyName}] {error.Message}");
}

[Validatable]
public partial record UserRegistration(
    [property: VLength(3, 20)] string Username,
    [property: VRange(18, 100)] int Age
);
```

오류 목록이 필요하지 않다면 `IsValid` 속성으로 할당 없이 유효성 검사 성공 여부만 가져올 수 있습니다.

```cs
var user = new UserRegistration("john", 20);

Console.WriteLine($"IsValid: {user.IsValid}"); // -> True
```