using App.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App
{   
    static partial class Program
    {
        static async Task<string> ChooseCommandAsync(string command)
        {
            switch (command)
            {
                case "exit":
                    return "exit";
                default:
                    {
                        Console.WriteLine($"Вы ввели: {command}");
                        System.Console.WriteLine(await GetRelativeWordsAsync(_url, command));
                        NextLine();
                        break;
                    }
            }
            return "exit";
        }

        static string ReadLineUTF()
        {
            var currentWord = new StringBuilder();
            ConsoleKeyInfo currentKey = new ConsoleKeyInfo();

            do
            {
                currentKey = Console.ReadKey();

                if (currentKey.Key.IsEscape())
                    return "exit";
                else if (currentKey.Key.IsBackspace())
                    currentWord.PopCharacter();
                else if (!currentKey.Key.IsEnter())
                    currentWord.Append(currentKey.KeyChar.ToString());

            } while (!currentKey.Key.IsEnter());

            if (!InputValidator.IsValid(currentWord.ToString()))
                return "exit";

            return currentWord.ToString();
        }

        static void OutputInfo(string url)
        {
            Console.WriteLine($"Конечный хост отправки: {url}");
        }

        static void NextLine() => Console.WriteLine();
    }
}
