using System;
using System.Collections.Generic;

namespace LinearProgramming
{
    public class LogicClass
    {
        int selectedRow;
        int selectedColumn;       

        NumberClass lambda = new NumberClass("1");
        //Dictionary<string, string> iterations = new Dictionary<string, string>();
        List<List<string>> iterations = new List<List<string>>();


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

        public List<List<string>> Iterations
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

            //NumberClass number1 = new NumberClass("1/3");
            //NumberClass number2 = new NumberClass("2/3");

            //Console.WriteLine(number1 + number2);
            //Console.WriteLine(number1 - number2);
            //Console.WriteLine(number1 * number2);
            //Console.WriteLine(number1 / number2);

        }

        private int ColumnSelection(TableClass myTable)
        {
            for (int j = 1; j < myTable.BasicVariablesCount + 1; j++)
            {
                if (myTable.GetTableBeforeCalculationsItem(0,j).IsNegative)
                {
                    return j;
                }               
            }
            return -1;
        }

        private int RowSelection(TableClass myTable)
        {
            for (int i = 1; i < myTable.FreeVariablesCount; i++)
            {
                if (myTable.GetTableBeforeCalculationsItem(i, 0).IsNegative)
                {
                    return i;
                }
            }
            return -1;
        }

        private void AddIteration(TableClass myTable)
        {
            List<string> tempList = new List<string>() { myTable.FreeVariables[SelectedRow], myTable.BasicVariables[SelectedColumn] };
            Iterations.AddRange((IEnumerable<List<string>>)tempList);

            foreach (var item in Iterations)
            {
                Console.WriteLine(item + " ");
            }
        }

        private NumberClass GeneralCoefficient(TableClass myTable)
        {
            Lambda = myTable.GetTableBeforeCalculationsItem(SelectedRow, SelectedColumn);

            return new NumberClass("1") / Lambda;
        }

        private void SetNewItemsForSelectedRow(TableClass myTable)
        {
            for (int i = 0; i < myTable.BasicVariablesCount + 1; i++)
            {
                if (i != SelectedColumn)
                {
                    myTable.TableAfterCalculations[SelectedRow][i] = myTable.TableBeforeCalculations[SelectedRow][i] / Lambda;
                }
            }
        }

        private void SetNewItemsForSelectedColumn(TableClass myTable)
        {
            for (int i = 0; i < myTable.FreeVariablesCount; i++)
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
            for (int i = 0; i < myTable.FreeVariablesCount; i++)
            {
                for (int j = 0; j < myTable.BasicVariablesCount + 1; j++)
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
            for (int i = 0; i < myTable.FreeVariablesCount; i++)
            {
                for (int j = 0; j < myTable.BasicVariablesCount + 1; j++)
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

            string temp = myTable.BasicVariables[selectedColumn];
            myTable.BasicVariables[selectedColumn] = "-" + myTable.FreeVariables[selectedRow];
            myTable.FreeVariables[selectedRow] = temp.Remove(0, 1);
        }
    }
}
