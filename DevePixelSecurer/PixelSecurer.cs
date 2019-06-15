using System.Drawing;

namespace DevePixelSecurer
{
    public class PixelSecurer
    {
        public int ParityFactor { get; }

        public PixelSecurer(int parityFactor = 16)
        {
            ParityFactor = parityFactor;
        }

        public Color FromSecuredPixel(SecuredPixel securedPixel)
        {
            var red = FromSecuredValue(securedPixel.IdentifierPixel.R, securedPixel.ValuePixel.R);
            var green = FromSecuredValue(securedPixel.IdentifierPixel.G, securedPixel.ValuePixel.G);
            var blue = FromSecuredValue(securedPixel.IdentifierPixel.B, securedPixel.ValuePixel.B);

            return Color.FromArgb(red, green, blue);
        }

        public SecuredPixel ToSecuredPixel(Color inputPixel)
        {
            var red = ToSecuredValue(inputPixel.R);
            var green = ToSecuredValue(inputPixel.G);
            var blue = ToSecuredValue(inputPixel.B);

            var resultColor = Color.FromArgb(red.value, green.value, blue.value);
            var identifierColor = Color.FromArgb(red.identifier, green.identifier, blue.identifier);

            return new SecuredPixel()
            {
                IdentifierPixel = identifierColor,
                ValuePixel = resultColor
            };
        }

        private (int identifier, int value) ToSecuredValue(int input)
        {
            int tot = input * ParityFactor + (ParityFactor / 2);

            int theValue = tot % 256;
            int identifierStart = tot / 256;
            int identifier = identifierStart * (256 / ParityFactor) + ((256 / ParityFactor) / 2);

            return (identifier, theValue);
        }

        private int FromSecuredValue(int inputIdentifier, int input)
        {
            int identifier = inputIdentifier / (256 / ParityFactor);

            int value = input + (identifier * 256);
            int actualValue = value / ParityFactor;

            return actualValue;
        }
    }
}
