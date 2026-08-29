using System;
using System.Collections.Generic;
using System.Text;

namespace Validated.Generator.Utilities;

public class MessageFormatter
{
    private readonly Dictionary<string, string> _placeholders = new(StringComparer.OrdinalIgnoreCase);

    public MessageFormatter With(string name, object? value)
    {
        _placeholders[name] = value?.ToString() ?? string.Empty;
        return this;
    }

    public MessageFormatter With(string name, int value)
    {
        _placeholders[name] = value.ToString();
        return this;
    }

    public MessageFormatter With(string name, string value)
    {
        _placeholders[name] = value;
        return this;
    }

    public string Format(string? input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;
        if (_placeholders.Count == 0) return input!;
        return Format(input!.AsSpan());
    }

    public string Format(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty)
            return string.Empty;

        StringBuilder sb = new(input.Length);
        int i = 0;

        while (i < input.Length)
        {
            if (input[i] == '{')
            {
                int closeIndex = input.Slice(i).IndexOf('}');

                if (closeIndex > 1)
                {
                    ReadOnlySpan<char> keySpan = input.Slice(i + 1, closeIndex - 1);
                    string key = keySpan.ToString();

                    if (_placeholders.TryGetValue(key, out string? value))
                    {
                        sb.Append(value);
                        i += closeIndex + 1;
                        continue;
                    }
                }
            }

            sb.Append(input[i]);
            i++;
        }

        return sb.ToString();
    }
}
