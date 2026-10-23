using System;
using System.IO;

namespace CatAndMouse
{
    static class GameTests
    {
        public static bool RunAll()
        {
            Console.WriteLine("===== РЕЗУЛЬТАТ ТЕСТА =====");

            bool caught = Check("Кот поймал мышь", "10", new[] { "M 5", "C 2", "C 3" }, "Mouse caught at:  5");
            bool evaded = Check("Мышь убежала", "10", new[] { "M 5", "C 2", "M 3" }, "Mouse evaded Cat");

            bool ok = caught && evaded;
            Console.WriteLine();
            Console.WriteLine(ok ? ">>> Программа работает ПРАВИЛЬНО"
                                 : ">>> Программа работает НЕПРАВИЛЬНО");
            return ok;
        }

        static bool Check(string name, string size, string[] commands, string expected)
        {
            string log = Play(size, commands);
            bool ok = log.Contains(expected);
            Console.WriteLine("{0}: {1}", ok ? "[ OK ]" : "[FAIL]", name);
            Console.WriteLine("   команды:  " + string.Join(", ", commands));
            Console.WriteLine("   ожидалось: " + expected);
            Console.WriteLine("   в логе:    " + (ok ? "найдено" : "НЕ найдено"));
            return ok;
        }

        static string Play(string size, string[] commands)
        {
            string oldIn = Game.InputFile, oldOut = Game.OutFile;
            Game.InputFile = Path.GetTempFileName();
            Game.OutFile = Path.GetTempFileName();
            File.WriteAllText(Game.InputFile, size + Environment.NewLine + string.Join(Environment.NewLine, commands));
            new Game(int.Parse(size)).Run();
            string result = File.ReadAllText(Game.OutFile);
            File.Delete(Game.InputFile);
            File.Delete(Game.OutFile);
            Game.InputFile = oldIn;
            Game.OutFile = oldOut;
            return result;
        }
    }
}