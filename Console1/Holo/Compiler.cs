using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;

namespace Holo
{
    /// <summary>
    /// Holo Language Compiler
    /// </summary>
    public class Compiler
    {
        Stack<string> orderList;
        Dictionary<string, string> Vars; // type value

        Type funcs;
        object funcInstance;

        public Compiler()
        {
            Init();
        }

        private void Init()
        {
            orderList = new Stack<string>();

            funcs = typeof(CompilerFun);
            funcInstance = Activator.CreateInstance(typeof(CompilerFun));
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
                    MethodInfo mi = funcs.GetMethod(part[0], BindingFlags.NonPublic | BindingFlags.Instance);
                    object[] param = new object[1];
                    param[0] = part;
                    mi.Invoke(funcInstance, param);
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

        private class CompilerFun
        {
            private void reg(string[] part)
            {
                if (part.Length > 1)
                {
                    for (int i = 1; i < part.Length; i++)
                    {
                        if (TryToRegister(part[i]))
                            Console.WriteLine(part[i] + " registered. Value: 0");
                        else
                            Console.WriteLine(part[i] + " can't be registered.\nPlease make sure the item is linked and input the correct name.");
                    }
                }
                else
                    Console.WriteLine("Commend Error.\nThis function need two params at least.");
            }

            private bool TryToRegister(string item)
            {
                return true;
            }
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

            OPERATOR.Add("+");
            OPERATOR.Add("-");
            OPERATOR.Add("*");
            OPERATOR.Add("\\");

            LOGIC.Add("&&");
            LOGIC.Add("||");
            LOGIC.Add("!");
            LOGIC.Add("~");

            WORDS.AddRange(OPT);
            WORDS.AddRange(OPERATOR);
            WORDS.AddRange(LOGIC);
        }

        public bool IsKeyWord(string source)
        {
            return WORDS.Contains(source);
        }
    }
}
