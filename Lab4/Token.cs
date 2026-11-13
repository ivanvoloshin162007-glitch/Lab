namespace TextTokenizer
{
    public abstract class Token
    {
        public string Value { get; set; }

        public int LineNumber { get; set; }

        protected Token(string value, int lineNumber = 0)
        {
            Value = value ?? string.Empty;
            LineNumber = lineNumber;
        }

        public override string ToString() => Value;
    }
}
