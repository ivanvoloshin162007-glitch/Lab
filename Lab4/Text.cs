using System.Collections.Generic;

namespace TextTokenizer
{
    public class Text
    {
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();

        public Concordance BuildConcordance()
        {
            var concordance = new Concordance();
            foreach (var sentence in Sentences)
                foreach (var word in sentence.Words)
                    concordance.Add(word.Value, word.LineNumber);
            return concordance;
        }
    }
}
