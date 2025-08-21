using System.Text;

namespace DiscountCodeSystem.Services
{
    public class DiscountCodeGenerator
    {
        private static readonly char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private readonly Random _random = new();

        public HashSet<string> GenerateUniqueCodes(int count, byte length)
        {
            if (length < 7 || length > 8)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be either 7 or 8 characters.");

            var codes = new HashSet<string>();
            while (codes.Count < count)
            {
                var code = new string(Enumerable.Range(0, length)
                    .Select(_ => chars[_random.Next(chars.Length)]).ToArray());
                codes.Add(code);
            }
            return codes;
        }

    }

}


