namespace Paynet.Challenge.Core.Utils
{
    public class RandomCodeGenerator
    {
        private static readonly char[] Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly Random Random = new Random();

        public static string GenerateRandomCode(int length = 6)
        {
            var code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = Characters[Random.Next(Characters.Length)];
            }

            return new string(code);
        }

    }
}