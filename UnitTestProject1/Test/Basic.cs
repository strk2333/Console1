using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using System.Diagnostics;

namespace UnitTestProject1.Test
{
    [TestClass]
    public class Basic
    {
        [Fact]
        public void BigCalcTest()
        {
            //int i = 1 << 32 - 1;
            string result;
            if ((result = BigPlus("1e12", "1e12")) != null)
                Debug.WriteLine(result);
        }

        private string BigPlus(string n1, string n2)
        {
            string stemp;
            stemp = n1.Length > n2.Length ? n1 : n2;
            n2 = n1.Length > n2.Length ? n2 : n1;
            n1 = stemp;
            int[] crr = new int[n1.Length + 1];
            StringBuilder result = new StringBuilder();

            if (!CheckNum(n1) || !CheckNum(n2))
            {
                Debug.WriteLine("Error: Input is invalid.");
                return null;
            }

            if (n1.Contains('.'))
            {

            }

            // both part
            for (int i = 1; i < n2.Length + 1; i++)
            {
                int num1 = n1[n1.Length - i] - '0';
                int num2 = n2[n2.Length - i] - '0';
                if (num1 + num2 + crr[i - 1] > 9)
                {
                    crr[i] = 1;
                    result.Append((num1 + num2 + crr[i - 1]) % 10);
                }
                else
                {
                    result.Append(num1 + num2 + crr[i - 1]);
                }
            }
            
            // left part
            for (int i = 0; i < n1.Length - n2.Length; i++)
            {
                int num1 = n1[n1.Length - n2.Length - i - 1] - '0';
                if (num1 + crr[i + n2.Length] > 9)
                {
                    crr[i + n2.Length + 1] = 1;
                    result.Append((num1 + crr[i + n2.Length]) % 10);
                }
                else if (num1 + crr[i + n2.Length] > 0)
                {
                    result.Append(num1 + crr[i + n2.Length]);
                }
            }

            // highest bit
            if (crr[n1.Length] == 1)
                result.Append(1);

            // reverse string
            char[] tmp = new char[result.Length];
            for (int i = 0; i < result.Length; i++)
            {
                tmp[i] = result[result.Length - i - 1];
            }

            return new string(tmp);
        }

        // return true if n can be converted to number.
        private bool CheckNum(string n)
        {
            if (n.Split('.').Length > 2)
                return false;

            for (int i = 0; i < n.Length; i++)
            {
                if (n[i] < '0' || n[i] > '9')
                    return false;
            }
            return true;
        }

        // Scientific Notation 
        private string TryConvertFromSN(string s)
        {
            if (!s.Contains('e') && !s.Contains('E'))
                return null;

            string result = "";
            string[] p1;
            string[] p2;

            p1 = s.Split('E');
            p1 = s.Split('e');
            if (p1.Length != 2)
                return null;

            p2 = p1[0].Split('.');
            if (p2.Length > 2)
                return null;

            for (int i = 0; i < p2[0].Length; i++)
            {
                result += p2[0][i];
            }

            if (p2.Length == 2)
            {

            }

            return result;
        }
    }
}
