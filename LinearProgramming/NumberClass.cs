using System;
namespace LinearProgramming
{
    public class NumberClass
    {
        private bool isNegative = false;
        private double numerator;
        private double detominator;

        public NumberClass(string number)
        {
            if (number[0] == '-')
            {
                isNegative = true;
            }

            int indexOfSlash = number.IndexOf('/');

            if (indexOfSlash < 0)
            {
                numerator = Convert.ToDouble(number);
                detominator = 1;
            }
            else
            {
                string[] tempNumbers = number.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                numerator = Convert.ToDouble(tempNumbers[0]);
                detominator = Convert.ToDouble(tempNumbers[1]);
            }
        }

        public override string ToString()
        {
            if (detominator == 1)
            {
                return numerator.ToString();
            }
            else
            {
                return numerator.ToString() + '/' + detominator.ToString();
            }
        }
    }
}
