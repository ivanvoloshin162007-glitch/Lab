namespace TextTokenizer
{
    public abstract class Token
    {
        public string Value { get; set; }

        protected Token(string value)
        {
            Value = value ?? string.Empty;
        }

        public override string ToString() => Value;
    }
}