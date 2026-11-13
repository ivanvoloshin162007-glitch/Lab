using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextTokenizer
{
    public class ConcordanceEntry
    {
        public string Word { get; }
        public int Count { get; private set; }
        public SortedSet<int> Lines { get; } = new SortedSet<int>();

        public ConcordanceEntry(string word)
        {
            Word = word;
        }

        public void Register(int lineNumber)
        {
            Count++;
            Lines.Add(lineNumber); 
        }

        public override string ToString() => $"{Count}: {string.Join(" ", Lines)}";
    }

    public class Concordance
    {
        private readonly Dictionary<string, ConcordanceEntry> _entries =
            new Dictionary<string, ConcordanceEntry>(StringComparer.OrdinalIgnoreCase);

        public int Count => _entries.Count;

        public void Add(string word, int lineNumber)
        {
            if (string.IsNullOrEmpty(word)) return;

            if (!_entries.TryGetValue(word, out var entry))
            {
                entry = new ConcordanceEntry(word.ToLowerInvariant());
                _entries[word] = entry;
            }
            entry.Register(lineNumber);
        }

        public bool TryGetEntry(string word, out ConcordanceEntry? entry)
            => _entries.TryGetValue(word, out entry);

        public IEnumerable<ConcordanceEntry> Entries =>
            _entries.Values.OrderBy(e => e.Word, StringComparer.Ordinal);

        public override string ToString()
        {
            if (_entries.Count == 0) return string.Empty;

            int width = _entries.Values.Max(e => e.Word.Length) + 4;
            var sb = new StringBuilder();
            foreach (var e in Entries)
                sb.AppendLine(e.Word.PadRight(width, '.') + e);
            return sb.ToString().TrimEnd();
        }
    }
}
