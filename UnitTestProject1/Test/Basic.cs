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
            if ((result = BigPlus("123", "45")) != null)
                Debug.WriteLine(result);
        }

        private string BigPlus(string n1, string n2)
        {
            int maxLen = Math.Max(n1.Length, n2.Length);
            int minLen = Math.Min(n1.Length, n2.Length);
            int[] crr = new int[maxLen + 1];
            StringBuilder result = new StringBuilder();

            if (!CheckNum(n1) && !CheckNum(n2))
                return null;

            for (int i = 1; i < minLen + 1; i++)
            {
                int num1 = n1[n1.Length - i] - '0';
                int num2 = n2[n2.Length - i] - '0';
                if (num1 + num2 + crr[i - 1] > 10)
                {
                    crr[i] = 1;
                    result.Append((num1 + num2 + crr[i]) % 10);
                }
                else
                {
                    result.Append(num1 + num2 + crr[i]);
                }
            }

            for (int i = maxLen - minLen; i > 0; i--)
            {
                int num1 = n1[i - 1] - '0';
                if (num1 + crr[i - 1] > 10)
                {
                    crr[i] = 1;
                    result.Append((num1 + crr[i - 1]) % 10);
                }
                else
                {
                    result.Append(num1 + crr[i - 1]);
                }
            }

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
            for (int i = 0; i < n.Length; i++)
            {
                if (n[i] < '0' || n[i] > '9')
                    return false;
            }
            return true;
        }
    }
}
