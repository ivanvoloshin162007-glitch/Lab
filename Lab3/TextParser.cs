using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TextTokenizer
{
    public class TextParser
    {
        private static readonly Regex TokenRegex =
            new Regex(Constants.TokenRegexPattern, RegexOptions.Compiled);

        public Text ParseFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден: {filePath}");

            string content = File.ReadAllText(filePath, System.Text.Encoding.UTF8);

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Файл пуст или содержит только пробелы.", nameof(filePath));

            return ParseText(content);
        }

        public Text ParseText(string input)
        {
            var text = new Text();
            var matches = TokenRegex.Matches(input);
            var currentSentenceTokens = new List<Token>();

            foreach (Match match in matches)
            {
                string val = match.Value;
                if (string.IsNullOrEmpty(val)) continue;

                if (char.IsLetterOrDigit(val[0]) || val.Contains('-') && val.Length > 1 && char.IsLetterOrDigit(val[0]))
                {
                    currentSentenceTokens.Add(new Word(val));
                }
                else
                {
                    currentSentenceTokens.Add(new Punctuation(val));

                    bool isTerminal = val.Contains('.') || val.Contains('!') || val.Contains('?');

                    if (isTerminal)
                    {
                        bool isAbbreviation = false;
                        if (val == "." && currentSentenceTokens.Count >= 2)
                        {
                            var prevToken = currentSentenceTokens[currentSentenceTokens.Count - 2];
                            if (prevToken is Word w && w.Value.Length <= 3)
                                isAbbreviation = true;
                        }

                        if (!isAbbreviation)
                        {
                            var sentence = new Sentence { Tokens = new List<Token>(currentSentenceTokens) };
                            text.Sentences.Add(sentence);
                            currentSentenceTokens.Clear();
                        }
                    }
                }
            }

            if (currentSentenceTokens.Count > 0)
                text.Sentences.Add(new Sentence { Tokens = currentSentenceTokens });

            return text;
        }
    }
}