using System;

namespace LinearProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            //NumberClass number1 = new NumberClass("3/20");
            //NumberClass number2 = new NumberClass("-2/5");
            //////NumberClass number4 = new NumberClass("-4");
            //////NumberClass number5 = new NumberClass("4/5");

            //Console.WriteLine(number1 + number2);
            ////Console.WriteLine(number2.ToString());
            ////Console.WriteLine(number3.ToString());
            ////Console.WriteLine(number4.ToString());
            ////Console.WriteLine(number5.ToString());

            TableClass tableClass = new TableClass();
            LogicClass logicClass = new LogicClass(tableClass);

            Console.ReadLine();
        }
    }
}
