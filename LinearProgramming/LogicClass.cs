﻿using System;
using System.Collections.Generic;

namespace LinearProgramming
{
    public class LogicClass
    {
        int count = 0;
        int _selectedRow = -1;
        int _selectedColumn = -1;
        bool _isDone = false;

        NumberClass _zero = new NumberClass("0");
        NumberClass _lambda = new NumberClass("1");       
        Dictionary<int, List<int>> _iterations = new Dictionary<int, List<int>>();
        Dictionary<string, NumberClass> _solution = new Dictionary<string, NumberClass>();

        List<int> minIndexList = new List<int>();

        public bool IsDone
        {
            get => _isDone;
            set => _isDone = value;
        }

        public NumberClass Zero
        {
            get => _zero;
        }

        public NumberClass Lambda
        {
            get => _lambda;
            set => _lambda = value;
        }

        public int SelectedRow
        {
            get => _selectedRow;
            set => _selectedRow = value;
        }

        public int SelectedColumn
        {
            get => _selectedColumn;
            set => _selectedColumn = value;
        }

        public Dictionary<int, List<int>> Iterations
        {
            get => _iterations;
            set => _iterations = value;
        }

        public Dictionary<string, NumberClass> Solution
        {
            get => _solution;
            set => _solution = value;
        }

        public LogicClass(TableClass myTable)
        {
            do
            {
                MainLogigMethod(myTable);
            } while (!IsDone);
            PrintSolution(myTable);
        }

        private void MainLogigMethod(TableClass myTable)
        {
            count++;
            ColumnAndRowSelection(myTable);
            AddIteration();
            myTable.TableAfterCalculations[_selectedRow][_selectedColumn] = GeneralCoefficient(myTable);
            SetNewItemsForSelectedRow(myTable);
            SetNewItemsForSelectedColumn(myTable);
            SetOtherNewItems(myTable);
            myTable.PrintTable(SelectedColumn, SelectedRow);
            UpdateFirstTable(myTable);
            IsDone = CheckTheEndOfTheSolution(myTable);
            if (IsDone)
            {
                IsDone = IsSolutionInteger(myTable);
            }
            SelectedColumn = -1;
            SelectedRow = -1;

            //Console.WriteLine(count);
            //PrintSolution(myTable);
        }

        // Доп. условие вводится в конце этого метода
        private bool CheckTheEndOfTheSolution(TableClass myTable)
        {
            Solution.Clear();
            for (int i = 1; i < myTable.FreeVariablesCount + 1; i++)
            {
                Solution.Add(myTable.FreeVariables[i], new NumberClass("0"));
            }
            for (int i = 1; i < myTable.BasicVariablesCount + 1; i++)
            {
                Solution.Add(myTable.BasicVariables[i], myTable.TableBeforeCalculations[i][0]);
            }
            for (int i = 1; i < myTable.FreeVariablesCount + 1; i++)
            {
                if (myTable.TableBeforeCalculations[0][i].IsNegative)
                {
                    return false;
                }
                if (Solution["X" + i].IsNegative)
                {
                    return false;
                }
            }
            for (int i = myTable.FreeVariablesCount + 1; i < myTable.BasicVariablesCount + myTable.FreeVariablesCount  + 1; i++)
            {
                if (Solution["X" + i].IsNegative)
                {
                    return false;
                }
            }
            for (int i = 0; i < myTable.BasicVariablesCount + 1; i++)
            {
                NumberClass result = new NumberClass("0");

                if (i == 0)
                {
                    for (int j = 1; j < myTable.FreeVariablesCount + 1; j++)
                    {
                        result += Solution["X" + j] * new NumberClass(myTable.InputValues[0][j]) * new NumberClass("-1");
                    }
                    if (result != myTable.TableBeforeCalculations[0][0])
                    {
                        return false;
                    }
                }
                else
                {
                    for (int j = 0; j < myTable.FreeVariablesCount + 1; j++)
                    {
                        result += myTable.TableBeforeCalculations[i][j];
                    }
                    if (result.IsNegative)
                    {
                        return false;
                    }
                }
            }

            //      Доп. условие
            // 
            //      1. Вводите обратное условие, то есть, если условие (... > 1),
            // то для проверки введите (... <= 1)
            //      2. Необходимая переменная с индексом i прописывается
            // следующим образом: Solution["X" + i].ToDouble()
            //      Ниже прописано доп. условие из 18 варианта (X2•Lg(X1) > 1)

            //if (600 / Math.Pow(Solution["X" + 1].ToDouble(), 2) <= Solution["X" + 2].ToDouble())
            //{
            //    return false;
            //}
            return true;
        }

        private void ColumnAndRowSelection(TableClass myTable)
        {
            do
            {
                SelectedColumn = ColumnSelection(myTable);
                if (SelectedColumn != -1)
                {
                    RowSelection(myTable);
                    if (SelectedRow == -1)
                    {
                        SelectedRow = RowSelectionIfAcceptableSolution(myTable);
                        if (SelectedRow == -1)
                        {
                            SelectedColumn++;
                            ColumnAndRowSelection(myTable);
                        }                      
                    }
                    break;
                }
                else
                {
                    //if (AreColumnsPositiveAndRowsNegative(myTable))
                    //{
                        

                    //    break;
                    //}

                    List<NumberClass> line1 = new List<NumberClass>();
                    List<NumberClass> line2 = new List<NumberClass>();

                    if (IsSolutionInteger(myTable))
                    {                        
                        for (int j = 0; j < myTable.FreeVariablesCount + 1; j++)
                        {
                            line1.Add(new NumberClass("-1"));
                            line2.Add(new NumberClass("-1"));
                        }                                          
                    }
                    else
                    {
                        int minIndexOfBasicVariables = myTable.FreeVariablesCount + myTable.BasicVariablesCount + 1;
                        for (int i = 1; i < myTable.BasicVariablesCount + 1; i++)
                        {
                            if (Convert.ToInt32(myTable.BasicVariables[i][1].ToString()) < minIndexOfBasicVariables)
                            {
                                bool flag = true;
                                for (int k = 0; k < minIndexList.Count; k++)
                                {
                                    if (minIndexList[k] == Convert.ToInt32(myTable.BasicVariables[i][1].ToString()))
                                    {

                                        flag = false;
                                    }
                                }
                                if (flag)
                                {
                                    minIndexOfBasicVariables = Convert.ToInt32(myTable.BasicVariables[i][1].ToString());
                                }
                                
                                
                            }
                        }
                        minIndexList.Add(minIndexOfBasicVariables);

                        minIndexOfBasicVariables = myTable.BasicVariables.IndexOf("X" + minIndexOfBasicVariables);
                        for (int j = 0; j < myTable.FreeVariablesCount + 1; j++)
                        {
                            var newItem = new NumberClass(Convert.ToString(Math.Floor(myTable.TableBeforeCalculations[minIndexOfBasicVariables][j].ToDouble())));

                            
                            if (myTable.TableBeforeCalculations[minIndexOfBasicVariables][j].IsNegative)
                            {
                                line1.Add(new NumberClass(Convert.ToString(newItem + myTable.TableBeforeCalculations[minIndexOfBasicVariables][j])));
                                line2.Add(new NumberClass(Convert.ToString(newItem + myTable.TableBeforeCalculations[minIndexOfBasicVariables][j])));
                            }
                            else
                            {
                                line1.Add(new NumberClass(Convert.ToString(newItem - myTable.TableBeforeCalculations[minIndexOfBasicVariables][j])));
                                line2.Add(new NumberClass(Convert.ToString(newItem - myTable.TableBeforeCalculations[minIndexOfBasicVariables][j])));
                            }
                        }                       
                    }
                    int minIndexOfFreeVariable = myTable.BasicVariablesCount + myTable.FreeVariablesCount + 1;
                    for (int i = 1; i < myTable.FreeVariablesCount + 1; i++)
                    {
                        if (Convert.ToInt32(myTable.FreeVariables[i][1].ToString()) < minIndexOfFreeVariable)
                        {
                            SelectedColumn = i;
                            minIndexOfFreeVariable = Convert.ToInt32(myTable.FreeVariables[i][1].ToString());
                        }
                    }

                    SelectedRow = myTable.BasicVariablesCount + myTable.FreeVariablesCount - 1;

                    myTable.TableBeforeCalculations.Add(line1);
                    myTable.TableAfterCalculations.Add(line2);
                    myTable.BasicVariables.Add("X" + (myTable.FreeVariablesCount + ++myTable.BasicVariablesCount));

                    break;
                }
            } while (SelectedColumn < myTable.FreeVariablesCount + 1);
        }

        private bool IsSolutionInteger(TableClass myTable)
        {
            for (int i = 1; i < myTable.FreeVariablesCount + 1; i++)
            {
                if (Solution["X" + i].Detominator != 1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool AreColumnsPositiveAndRowsNegative(TableClass myTable)
        {
            if (IsSolutionInteger(myTable))
            {
                for (int i = 1; i < myTable.FreeVariablesCount + 1; i++)
                {
                    if (myTable.TableBeforeCalculations[0][i].IsNegative)
                    {
                        return false;
                    }
                }
                for (int i = 1; i < myTable.BasicVariablesCount + 1; i++)
                {
                    if (myTable.TableBeforeCalculations[i][0].IsNegative)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private int ColumnSelection(TableClass myTable)
        {
            if (SelectedColumn == -1)
            {
                SelectedColumn = 1;
            }
            for (; SelectedColumn < myTable.FreeVariablesCount + 1; SelectedColumn++)
            {
                if (myTable.TableBeforeCalculations[0][SelectedColumn].IsNegative)
                {
                    
                    return SelectedColumn;
                }
            }
            return -1;
        }

        private void RowSelection(TableClass myTable)
        {
            int minIndexOfBasicVariable = myTable.BasicVariablesCount + myTable.FreeVariablesCount + 1;
            bool hasNegativeNumbers = false;

            for (int i = 1; i < myTable.BasicVariablesCount + 1; i++)
            {
                if (myTable.TableBeforeCalculations[i][0].IsNegative)
                {                   
                    bool isRepeated = false;
                    hasNegativeNumbers = true;
                    if (Iterations.ContainsKey(SelectedColumn))
                    {
                        if (Iterations[SelectedColumn].Contains(i))
                        {
                            isRepeated = true;                            
                        }
                    }

                    if (!isRepeated && (Convert.ToInt32(myTable.BasicVariables[i][1].ToString()) < minIndexOfBasicVariable))
                    {
                        SelectedRow = i;
                        minIndexOfBasicVariable = Convert.ToInt32(myTable.BasicVariables[i][1].ToString());
                    }                  
                }
            }
            if (!hasNegativeNumbers)
            {
                SelectedRow = -1;
            }
            else if (minIndexOfBasicVariable == myTable.BasicVariablesCount + myTable.FreeVariablesCount + 1)
            {
                SelectedColumn++;
                ColumnAndRowSelection(myTable);
            }
        }

        private int RowSelectionIfAcceptableSolution(TableClass myTable)
        {           
            int indexOfMaxDifference = -1;
            NumberClass maxDifference = Zero;
            
            for (int i = 1; i < myTable.BasicVariablesCount + 1; i++)
            {
                if (myTable.TableBeforeCalculations[i][SelectedColumn] / myTable.TableBeforeCalculations[i][0] > maxDifference)
                {
                    bool isRepeated = false;
                    if (Iterations.ContainsKey(SelectedColumn))
                    {
                        if (Iterations[SelectedColumn].Contains(i))
                        {
                            isRepeated = true;
                        }
                    }
                    if (!isRepeated)
                    {
                        maxDifference = myTable.TableBeforeCalculations[i][SelectedColumn] / myTable.TableBeforeCalculations[i][0];
                        indexOfMaxDifference = i;
                    }        
                }
            }           
            return indexOfMaxDifference;
        }

        private void AddIteration()
        {
            if (!Iterations.ContainsKey(SelectedColumn))
            {
                List<int> tempList = new List<int>();
                tempList.Add(SelectedRow);
                Iterations.Add(SelectedColumn, tempList);
            }
            else
            {
                Iterations[SelectedColumn].Add(SelectedRow);
            }
            //foreach (var item in Iterations)
            //{
            //    Console.Write($"{item.Key} = {item.Value}\n");
            //}  
        }

        private NumberClass GeneralCoefficient(TableClass myTable)
        {
            Lambda = myTable.GetTableBeforeCalculationsItem(SelectedRow, SelectedColumn);

            return new NumberClass("1") / Lambda;
        }

        private void SetNewItemsForSelectedRow(TableClass myTable)
        {
            for (int i = 0; i < myTable.FreeVariablesCount + 1; i++)
            {
                if (i != SelectedColumn)
                {
                    myTable.TableAfterCalculations[SelectedRow][i] = myTable.TableBeforeCalculations[SelectedRow][i] / Lambda;
                }
            }
        }

        private void SetNewItemsForSelectedColumn(TableClass myTable)
        {
            for (int i = 0; i < myTable.BasicVariablesCount + 1; i++)
            {
                if (i != SelectedRow)
                {
                    myTable.TableAfterCalculations[i][SelectedColumn] = myTable.TableBeforeCalculations[i][SelectedColumn] / Lambda ;
                    myTable.TableAfterCalculations[i][SelectedColumn] *= new NumberClass("-1");
                }
            }
        }

        private void SetOtherNewItems(TableClass myTable)
        {
            for (int i = 0; i < myTable.BasicVariablesCount + 1; i++)
            {
                for (int j = 0; j < myTable.FreeVariablesCount + 1; j++)
                {
                    if (i != SelectedRow && j != SelectedColumn)
                    {
                        myTable.TableAfterCalculations[i][j] = myTable.TableBeforeCalculations[_selectedRow][j] * myTable.TableAfterCalculations[i][_selectedColumn];
                    }
                }
            }
        }

        private void UpdateFirstTable(TableClass myTable)
        {
            for (int i = 0; i < myTable.BasicVariablesCount + 1; i++)
            {
                for (int j = 0; j < myTable.FreeVariablesCount + 1; j++)
                {
                    if (i == SelectedRow || j == SelectedColumn)
                    {                       
                        myTable.TableBeforeCalculations[i][j] = myTable.TableAfterCalculations[i][j];
                    }
                    else
                    {
                        myTable.TableBeforeCalculations[i][j] = myTable.TableBeforeCalculations[i][j] + myTable.TableAfterCalculations[i][j];
                    }
                }
            }

            // Обмен данных 
            (myTable.FreeVariables[_selectedColumn], myTable.BasicVariables[_selectedRow]) = (myTable.BasicVariables[_selectedRow], myTable.FreeVariables[_selectedColumn]);
        }

        private void PrintSolution(TableClass myTable)
        {
            myTable.PrintTable();
            foreach (var item in Solution)
            {
                Console.Write($"{item.Key} = {item.Value}, ");
            }
            Console.WriteLine("q = " + myTable.GetTableBeforeCalculationsItem(0, 0).ToString() + ";");
        }
    }
}
