using System;

namespace LinearProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            //NumberClass number1 = new NumberClass("-123/23");
            //NumberClass number2 = new NumberClass("0");
            //NumberClass number3 = new NumberClass("10");
            //NumberClass number4 = new NumberClass("-4");
            //NumberClass number5 = new NumberClass("4/5");

            //Console.WriteLine(number1.ToString());
            //Console.WriteLine(number2.ToString());
            //Console.WriteLine(number3.ToString());
            //Console.WriteLine(number4.ToString());
            //Console.WriteLine(number5.ToString());

            TableClass tableClass = new TableClass();
            LogicClass logicClass = new LogicClass(tableClass);

            Console.ReadLine();
        }
    }
}
