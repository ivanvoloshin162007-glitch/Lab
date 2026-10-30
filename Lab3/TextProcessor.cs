using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace TextTokenizer
{
    public class TextProcessor
    {
        public IEnumerable<Sentence> SortByWordCount(Text text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            return text.Sentences.OrderBy(s => s.Words.Count());
        }

        public IEnumerable<Sentence> SortByLength(Text text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            return text.Sentences.OrderBy(s => s.Length);
        }

        public IEnumerable<string> GetUniqueWordsInInterrogativeByLength(Text text, int wordLength)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (wordLength <= 0) throw new ArgumentException("Длина слова должна быть больше 0.", nameof(wordLength));

            return text.Sentences
                .Where(s => s.IsInterrogative())
                .SelectMany(s => s.Words)
                .Select(w => w.Value)
                .Where(w => w.Length == wordLength)
                .Distinct(StringComparer.OrdinalIgnoreCase);
        }

        public void RemoveWordsByLengthAndConsonant(Text text, int wordLength)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (wordLength <= 0) throw new ArgumentException("Длина слова должна быть больше 0.", nameof(wordLength));

            foreach (var sentence in text.Sentences)
            {
                sentence.Tokens.RemoveAll(token =>
                {
                    if (token is Word word && word.Value.Length == wordLength)
                    {
                        char firstChar = char.ToLowerInvariant(word.Value[0]);
                        return Constants.RussianConsonants.Contains(firstChar)
                            || Constants.EnglishConsonants.Contains(firstChar);
                    }
                    return false;
                });
            }
        }

        public void ReplaceWordsInSentence(Text text, int sentenceIndex, int wordLength, string replacement)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (sentenceIndex < 0 || sentenceIndex >= text.Sentences.Count)
                throw new ArgumentOutOfRangeException(nameof(sentenceIndex), "Индекс предложения вне диапазона.");
            if (wordLength <= 0) throw new ArgumentException("Длина слова должна быть больше 0.", nameof(wordLength));
            if (replacement == null) throw new ArgumentNullException(nameof(replacement));

            var sentence = text.Sentences[sentenceIndex];
            for (int i = 0; i < sentence.Tokens.Count; i++)
            {
                if (sentence.Tokens[i] is Word word && word.Value.Length == wordLength)
                    sentence.Tokens[i] = new Word(replacement);
            }
        }

        public void RemoveStopWords(Text text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            foreach (var sentence in text.Sentences)
            {
                sentence.Tokens.RemoveAll(token =>
                {
                    if (token is Word word)
                        return Constants.RussianStopWords.Contains(word.Value)
                            || Constants.EnglishStopWords.Contains(word.Value);
                    return false;
                });
            }
        }

        public void SerializeToXml(Text text, string filePath)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var xmlText = new XmlTextModel(text);
            var serializer = new XmlSerializer(typeof(XmlTextModel));

            var settings = new System.Xml.XmlWriterSettings
            {
                Indent = true,
                Encoding = System.Text.Encoding.UTF8,
                OmitXmlDeclaration = false
            };

            using (var writer = System.Xml.XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument(true);
                serializer.Serialize(writer, xmlText);
                writer.WriteEndDocument();
            }
        }
    }

    [XmlRoot("text")]
    public class XmlTextModel
    {
        [XmlElement("sentence")]
        public List<XmlSentenceModel> Sentences { get; set; } = new List<XmlSentenceModel>();

        public XmlTextModel() { }

        public XmlTextModel(Text text)
        {
            foreach (var s in text.Sentences)
                Sentences.Add(new XmlSentenceModel(s));
        }
    }

    public class XmlSentenceModel
    {
        [XmlElement("word")]
        public List<string> Words { get; set; } = new List<string>();

        public XmlSentenceModel() { }

        public XmlSentenceModel(Sentence sentence)
        {
            foreach (var word in sentence.Words)
                Words.Add(word.Value);
        }
    }
}