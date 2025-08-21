using Xunit;
using DiscountCodeSystem.Services;

namespace DiscountCodeSystem.Tests
{
    public class DiscountCodeTests
    {
        [Fact]
        public void GeneratesCorrectCount()
        {
            var generator = new DiscountCodeGenerator();
            var codes = generator.GenerateUniqueCodes(100, 7);
            Xunit.Assert.Equal(100, codes.Count);
        }

        [Fact]
        public void CodesAreUnique()
        {
            var generator = new DiscountCodeGenerator();
            var codes = generator.GenerateUniqueCodes(2000, 7);
            Xunit.Assert.Equal(2000, codes.Distinct().Count());
        }
    }
}