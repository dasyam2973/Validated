using System;
using System.Text;

namespace Validated.Generator.Utilities;

public sealed class IndentedStringBuilder
{
    private readonly StringBuilder _sb = new();
    private int _indentLevel = 0;

    public void Line(string text = "")
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            _sb.AppendLine();
            return;
        }

        _sb.Append(' ', _indentLevel * 4);
        _sb.AppendLine(text);
    }

    public IDisposable Block(string header = "")
    {
        if (!string.IsNullOrEmpty(header))
        {
            Line(header);
        }
        Line("{");
        _indentLevel++;
        return new BlockScope(this);
    }

    public IDisposable Indent()
    {
        _indentLevel++;
        return new IndentScope(this);
    }

    public override string ToString() => _sb.ToString();

    readonly struct BlockScope : IDisposable
    {
        private readonly IndentedStringBuilder _builder;
        public BlockScope(IndentedStringBuilder builder) => _builder = builder;

        public void Dispose()
        {
            _builder._indentLevel--;
            _builder.Line("}");
        }
    }

    readonly struct IndentScope : IDisposable
    {
        private readonly IndentedStringBuilder _builder;
        public IndentScope(IndentedStringBuilder builder) => _builder = builder;

        public void Dispose()
        {
            _builder._indentLevel--;
        }
    }
}
