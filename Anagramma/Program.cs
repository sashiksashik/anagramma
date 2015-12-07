using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Anagramma
{
    class Program
    {
        static void Main(string[] args)
        {
            String data = "";
            StringBuilder helpMenu = new StringBuilder();
            helpMenu.Append("Программа может принимать 2 или 3 аргумента:\n");
            helpMenu.Append(" 1. Файл со словами.\n");
            helpMenu.Append(" 2. Файл для вывода\n");
            helpMenu.Append(" 3. Кодировка файла:\n");
            foreach (EncodingInfo enc in Encoding.GetEncodings())
            {
                helpMenu.Append("   - " + enc.Name + "\n");
            }
            if (args.Length < 2) Console.WriteLine(helpMenu.ToString());
            if (args.Length == 2) data = File.ReadAllText(args[0]);
            if (args.Length == 3) data = File.ReadAllText(args[0], Encoding.GetEncoding(args[2]));
            if (!data.Equals("")) doWork(data, args[1]);
            Console.WriteLine("Нажмите ENTER чтобы закрыть программу.\n");
            Console.ReadLine();
        }

        private static void doWork(String data, String output)
        {
            Dictionary<int, List<String>> dic = new Dictionary<int, List<String>>();//Словарь по хэшу(метод getStringHash) слова в качестве ключа  и списку слов,соответствующих ему, в качестве значения.
            Regex regex = new Regex("[\\w\\-]+");
            MatchCollection words = regex.Matches(data);
            foreach (Match m in words)//Заполняем словарь
            {
                int hash = getStringHash(m.Value);
                if (dic.ContainsKey(hash)) dic[hash].Add(m.Value);
                else
                {
                    List<String> list = new List<string>();
                    list.Add(m.Value);
                    dic.Add(hash, list);
                }
            }
            var e = dic.GetEnumerator();
            StringBuilder str = new StringBuilder();
            while (e.MoveNext())
            {
                foreach (String s in e.Current.Value) str.Append(s + " ");
                str.Append("\r\n");
            }
            if(output.Contains("\\"))Directory.CreateDirectory(output.Substring(0,output.LastIndexOf('\\')));
            if (output.Contains("/")) Directory.CreateDirectory(output.Substring(0, output.LastIndexOf('/')));
            File.WriteAllText(output, str.ToString());

        }

        private static int getStringHash(String str)
        {
            int hash = 0;
            Dictionary<char, uint> dic = new Dictionary<char, uint>();
            foreach (char ch in str)
            {
                if (dic.ContainsKey(ch)) dic[ch]++; else dic.Add(ch, 1); //считаем сколько раз в слове встречаются буквы, составляющие его.
            }
            var e = dic.GetEnumerator();
            while (e.MoveNext())
            {
                hash ^= e.Current.Key * (int)e.Current.Value * 11;//Если сложить множество всех букв или по порядку в слове, то получим что "кок"="тот" или "кот"="ток"="кошка".
            }
            return hash;
        }
    }

}
