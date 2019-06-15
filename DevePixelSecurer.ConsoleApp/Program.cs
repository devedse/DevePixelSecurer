using System;
using System.Drawing;

namespace DevePixelSecurer.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var pixelSecurer = new PixelSecurer();

            int brokenModifier = 7;

            if (brokenModifier < -(pixelSecurer.ParityFactor / 2) || brokenModifier >= (pixelSecurer.ParityFactor / 2))
            {
                throw new Exception("This is not supported by this algorithm");
            }

            for (int i = 0; i < 256; i++)
            {
                var inputColor = Color.FromArgb(i, i, i);
                var withIdentifier = pixelSecurer.ToSecuredPixel(inputColor);

                withIdentifier.ValuePixel = Color.FromArgb(withIdentifier.ValuePixel.R + brokenModifier, withIdentifier.ValuePixel.G + brokenModifier, withIdentifier.ValuePixel.B + brokenModifier);
                withIdentifier.IdentifierPixel = Color.FromArgb(withIdentifier.IdentifierPixel.R + brokenModifier, withIdentifier.IdentifierPixel.G + brokenModifier, withIdentifier.IdentifierPixel.B + brokenModifier);

                var outputColor = pixelSecurer.FromSecuredPixel(withIdentifier);

                var theSame = inputColor == outputColor;

                Console.WriteLine($"{ToColorString(inputColor)} - {ToColorString(outputColor)}      {ToColorString(withIdentifier.IdentifierPixel)} {ToColorString(withIdentifier.ValuePixel)} {(theSame ? "" : "  << WRONG")}");
            }
        }

        public static string ToColorString(Color c)
        {
            return $"({c.R.ToString().PadLeft(3, '0')},{c.G.ToString().PadLeft(3, '0')},{c.B.ToString().PadLeft(3, '0')})";
        }
    }
}
