using System;
using System.Linq;
using Xunit;

namespace Utils.Math
{
    public class NumericalSystemHelperTests
    {
        [Fact]
        public void Can_convert_decimal_to_custom_numerical_system_and_back()
        {
            for (int i = 0; i < 234564334; i = i + 99999) {
                var stringRepresentation = NumericalSystemHelper.DecimalToArbitrarySystem(i);
                long backToInt = NumericalSystemHelper.ArbitraryToDecimalSystem(stringRepresentation);
                Assert.Equal(backToInt, i);
                Assert.True(stringRepresentation.Length <= i.ToString().Length);
            }
        }

        [Fact]
        public void Digits_of_converter_system_are_unique()
        {
            var length = NumericalSystemHelper.Digits.Length;
            var distinctLenght = NumericalSystemHelper.Digits.ToCharArray().Distinct().Count();
            Assert.Equal(length, distinctLenght);
        }

        [Fact]
        public void Digits_of_converter_do_not_contain_sql_problematic_characters()
        {
            // characters with meaning in sql server
            Assert.False(NumericalSystemHelper.Digits.Contains('%')); // like queries
            Assert.False(NumericalSystemHelper.Digits.Contains('\'')); // string
            Assert.False(NumericalSystemHelper.Digits.Contains('\"')); // string 

            // no uper lower characters as normally, database collaction does not distinguish this
            var toUpper = NumericalSystemHelper.Digits.ToUpperInvariant();
            Assert.True(toUpper == NumericalSystemHelper.Digits);
        }


        [Fact]
        public void Digits_of_converter_do_not_contain_non_sql_problematic_characters()
        {
            Assert.False(NumericalSystemHelper.Digits.Contains('/')); // Pathseparator

            // leftovers (not sql)
            Assert.False(NumericalSystemHelper.Digits.Contains('-')); // minus values should also be allowed
        }

    }
}
