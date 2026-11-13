using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            string? userInput = AskForInputPath();

            var filesToProcess = ResolveInputFiles(userInput);

            if (filesToProcess.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Не найдено ни одного входного файла. Завершение работы.");
                Console.ResetColor();
                return;
            }

            foreach (var inputFile in filesToProcess)
            {
                Console.WriteLine($"\n===== Обработка файла: {inputFile} =====");
                ProcessFile(inputFile);
            }

            Console.WriteLine("\n=== Работа приложения завершена ===");
        }

        private static string? AskForInputPath()
        {
            Console.WriteLine("\nВведите путь к файлу или папке с .txt файлами.");
            Console.WriteLine($"  (Enter — использовать по умолчанию: {Constants.InputPath})");
            Console.Write("Путь: ");

            string? input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? null : input.Trim().Trim('"');
        }

        private static List<string> ResolveInputFiles(string? userInput)
        {
            var result = new List<string>();

            if (userInput == null)
            {
                if (!File.Exists(Constants.InputPath))
                {
                    File.WriteAllText(Constants.InputPath, Constants.DefaultInputText,
                        System.Text.Encoding.UTF8);
                    Console.WriteLine($"Создан файл по умолчанию: {Constants.InputPath}");
                }
                result.Add(Constants.InputPath);
                return result;
            }

            if (Directory.Exists(userInput))
            {
                var txtFiles = Directory.GetFiles(userInput, "*.txt", SearchOption.TopDirectoryOnly)
                                        .OrderBy(f => f)
                                        .ToList();

                if (txtFiles.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"В папке '{userInput}' нет .txt файлов.");
                    Console.ResetColor();
                    return result;
                }

                Console.WriteLine($"Найдено файлов: {txtFiles.Count}");
                result.AddRange(txtFiles);
                return result;
            }

            if (File.Exists(userInput))
            {
                result.Add(userInput);
                return result;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Путь не найден: '{userInput}'");
            Console.ResetColor();
            return result;
        }

        private static void ProcessFile(string inputFile)
        {
            try
            {
                Console.WriteLine("=== Запуск демонстрации токенизации текста ===");

                var parser = new TextParser();
                var text = parser.ParseFile(inputFile);
                var processor = new TextProcessor();

                // Конкорданс строится по исходному тексту, до заданий 4–6, которые изменяют текст.
                var concordance = text.BuildConcordance();

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

                string outputPath = BuildOutputPath(inputFile);

                Console.WriteLine($"\n7. Сериализация текста в XML (сохранение в {outputPath})...");
                processor.SerializeToXml(text, outputPath);
                Console.WriteLine("XML успешно сформирован и сохранен!");

                Console.WriteLine("\n8. Конкорданс исходного текста (слово, число вхождений: номера строк):");
                Console.WriteLine(concordance);

                Console.WriteLine("\n=== Демонстрация завершена успешно ===");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nОшибка при обработке '{inputFile}': {ex.Message}");
                Console.ResetColor();
            }
        }

        private static string BuildOutputPath(string inputFile)
        {
            if (string.Equals(inputFile, Constants.InputPath, StringComparison.OrdinalIgnoreCase))
                return Constants.OutputPath;

            string dir = Path.GetDirectoryName(inputFile) ?? ".";
            string nameWithoutExt = Path.GetFileNameWithoutExtension(inputFile);
            return Path.Combine(dir, nameWithoutExt + ".xml");
        }
    }
}