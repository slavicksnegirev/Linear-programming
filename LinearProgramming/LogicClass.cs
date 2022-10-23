using System;
using System.Collections.Generic;

namespace LinearProgramming
{
    public class LogicClass
    {
        int _selectedRow = -1;
        int _selectedColumn = -1;
        bool _isDone = false;

        NumberClass _lambda = new NumberClass("1");
        List<List<int>> _iterations = new List<List<int>>();
        Dictionary<string, NumberClass> _solution = new Dictionary<string, NumberClass>();

        public bool IsDone
        {
            get => _isDone;
            set => _isDone = value;
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

            myTable.PrintTable();
            foreach (var item in Solution)
            {
                Console.Write($"{item.Key} = {item.Value}, ");
            }
            Console.WriteLine("q = " + myTable.GetTableBeforeCalculationsItem(0,0).ToString() + ";");
        }

        private void MainLogigMethod(TableClass myTable)
        {
            ColumnAndRowSelection(myTable);
            AddIteration(myTable);
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
                if (Solution["X" + i].IsNegative || Solution["X" + i] == new NumberClass("0"))
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
            return true;
        }

        private void ColumnAndRowSelection(TableClass myTable)
        {
            SelectedColumn = ColumnSelection(myTable);
            if (SelectedColumn != -1)
            {
                SelectedRow = RowSelection(myTable);
                if (SelectedRow == -1)
                {
                    SelectedRow = RowSelectionIfAcceptableSolution(myTable);
                }
            }
                

            //if (SelectedColumn != -1 && SelectedRow != -1)
            //{
            //    for (int i = 0; i < Iterations.Count; i++)
            //    {
            //        if (Iterations[i][0] == SelectedRow && Iterations[i][1] == SelectedColumn)
            //        {


            //        }
            //    }
            //}
            
        } //todo убрать зацикливание и добавление новой строки

        private int ColumnSelection(TableClass myTable)
        {
            for (int j = 1; j < myTable.FreeVariablesCount + 1; j++)
            {
                if (myTable.GetTableBeforeCalculationsItem(0, j).IsNegative)
                {
                    return j;
                }
            }
            return -1;
        }

        private int RowSelection(TableClass myTable)
        {
            for (int i = 1; i < myTable.BasicVariablesCount + 1; i++)
            {
                if (myTable.GetTableBeforeCalculationsItem(i, 0).IsNegative)
                {
                    return i;
                }
            }
            return -1;
        }

        private int RowSelectionIfAcceptableSolution(TableClass myTable)
        {
            int indexOfMaxDifference = -1;
            NumberClass maxDifference = new NumberClass("0");
            
            for (int i = 1; i < myTable.BasicVariablesCount + 1; i++)
            {
                if (myTable.GetTableBeforeCalculationsItem(i, _selectedColumn) / myTable.GetTableBeforeCalculationsItem(i, 0) > maxDifference)
                {
                    maxDifference = myTable.GetTableBeforeCalculationsItem(i, _selectedColumn) / myTable.GetTableBeforeCalculationsItem(i, 0);
                    indexOfMaxDifference = i;
                }
            }
            return indexOfMaxDifference;
        }

        private void AddIteration(TableClass myTable) // todo сделать нормально для учета циклов
        {
            List<int> tempList = new List<int>();
            tempList.Add(SelectedRow);
            tempList.Add(SelectedColumn);
            Iterations.Add(tempList);

            //for (int i = 0; i < Iterations.Count; i++)
            //{
            //    for (int j = 0; j < 2; j++)
            //    {
            //        Console.Write(Iterations[i][j] + " ");
            //    }
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
    }
}
