using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Bom.Utils.Math
{
    public static class NumericalSystemHelper
    {

        // from (supposedly pretty fast)
        // https://stackoverflow.com/questions/923771/quickest-way-to-convert-a-base-10-number-to-any-base-in-net
        // https://www.pvladov.com/2012/07/arbitrary-to-decimal-numeral-system.html


        /// <summary>
        /// Digits in the correct order
        /// </summary>
        /// <remarks>Some digits are consciously left out: 
        /// - characters that have a meaning in sql such as: " ' %
        /// - low letter because most database collations do not distinguish between lower or uppercase letters
        /// </remarks> 
        public const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_ÜÖÄÉÈ=#@(){}&$+*°¬|¢[]";

        public static readonly int Radix = Digits.Length;

        /// <summary>
        /// Converts the given decimal number to the numeral system with a higher Radix (see Digits)
        /// </summary>
        /// <param name="decimalNumber">The number to convert.</param>
        public static string DecimalToArbitrarySystem(long decimalNumber)
        {
            const int BitsInLong = 64;


            //if (radix < 2 || radix > Digits.Length)
            //{
            //    throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());
            //}

            if (decimalNumber == 0)
            {
                return "0";
            }

            int index = BitsInLong - 1;
            long currentNumber = System.Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % Radix);
                charArray[index--] = Digits[remainder];
                currentNumber = currentNumber / Radix;
            }

            string result = new String(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }

        /// <summary>
        /// Converts the given number from the numeral system with a higher Radix (see Digits)
        /// </summary>
        /// <param name="number">The arbitrary numeral system number to convert.</param>
        public static long ArbitraryToDecimalSystem(string number)
        {
            //if (radix < 2 || radix > Digits.Length)
            //{
            //    throw new ArgumentException("The radix must be >= 2 and <= " +
            //        Digits.Length.ToString());
            //}

            if (String.IsNullOrEmpty(number))
            {
                return 0;
            }

            // Make sure the arbitrary numeral system number is in upper case
            //number = number.ToUpperInvariant(); -> we just assume this is the case

            long result = 0;
            long multiplier = 1;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                char c = number[i];
                if (i == 0 && c == '-')
                {
                    // This is the negative sign symbol
                    result = -result;
                    break;
                }

                int digit = Digits.IndexOf(c);
                if (digit == -1)
                {
                    throw new ArgumentException(
                        "Invalid character in the arbitrary numeral system number",
                        "number");
                }
                result += digit * multiplier;
                multiplier *= Radix;
            }

            return result;
        }

    }
}
