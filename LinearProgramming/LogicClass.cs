using System;
using System.Collections.Generic;

namespace LinearProgramming
{
    public class LogicClass
    {
        int _selectedRow = -1;
        int _selectedColumn = -1;
        bool _isDone = false;

        NumberClass _zero = new NumberClass("0");
        NumberClass _lambda = new NumberClass("1");       
        List<List<int>> _iterations = new List<List<int>>();
        Dictionary<string, NumberClass> _solution = new Dictionary<string, NumberClass>();

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

        public List<List<int>> Iterations
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
            ColumnAndRowSelection(myTable);
            AddIteration();
            myTable.TableAfterCalculations[_selectedRow][_selectedColumn] = GeneralCoefficient(myTable);
            SetNewItemsForSelectedRow(myTable);
            SetNewItemsForSelectedColumn(myTable);
            SetOtherNewItems(myTable);
            myTable.PrintTable(SelectedColumn, SelectedRow);
            UpdateFirstTable(myTable);
            IsDone = CheckTheEndOfTheSolution(myTable);
        }

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
                if (i != myTable.FreeVariablesCount + 1 && myTable.TableBeforeCalculations[0][i].IsNegative)
                {
                    return false;
                }
                // Если в условии свободные переменные (Х1, Х2 и т.д.) строго положительные ->
                // -> добавить проверку на ноль
                if (Solution["X" + i].IsNegative /*|| Solution["X" + i] == Zero*/)
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

                for (int j = 0; j < myTable.FreeVariablesCount + 1; j++)
                {
                    result += myTable.TableBeforeCalculations[i][j];
                }
                if (result.IsNegative)
                {
                    return false;
                }
            }
            // Доп. условие
            if (Convert.ToDouble(Solution["X" + 1].Numerator / Solution["X" + 1].Detominator) * Math.Sqrt(Convert.ToDouble(Solution["X" + 2].Numerator / Solution["X" + 2].Detominator)) - 8 >= 0)
            {
                return false;
            }
            return true;
        }

        private void ColumnAndRowSelection(TableClass myTable)
        {
            int index = 1;
            
            do
            {
                SelectedColumn = ColumnSelection(myTable, index);
                if (SelectedColumn != -1)
                {
                    SelectedRow = RowSelection(myTable, index);
                    if (SelectedRow == -1)
                    {
                        SelectedRow = RowSelectionIfAcceptableSolution(myTable);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    bool isSolutionInteger = true;

                    for (int i = 1; i < Solution.Count + 1; i++)
                    {
                        if (Solution["X" + i].Detominator != 1)
                        {
                            isSolutionInteger = false;
                        }            
                    }
                    if (isSolutionInteger)
                    {
                        List<NumberClass> line1 = new List<NumberClass>();
                        List<NumberClass> line2 = new List<NumberClass>();
                        for (int j = 0; j < myTable.FreeVariablesCount + 1; j++)
                        {
                            line1.Add(new NumberClass("-1"));
                            line2.Add(new NumberClass("-1"));
                        }

                        SelectedColumn = 1;
                        SelectedRow = myTable.BasicVariablesCount + myTable.FreeVariablesCount - 1;

                        myTable.TableBeforeCalculations.Add(line1);
                        myTable.TableAfterCalculations.Add(line2);
                        myTable.BasicVariables.Add("X" + (myTable.FreeVariablesCount + ++myTable.BasicVariablesCount));
                        
                    }
                    break;
                }
            } while (SelectedColumn < myTable.FreeVariablesCount + 1);
        }

        private int ColumnSelection(TableClass myTable, int index)
        {
            for (; index < myTable.FreeVariablesCount + 1; index++)
            {
                if (myTable.GetTableBeforeCalculationsItem(0, index).IsNegative)
                {
                    return index;
                }
            }
            return -1;
        }

        private int RowSelection(TableClass myTable, int index)
        { 
            bool hasNegativeNumbers = false;

            for (int i = 1; i < myTable.BasicVariablesCount + 1; i++)
            {
                if (myTable.GetTableBeforeCalculationsItem(i, 0).IsNegative)
                {
                    bool isRepeated = false;
                    hasNegativeNumbers = true;
                    for (int k = 0; k < Iterations.Count; k++)
                    {                
                        if (Iterations[k][0] == index && Iterations[k][1] == i)
                        {
                            isRepeated = true;  
                        }
                    }
                    if (!isRepeated)
                    {
                        return i;
                    }                  
                }
            }
            if (hasNegativeNumbers)
            {
                return index;
            }
            else
            {
                return -1;
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
                    for (int k = 0; k < Iterations.Count; k++)
                    {
                        if (Iterations[k][0] == SelectedColumn && Iterations[k][1] == i)
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
            List<int> tempList = new List<int>();
            tempList.Add(SelectedColumn);
            tempList.Add(SelectedRow);
            Iterations.Add(tempList);

            //for (int i = 0; i < Iterations.Count; i++)
            //{
            //    Console.WriteLine(Iterations[i][0] + " " + Iterations[i][1]);
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
