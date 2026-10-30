using System;
using System.Collections.Generic;

namespace TextTokenizer
{
    public static class Constants
    {
        public const string InputPath = "input.txt";
        public const string OutputPath = "output.xml";

        public const string DefaultInputText =
            "Mary had a little lamb. Is this a test? " +
            "Aesop and the clever wolf went to run, but stopped! " +
            "Где же наш котенок? Мама мыла раму, а папа читал книгу.";

        public const string TokenRegexPattern =
            @"([a-zA-Zа-яА-ЯёЁ0-9]+(?:-[a-zA-Zа-яА-ЯёЁ0-9]+)*)|([.,!?;:«»""()—\-_…]+)";

        public const int DemoWordLengthForUnique = 4;
        public const int DemoWordLengthToRemove = 3;
        public const int DemoSentenceIndexForReplace = 1;
        public const int DemoWordLengthToReplace = 2;
        public const string DemoReplacement = "[РПЛ]";

        public static readonly HashSet<string> RussianStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "и", "а", "но", "да", "или", "ли", "не", "ни", "в", "во", "на", "с", "со", "к", "ко",
            "о", "об", "обо", "по", "за", "у", "от", "до", "из", "ис", "около", "про", "для",
            "оно", "он", "она", "они", "мы", "вы", "ты", "я", "же", "ли", "бы", "что", "чтобы",
            "как", "так", "то", "тут", "там", "еще", "уже", "быть", "есть", "был", "была", "были"
        };
       
        public static readonly HashSet<string> EnglishStopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "the", "a", "an", "and", "but", "or", "for", "on", "at", "by", "from", "up", "about",
            "into", "over", "after", "to", "of", "in", "is", "are", "was", "were", "be", "been",
            "that", "this", "it", "he", "she", "they", "we", "you", "i", "not", "with", "as"
        };

        public static readonly HashSet<char> RussianConsonants = new HashSet<char>
        {
            'б', 'в', 'г', 'д', 'ж', 'з', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ'
        };

        public static readonly HashSet<char> EnglishConsonants = new HashSet<char>
        {
            'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z'
        };
    }
}