using System.Collections.Generic;
using System.Linq;

namespace TextTokenizer
{
    public class Sentence
    {
        public List<Token> Tokens { get; set; } = new List<Token>();

        public IEnumerable<Word> Words => Tokens.OfType<Word>();

        public override string ToString()
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < Tokens.Count; i++)
            {
                var token = Tokens[i];
                if (token is Punctuation)
                {
                    result.Append(token.Value);
                }
                else
                {
                    if (i > 0 && Tokens[i - 1] is not Punctuation)
                    {
                        result.Append(' ');
                    }
                    else if (i > 0 && Tokens[i - 1].Value == "(")
                    {
                        
                    }
                    else if (i > 0)
                    {
                        result.Append(' ');
                    }
                    result.Append(token.Value);
                }
            }
            return result.ToString().Trim();
        }

        public int Length => ToString().Length;

        public bool IsInterrogative()
        {
            var lastPunctuation = Tokens.LastOrDefault(t => t is Punctuation);
            return lastPunctuation != null && lastPunctuation.Value.Contains('?');
        }
    }
}