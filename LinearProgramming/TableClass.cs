﻿using System;
using System.Collections.Generic;

namespace LinearProgramming
{
    public class TableClass
    {
        private int _freeVariablesCount;
        private int _basicVariablesCount;
        private int _maxCellWidth = 3;

        List<string> _freeVariables = new List<string>();
        List<string> _basicVariables = new List<string>();
        List<List<NumberClass>> _tableBeforeCalculations = new List<List<NumberClass>>();
        List<List<NumberClass>> _tableAfterCalculations = new List<List<NumberClass>>();

        List<List<string>> _inputValues = new List<List<string>>()
        {
            new List<string> { "0", "-1", "1" },
            new List<string> { "-12", "-5", "-1" },
            new List<string> { "-15", "-1", "-3" },

            // 11 Вариант
            //new List<string> { "0", "-2", "3" },
            //new List<string> { "1", "-1", "1" },
            //new List<string> { "-6", "2", "-4" },
            //new List<string> { "16", "1", "2" },

            // 18 Вариант
            //new List<string> { "0", "-1", "-3" },
            //new List<string> { "1", "-1", "1" },
            //new List<string> { "-6", "2", "-4" },
            //new List<string> { "16", "1", "2" },

            //new List<string> { "0", "-4", "1" },
            //new List<string> { "2", "-1", "2" },
            //new List<string> { "2", "1", "-1" },

            //new List<string> { "0", "1", "-1" },
            //new List<string> { "-6", "-1", "-4" },
            //new List<string> { "10", "2", "1" },

            //new List<string> { "0", "-1", "1" },
            //new List<string> { "2", "-2", "1" },
            //new List<string> { "2", "1", "-2" },
            //new List<string> { "5",  "1",  "1" }

            //new List<string> { "0", "1", "-1" },
            //new List<string> { "0", "1", "-2" },
            //new List<string> { "-3", "-3", "1" },
            //new List<string> { "8", "1", "1" },
        };

        public int FreeVariablesCount
        {
            get => _freeVariablesCount;
            set => _freeVariablesCount = value;
        }

        public int BasicVariablesCount
        {
            get => _basicVariablesCount;
            set => _basicVariablesCount = value;
        }

        public List<string> FreeVariables
        {
            get => _freeVariables;
            set => _freeVariables = value;
        }

        public List<string> BasicVariables
        {
            get => _basicVariables;
            set => _basicVariables = value;
        }

        public List<List<NumberClass>> TableBeforeCalculations
        {
            get => _tableBeforeCalculations;
            set => _tableBeforeCalculations = value;
        }

        public List<List<NumberClass>> TableAfterCalculations
        {
            get => _tableAfterCalculations;
            set => _tableAfterCalculations = value;
        }

        public List<List<string>> InputValues
        {
            get => _inputValues;
            set => _inputValues = value;
        }

        public TableClass()
        {
            FreeVariablesCount = InputValues[0].Count - 1;
            BasicVariablesCount = InputValues.Count - 1;  

            SetBasicVariablesValues();
            SetFreeVariablesValues();

            for (int i = 0; i < BasicVariablesCount + 1; i++)
            {
                List<NumberClass> line1 = new List<NumberClass>();
                List<NumberClass> line2 = new List<NumberClass>();
                for (int j = 0; j < FreeVariablesCount + 1; j++)
                {
                    line1.Add(new NumberClass(InputValues[i][j]));
                    line2.Add(new NumberClass("0"));
                }
                TableBeforeCalculations.Add(line1);
                TableAfterCalculations.Add(line2);
            }
        }

        public void PrintTable(int selectedColumn, int selectedRow)
        {
            GetMaxCellWidth();
            Console.Write("\n   | ");
            for (int i = 0; i < FreeVariablesCount + 1; i++)
            {
                for (int j = 0; j < CountFreeSpaceForVatiablesItems(i); j++)
                {
                    Console.Write(" ");
                }
                Console.Write("-" + FreeVariables[i] + " | ");
            }
            PrintUnderline();

            for (int i = 0; i < BasicVariablesCount + 1; i++)
            {
                if (BasicVariables[i] == "q")
                {
                    Console.Write(" q | ");
                    for (int j = 0; j < FreeVariablesCount + 1; j++)
                    {
                        PrintFirstRow(i, j);
                    }
                }
                else
                {
                    Console.Write(BasicVariables[i] + " | ");
                    for (int j = 0; j < FreeVariablesCount + 1; j++)
                    {
                        PrintFirstRow(i, j);
                    }                   
                }
                if (i == selectedRow)
                {
                    Console.Write("*");
                }
                Console.Write("\n   | ");
                for (int j = 0; j < FreeVariablesCount + 1; j++)
                {
                    PrintSecondRow(i, j);
                }
                PrintUnderline();
            }
            Console.Write("     ");
            for (int i = 0; i < FreeVariablesCount + 1; i++)
            {
                if (i == selectedColumn)
                {
                    Console.WriteLine("*");
                    return;
                }
                for (int j = 0; j < _maxCellWidth + 3; j++)
                {
                    Console.Write(" ");
                }
            }
        }

        public void PrintTable()
        {
            GetMaxCellWidth();
            Console.Write("\n   | ");
            for (int i = 0; i < FreeVariablesCount + 1; i++)
            {
                for (int j = 0; j < CountFreeSpaceForVatiablesItems(i); j++)
                {
                    Console.Write(" ");
                }
                Console.Write("-" + FreeVariables[i] + " | ");
            }
            PrintUnderline();
            for (int i = 0; i < BasicVariablesCount + 1; i++)
            {
                if (BasicVariables[i] == "q")
                {
                    Console.Write(" q | ");
                    for (int j = 0; j < FreeVariablesCount + 1; j++)
                    {
                        PrintFirstRow(i, j);
                    }
                }
                else
                {
                    Console.Write(BasicVariables[i] + " | ");
                    for (int j = 0; j < FreeVariablesCount + 1; j++)
                    {
                        PrintFirstRow(i, j);
                    }
                }
                PrintUnderline();
            }
            Console.Write("\nОтвет: ");
        }

        private void PrintFirstRow(int i, int j)
        {
            Console.Write(TableBeforeCalculations[i][j]);
            for (int k = 0; k < CountFreeSpaceForTableBeforeCalculatingItems(i, j); k++)
            {
                Console.Write(" ");
            }
            Console.Write(" | ");
        }

        private void PrintSecondRow(int i, int j)
        {
            for (int k = 0; k < CountFreeSpaceForTableAfterCalculatingItems(i, j); k++)
            {
                Console.Write(" ");
            }
            Console.Write(TableAfterCalculations[i][j]);
            Console.Write(" | ");
        }

        private void PrintUnderline()
        {
            Console.Write("\n---+");
            for (int i = 0; i < FreeVariablesCount + 1; i++)
            {
                for (int j = 0; j < _maxCellWidth + 2; j++)
                {
                    Console.Write("-");
                }
                Console.Write("+");
            }
            Console.WriteLine();
        }

        private void SetFreeVariablesValues()
        {
            for (int i = FreeVariablesCount; i < BasicVariablesCount + FreeVariablesCount + 1; i++)
            {
                if (i == FreeVariablesCount)
                {
                    BasicVariables.Add("q");
                }
                else
                {
                    BasicVariables.Add("X" + i);
                }
            }
        }

        private void SetBasicVariablesValues()
        {
            for (int i = 0; i < FreeVariablesCount + 1; i++)
            {
                if (i == 0)
                {
                    FreeVariables.Add("1");
                }
                else
                {
                    FreeVariables.Add("X" + i);
                }
            }
        }

        public NumberClass GetTableBeforeCalculationsItem(int i, int j)
        {
            return TableBeforeCalculations[i][j];
        }

        private void GetMaxCellWidth()
        {
            for (int i = 0; i < BasicVariablesCount + 1; i++)
            {
                for (int j = 0; j < FreeVariablesCount + 1; j++)
                {
                    if (TableBeforeCalculations[i][j].GetLenght() + TableAfterCalculations[i][j].GetLenght() + 1 > _maxCellWidth)
                    {
                        _maxCellWidth = TableBeforeCalculations[i][j].GetLenght() + TableAfterCalculations[i][j].GetLenght() + 1;
                    }
                }
            }
        }

        private int CountFreeSpaceForTableAfterCalculatingItems(int i, int j)
        {
            return _maxCellWidth - TableAfterCalculations[i][j].GetLenght();
        }

        private int CountFreeSpaceForTableBeforeCalculatingItems(int i, int j)
        {
            return _maxCellWidth - TableBeforeCalculations[i][j].GetLenght();
        }

        private int CountFreeSpaceForVatiablesItems(int i)
        {
            return _maxCellWidth - FreeVariables[i].Length - 1;
        }
    }
}
