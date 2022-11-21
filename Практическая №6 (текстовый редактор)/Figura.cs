using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Практическая__6__текстовый_редактор_
{
    public class Figura
    {
        public Figura()
        {

        }
        public Figura(string Name, int Height, int Width)
        {
            name = Name;
            width = Width;
            height = Height;
        }
        public int height;
        public int width;
        public string name;
    }
    public class Strelka
    {

    }
    public class Edit
    {
        public static string EditFile(string data)
        {
            List<string> lines = data.Split("\n").ToList();
            int pos = 1;
            int max_pos = lines.Count();
            bool change = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Изменение файла. Стрелка вправо для изменения строки, Enter для добавления новой");
                foreach (string line in lines)
                {
                    Console.WriteLine("   " + line);
                }
                if (change)
                {
                    Console.SetCursorPosition(3, pos);
                    lines[pos - 1] = Console.ReadLine();
                    change = false;
                }
                Console.SetCursorPosition(0, pos);
                Console.Write("=>");
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (max_pos > pos)
                            pos += 1;
                        else
                            pos = 1;
                        break;
                    case ConsoleKey.UpArrow:
                        if (pos > 1)
                            pos -= 1;
                        else
                            pos = max_pos;
                        break;
                    case ConsoleKey.RightArrow:
                        lines[pos - 1] = "";
                        change = true;
                        break;
                    case ConsoleKey.F1:
                        string response = "";
                        foreach (string element in lines)
                        {
                            response += element + "\n";
                        }
                        return response;
                }
            }
            return data;
        }
    }
    public class Converters
    {
        public static string ToTxt(List<Figura> data)
        {
            string response = "";
            foreach (Figura figura in data)
            {
                response = response + figura.name + "\n";
                response = response + figura.height + "\n";
                response = response + figura.width + "\n";
            }
            return response;
        }
        public static string ToXml(List<Figura> data)
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Figura>));
            using (FileStream fs = new FileStream("4ch.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, data);
            }
            string response = File.ReadAllText("4ch.xml");
            File.Delete("4ch.xml");
            return response;
        }
        public static string ToJson(List<Figura> data)
        {            
            return JsonConvert.SerializeObject(data);
        }
        public static string Serialize(string path, string data)
        {
            string response = "";
            List<Figura> bams = FromTxt(data);
            switch (path.Split(".")[^1])
            {
                case "json":
                    response = ToJson(bams);
                    break;
                case "xml":
                    response = ToXml(bams);
                    break;
                default:
                    response = ToTxt(bams);
                    break;
            }
            return response;
        }
        public static List<Figura> Deserialize(string path)
        {
            List<Figura> figures = new List<Figura>();
            string data = File.ReadAllText(path);
            switch (path.Split('.')[^1])
            {
                case "json":
                    figures = FromJson(data);
                    break;
                case "xml":
                    figures = FromXml(path);
                    break;
                default:
                    figures = FromTxt(data);
                    break;
            }
            return figures;
        }
        private static List<Figura> FromTxt(string data)
        {
            List<Figura> response = new List<Figura>();
            List<string> lines = data.Split("\n").ToList();
            lines.RemoveAll(x => x == "");
            for (int i = 0; i<lines.Count; i += 3)
            {
                string name;
                int width;
                int height;
                try
                {
                    name = lines[i];
                    width = Convert.ToInt32(lines[i + 1]);
                    height = Convert.ToInt32(lines[i + 2]);
                }
                catch {
                    break;
                }
                Figura figura = new Figura()
                {
                    name = name,
                    width = width,
                    height = height 
                };

                response.Add(figura);
            }
            return response;
        }
        private static List<Figura> FromJson(string data)
        {
            List<Figura> response;
            response = JsonConvert.DeserializeObject<List<Figura>>(data);
            return response;
        }
        private static List<Figura> FromXml(string path)
        {
            List<Figura> response;
            XmlSerializer xml = new XmlSerializer(typeof(List<Figura>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                response = (List<Figura>)xml.Deserialize(fs);
            }
            return response;
        }
    }
}
