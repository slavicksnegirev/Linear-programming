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
            Length = GetLenght();
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

        public static NumberClass operator +(NumberClass number1, NumberClass number2)
        {           
            NumberClass result = new NumberClass("0");

            if (number1.Detominator != number2.Detominator)
            {
                result.Detominator = number1.Detominator * number2.Detominator;
                result.Numerator = (number1.Numerator * number2.Detominator) + (number2.Numerator * number1.Detominator);
            }
            else
            {
                result.Detominator = number1.Detominator;
                result.Numerator = number1.Numerator + number2.Numerator;
            }

            if (result.Numerator < 0)
            {
                result.IsNegative = true;
            }
            else if (result.Numerator == 0)
            {
                result.Detominator = 1;
            }

            result.Length = result.GetLenght();

            return result;
        }

        public static NumberClass operator -(NumberClass number1, NumberClass number2)
        {
            NumberClass result = new NumberClass("0");

            if (number1.Detominator != number2.Detominator)
            {
                result.Detominator = number1.Detominator * number2.Detominator;
                result.Numerator = (number1.Numerator * number2.Detominator) - (number2.Numerator * number1.Detominator);
            }
            else
            {
                result.Detominator = number1.Detominator;
                result.Numerator = number1.Numerator - number2.Numerator;
            }

            if (result.Numerator < 0)
            {
                result.IsNegative = true;
            }
            else if (result.Numerator == 0)
            {
                result.Detominator = 1;
            }

            result.Length = result.GetLenght();

            return result;
        }

        public static NumberClass operator *(NumberClass number1, NumberClass number2)
        {
            NumberClass result = new NumberClass("0");

            result.Detominator = number1.Detominator * number2.Detominator;
            result.Numerator = number1.Numerator * number2.Numerator;

            if (result.Numerator < 0)
            {
                result.IsNegative = true;
            }
            else if (result.Numerator == 0)
            {
                result.Numerator = 0;
                result.Detominator = 1;
            }

            result.Length = result.GetLenght();

            return result;
        }

        public static NumberClass operator /(NumberClass number1, NumberClass number2)
        {
            NumberClass result = new NumberClass("0");

            if (number1.Numerator == 0 || number2.Numerator == 0)
            {
                result.Numerator = 0;
                result.Detominator = 1;
            }
            else
            {
                if (number2.isNegative)
                {
                    result.Detominator = number1.Detominator * (-1) * number2.Numerator;
                    result.Numerator = (-1) * number1.Numerator * number2.Detominator;

                }
                else
                {
                    result.Detominator = number1.Detominator * number2.Numerator;
                    result.Numerator = number1.Numerator * number2.Detominator;
                }
            }
            if (result.Numerator < 0)
            {
                result.IsNegative = true;
            }
            else if (result.Numerator == 0)
            {
                result.Detominator = 1;
            }

            result.Length = result.GetLenght();

            return result;
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
    }
}
