using System;
using System.Collections.Generic;

namespace LinearProgramming
{
    public class TableClass
    {
        private int basicVariablesCount;
        private int freeVariablesCount;
        private int maxCellWidth = 3;

        List<string> basicVariables = new List<string>();
        List<string> freeVariables = new List<string>();
        List<List<NumberClass>> myTable = new List<List<NumberClass>>();

        public TableClass(int basicVariablesCount, int freeVariablesCount)
        {
            this.basicVariablesCount = basicVariablesCount;
            this.freeVariablesCount = freeVariablesCount;

            for (int i = 0; i < basicVariablesCount + 1; i++)
            {
                if (i == 0)
                {
                    basicVariables.Add("1");

                }
                else
                {
                    basicVariables.Add("-X" + i);
                } 
            }

            for (int i = basicVariablesCount; i < freeVariablesCount + basicVariablesCount; i++)
            {
                if (i == basicVariablesCount)
                {
                    freeVariables.Add("q");

                }
                else
                {
                    freeVariables.Add("X" + i);
                }
            }

            List<NumberClass> row1 = new List<NumberClass>() { new NumberClass("0"), new NumberClass("1"), new NumberClass("-1") };
            List<NumberClass> row2 = new List<NumberClass>() { new NumberClass("-6"), new NumberClass("-1"), new NumberClass("-4") };
            List<NumberClass> row3 = new List<NumberClass>() { new NumberClass("10"), new NumberClass("2"), new NumberClass("1") };

            myTable.Add(row1);
            myTable.Add(row2);
            myTable.Add(row3);

            //for (int i = 0; i < freeVariablesCount; i++)
            //{
            //    List<NumberClass> row = new List<NumberClass>();
            //    if (i == 0)
            //    {
            //        Console.WriteLine("________________________________");
            //        Console.WriteLine("Введите значение для строки q.");

            //    }
            //    else
            //    {
            //        Console.WriteLine("________________________________");
            //        Console.WriteLine("Введите значение для строки X" + (i + basicVariablesCount + 1) + ".");
            //    }      
            //    for (int j = 0; j < basicVariablesCount + 1; j++)
            //    {
            //        if (j == 0)
            //        {
            //            Console.WriteLine("Введите значение для столбца 1: ");
            //            row.Add(new NumberClass(Console.ReadLine()));
            //        }
            //        else
            //        {
            //            Console.WriteLine("Введите значение для столбца X" + j + ": ");
            //            row.Add(new NumberClass(Console.ReadLine()));
            //        }
            //    }
            //    myTable.Add(row);
            //}
        }

        public void PrintTable()
        {
            GetMaxCellWidth();

            Console.WriteLine();
            Console.Write("   | ");

            for (int i = 0; i < basicVariablesCount + 1; i++)
            {
                for (int j = 0; j < CountFreeSpaceForVatiablesItems(i); j++)
                {
                    Console.Write(" ");
                }
                Console.Write(basicVariables[i] + " | ");
            }

            PrintUnderline();

            for (int i = 0; i < freeVariablesCount; i++)
            {
                if (freeVariables[i] == "q")
                {
                    Console.Write(" q | ");
                    for (int j = 0; j < basicVariablesCount + 1; j++)
                    {
                        PrintRow(i, j);
                    }
                    PrintUnderline();
                }
                else
                {
                    Console.Write(freeVariables[i] + " | ");
                    for (int j = 0; j < basicVariablesCount + 1; j++)
                    {
                        PrintRow(i, j);
                    }
                    PrintUnderline();
                }
            }
        }

        private void GetMaxCellWidth()
        {
            for (int i = 0; i < freeVariablesCount; i++)
            {
                for (int j = 0; j < basicVariablesCount + 1; j++)
                {
                    if (myTable[i][j].GetLenght() > maxCellWidth)
                    {
                        maxCellWidth = myTable[i][j].GetLenght();
                    }
                }
            }
        }

        private int CountFreeSpaceForTableItems(int i, int j)
        {
            return maxCellWidth - myTable[i][j].GetLenght();
        }

        private int CountFreeSpaceForVatiablesItems(int i)
        {
            return maxCellWidth - basicVariables[i].Length;
        }

        private void PrintRow(int i, int j)
        {
            Console.Write(myTable[i][j]);
            for (int k = 0; k < CountFreeSpaceForTableItems(i, j); k++)
            {
                Console.Write(" ");
            }
            Console.Write(" | ");
        }

        private void PrintUnderline()
        {
            Console.Write("\n---+");
            for (int i = 0; i < basicVariablesCount + 1; i++)
            {
                for (int j = 0; j < maxCellWidth + 2; j++)
                {
                    Console.Write("-");
                }
                Console.Write("+");
            }
            Console.WriteLine();
        }
    }
}
