using System;
namespace LinearProgramming
{
    public class NumberClass
    {
        private bool isNegative = false;
        private double numerator;
        private double detominator;
        private int length = 0;

        public bool IsNegative
        {
            get => isNegative;
            set => isNegative = value;
        }

        public double Numerator
        {
            get => numerator;
            set => numerator = value;
        }

        public double Detominator
        {
            get => detominator;
            set => detominator = value;
        }

        public int Length
        {
            get => length;
            set => length = value;
        }

        public NumberClass(string number)
        {
            if (number[0] == '-')
            {
                IsNegative = true;
            }

            int indexOfSlash = number.IndexOf('/');

            if (indexOfSlash < 0)
            {
                Numerator = Convert.ToDouble(number);
                Detominator = 1;
            }
            else
            {
                string[] tempNumbers = number.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                Numerator = Convert.ToDouble(tempNumbers[0]);
                Detominator = Convert.ToDouble(tempNumbers[1]);
            }
        }

        public override string ToString()
        {
            if (Detominator == 1)
            {
                return Numerator.ToString();
            }
            else
            {
                return Numerator.ToString() + '/' + Detominator.ToString();
            }
        }

        public int GetLenght()
        {
            if (Detominator == 1)
            {
                Length = Numerator.ToString().Length;
            }
            else
            {
                Length = Numerator.ToString().Length + 1 + Detominator.ToString().Length;
            }

            return Length;
        }
    }
}
