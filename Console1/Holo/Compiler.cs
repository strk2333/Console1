using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Holo
{
    /// <summary>
    /// Holo On Light Off
    /// </summary>
    public class Compiler
    {
        Stack<string> orderList;
        Dictionary<string, string> Vars; // type value

        public Compiler()
        {
            Init();
        }

        private void Init()
        {
            orderList = new Stack<string>();
        }

        public void Start()
        {
            List<char> cache = new List<char>();
            Console.WriteLine("---------- please input your code ----------");

            while (true)
            {
                int input;
                while ((input = Console.Read()) != '\r' && input != '\n')
                {
                    cache.Add((char)input);
                }

                if (cache.Count != 0)
                    orderList.Push(new string(cache.ToArray()).Trim().ToLower());
                
                if (orderList.Peek() == "end" || orderList.Peek() == "e")
                {
                    Console.WriteLine("-------------- start compile --------------");
                    Compile();
                    break;
                }
                cache.Clear();
            }
        }

        private void Compile()
        {
            ReverseStack(ref orderList);
            foreach (string s in orderList)
            {
                string[] part = s.Split(' ');

                if (KeyWord._instance.OPT.Contains(part[0]))
                {
                    if (part.Length > 1)
                        Console.WriteLine("Register " + part[1] + ".");
                    else
                        Console.WriteLine("Err");
                }
            }
        }

        private void ReverseStack(ref Stack<string> s)
        {
            Stack<string> tmp = new Stack<string>();
            while (s.Count != 0)
            {
                tmp.Push(s.Pop());
            }
            s = tmp;
        }
    }

    public class KeyWord
    {
        public static KeyWord _instance = new KeyWord();
        public List<string> WORDS = new List<string>();
        public List<string> OPT = new List<string>();
        public List<string> OPERATOR = new List<string>();
        public List<string> LOGIC = new List<string>();

        private KeyWord()
        {
            Init();
        }

        private void Init()
        {
            OPT.Add("reg");
        }
    }
}
