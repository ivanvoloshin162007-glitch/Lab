using System;
using System.IO;
using System.Linq;

namespace TextTokenizer
{

    public static class ProgramTester
    {
        public static void RunAllTests()
        {
            Console.WriteLine("=== ЗАПУСК ТЕСТОВ ПРОГРАММЫ ===");

            var parser = new TextParser();
            var processor = new TextProcessor();

            try
            {
                var text = parser.ParseText("Is this a test? Mary had a lamb. Где наш кот?");

                assert(text.Sentences.Count == 3, "Ошибка парсинга: неверное число предложений.");
                Console.WriteLine(" [OK] Парсинг текста");

                assert(text.Sentences[0].IsInterrogative(), "Ошибка: первое предложение должно быть вопросительным.");
                assert(!text.Sentences[1].IsInterrogative(), "Ошибка: второе предложение не должно быть вопросительным.");
                Console.WriteLine(" [OK] Определение вопросов");

                processor.ReplaceWordsInSentence(text, 1, 3, "[РЕПЛ]");
                assert(text.Sentences[1].ToString().Contains("[РЕПЛ]"), "Ошибка замены слов.");
                Console.WriteLine(" [OK] Замена слов в предложении");

                var stopText = parser.ParseText("This is a test.");
                processor.RemoveStopWords(stopText);
                assert(stopText.Sentences[0].Words.Count() == 1, "Ошибка удаления стоп-слов.");
                Console.WriteLine(" [OK] Удаление стоп-слов");

                string xmlFile = "test_out.xml";
                processor.SerializeToXml(text, xmlFile);
                assert(File.Exists(xmlFile), "Ошибка: XML-файл не создан.");
                if (File.Exists(xmlFile)) File.Delete(xmlFile);
                Console.WriteLine(" [OK] Сериализация в XML");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n ВСЕ ТЕСТЫ УСПЕШНО ПРОЙДЕНЫ!\n");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n ТЕСТ ПРОВАЛЕН: {ex.Message}\n");
                Console.ResetColor();
            }
        }

        private static void assert(bool condition, string message)
        {
            if (!condition) throw new Exception(message);
        }
    }
}