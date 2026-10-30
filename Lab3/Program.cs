using System;
using System.IO;

namespace TextTokenizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            ProgramTester.RunAllTests();

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Запуск основной работы приложения...");

            if (!File.Exists(Constants.InputPath))
            {
                File.WriteAllText(Constants.InputPath, Constants.DefaultInputText,
                    System.Text.Encoding.UTF8);
                Console.WriteLine($"Создан файл по умолчанию: {Constants.InputPath}");
            }

            try
            {
                Console.WriteLine("=== Запуск демонстрации токенизации текста ===");

                var parser = new TextParser();
                var text = parser.ParseFile(Constants.InputPath);
                var processor = new TextProcessor();

                Console.WriteLine("\n1. Предложения в порядке возрастания количества слов:");
                foreach (var s in processor.SortByWordCount(text))
                    Console.WriteLine($"  [Слов: {s.Words.Count()}] {s}");

                Console.WriteLine("\n2. Предложения в порядке возрастания длины (в символах):");
                foreach (var s in processor.SortByLength(text))
                    Console.WriteLine($"  [Символов: {s.Length}] {s}");

                int targetLength = Constants.DemoWordLengthForUnique;
                Console.WriteLine($"\n3. Уникальные слова длиной {targetLength} в вопросительных предложениях:");
                var uniqueWords = processor.GetUniqueWordsInInterrogativeByLength(text, targetLength);
                Console.WriteLine($"  Результат: {string.Join(", ", uniqueWords)}");

                Console.WriteLine($"\n4. Удаление слов длиной {Constants.DemoWordLengthToRemove}, начинающихся с согласной:");
                processor.RemoveWordsByLengthAndConsonant(text, Constants.DemoWordLengthToRemove);
                foreach (var s in text.Sentences)
                    Console.WriteLine($"  -> {s}");

                Console.WriteLine($"\n5. Замена слов длиной {Constants.DemoWordLengthToReplace} " +
                                  $"в предложении с индексом {Constants.DemoSentenceIndexForReplace} " +
                                  $"на подстроку '{Constants.DemoReplacement}':");
                processor.ReplaceWordsInSentence(text, Constants.DemoSentenceIndexForReplace,
                    Constants.DemoWordLengthToReplace, Constants.DemoReplacement);
                if (text.Sentences.Count > Constants.DemoSentenceIndexForReplace)
                    Console.WriteLine($"  -> {text.Sentences[Constants.DemoSentenceIndexForReplace]}");

                Console.WriteLine("\n6. Удаление стоп-слов из текста:");
                processor.RemoveStopWords(text);
                foreach (var s in text.Sentences)
                    Console.WriteLine($"  -> {s}");

                Console.WriteLine($"\n7. Сериализация текста в XML (сохранение в {Constants.OutputPath})...");
                processor.SerializeToXml(text, Constants.OutputPath);
                Console.WriteLine("XML успешно сформирован и сохранен!");

                Console.WriteLine("\n=== Демонстрация завершена успешно ===");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nОшибка: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}