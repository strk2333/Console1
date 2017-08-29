using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console1.WordGame;

namespace Console1
{
    class Program
    {
        static void Main(string[] args)
        {
            //MyClass mc = new MyClass();
            //WordGameController game = new WordGameController();
            //game.StartGame();
            Kits.Cralwer c = new Kits.Cralwer();
            c.Start();
            Console.Read();
        }
    }

    #region async await
    public class MyClass
    {
        public MyClass()
        {
            DisplayValue(); //这里不会阻塞  
            System.Diagnostics.Debug.WriteLine("MyClass() End.");
        }
        public Task<double> GetValueAsync(double num1, double num2)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    System.Diagnostics.Debug.WriteLine("Value is : " + num1);
                    num1 = num1 / num2;
                }
                System.Diagnostics.Debug.WriteLine("Value is : " + num1);
                return num1;
            });
        }
        public async void DisplayValue()
        {
            double result = await GetValueAsync(1234.5, 1.01);//此处会开新线程处理GetValueAsync任务，然后方法马上返回  
                                                              //这之后的所有代码都会被封装成委托，在GetValueAsync任务完成时调用  
            System.Diagnostics.Debug.WriteLine("Value is : " + result);
        }
    }
    #endregion
}
