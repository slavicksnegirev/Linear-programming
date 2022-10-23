using System;
using System.Collections.Generic;

namespace LinearProgramming
{
    public class LogicClass
    {
        int selectedRow = -1;
        int selectedColumn = -1;
        bool isDone = false;

        NumberClass lambda = new NumberClass("1");
        List<List<int>> iterations = new List<List<int>>();

        public bool IsDone
        {
            get => isDone;
            set => isDone = value;
        }

        public NumberClass Lambda
        {
            get => lambda;
            set => lambda = value;
        }

        public int SelectedRow
        {
            get => selectedRow;
            set => selectedRow = value;
        }

        public int SelectedColumn
        {
            get => selectedColumn;
            set => selectedColumn = value;
        }

        public List<List<int>> Iterations
        {
            get => iterations;
            set => iterations = value;
        }

        public LogicClass(TableClass myTable)
        {
            SelectedColumn = ColumnSelection(myTable);
            SelectedRow = RowSelection(myTable);
            AddIteration(myTable);
            myTable.TableAfterCalculations[selectedRow][selectedColumn] = GeneralCoefficient(myTable);
            SetNewItemsForSelectedRow(myTable);
            SetNewItemsForSelectedColumn(myTable);
            SetOtherNewItems(myTable);
            myTable.PrintTable(SelectedColumn, SelectedRow);

            UpdateFirstTable(myTable);
            myTable.PrintTable(SelectedColumn, SelectedRow);

        }

        private void ColumnAndRowSelection(TableClass myTable)
        {
            do
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
            } while (SelectedColumn < 0 || SelectedRow < 0);
            
        }

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
                if (myTable.GetTableBeforeCalculationsItem(i, 0) / myTable.GetTableBeforeCalculationsItem(i, selectedColumn) > maxDifference)
                {
                    maxDifference = myTable.GetTableBeforeCalculationsItem(i, 0) / myTable.GetTableBeforeCalculationsItem(i, selectedColumn);
                    indexOfMaxDifference = i;
                }
            }
            return indexOfMaxDifference;
        }

        private void AddIteration(TableClass myTable)
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
                    myTable.TableAfterCalculations[i][SelectedColumn] = myTable.TableBeforeCalculations[i][SelectedColumn] / Lambda;
                    myTable.TableAfterCalculations[i][SelectedColumn].Numerator *= -1;
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
                        myTable.TableAfterCalculations[i][j] = myTable.TableBeforeCalculations[selectedRow][j] * myTable.TableAfterCalculations[i][selectedColumn];
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

            //string temp = myTable.FreeVariables[selectedColumn];
            //myTable.FreeVariables[selectedColumn] = myTable.BasicVariables[selectedRow];
            //myTable.BasicVariables[selectedRow] = temp;

            // Обмен данных 
            (myTable.FreeVariables[selectedColumn], myTable.BasicVariables[selectedRow]) = (myTable.BasicVariables[selectedRow], myTable.FreeVariables[selectedColumn]);
        }
    }
}
