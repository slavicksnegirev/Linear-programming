using System;
using System.Collections.Generic;

namespace LinearProgramming
{
    public class LogicClass
    {
        NumberClass lambda = new NumberClass("1");

        public LogicClass(TableClass tableBeforeCalculations)
        {
            int j = ColumnSelection(tableBeforeCalculations);
            int i = RowSelection(tableBeforeCalculations);



            tableBeforeCalculations.PrintTable(j, i);


        }

        private int ColumnSelection(TableClass tableBeforeCalculations)
        {
            for (int j = 1; j < tableBeforeCalculations.BasicVariablesCount + 1; j++)
            {
                if (tableBeforeCalculations.GetTableItem(0,j).IsNegative)
                {
                    return j;
                }               
            }
            return -1;
        }

        private int RowSelection(TableClass tableBeforeCalculations)
        {
            for (int i = 1; i < tableBeforeCalculations.FreeVariablesCount + 1; i++)
            {
                if (tableBeforeCalculations.GetTableItem(i, 0).IsNegative)
                {
                    return i;
                }
            }
            return -1;
        }

        private NumberClass GeneralCoefficient(TableClass tableBeforeCalculations, int j, int i)
        {
            return (tableBeforeCalculations.GetTableItem(j,i));
        }
    }
}
