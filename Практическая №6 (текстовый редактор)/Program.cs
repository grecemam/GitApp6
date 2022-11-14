using Newtonsoft.Json;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Serialization;

namespace Практическая__6__текстовый_редактор_
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Напишите путь до файла, который вы хотите открыть");
            Console.WriteLine("----------------------------------------------");
            string openedPath = Console.ReadLine();
            List<Figura> figures = Converters.Deserialize(openedPath);
            string edited = Edit.EditFile(Converters.ToTxt(figures));
            Console.Clear();
            Console.WriteLine("Напишите путь до файла, в который вы хотите сохранить");
            Console.WriteLine("----------------------------------------------");
            string savedPath = Console.ReadLine();
            Console.Clear();
            string converted = Converters.Serialize(savedPath, edited);
            Console.WriteLine(edited + "\n" + converted);
            File.WriteAllText(savedPath, converted);

        }
        
    }
}

