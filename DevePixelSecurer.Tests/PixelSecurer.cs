using System;
using System.Drawing;
using Xunit;

namespace DevePixelSecurer.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void SecuresUnmodifiedPixels()
        {
            //Arrange
            var pixelSecurer = new PixelSecurer();

            int brokenModifier = 0;

            //Act / Assert
            for (int i = 0; i < 256; i++)
            {
                var inputColor = Color.FromArgb(i, i, i);
                var withIdentifier = pixelSecurer.ToSecuredPixel(inputColor);

                withIdentifier.ValuePixel = Color.FromArgb(withIdentifier.ValuePixel.R + brokenModifier, withIdentifier.ValuePixel.G + brokenModifier, withIdentifier.ValuePixel.B + brokenModifier);
                withIdentifier.IdentifierPixel = Color.FromArgb(withIdentifier.IdentifierPixel.R + brokenModifier, withIdentifier.IdentifierPixel.G + brokenModifier, withIdentifier.IdentifierPixel.B + brokenModifier);

                var outputColor = pixelSecurer.FromSecuredPixel(withIdentifier);

                var theSame = inputColor == outputColor;

                Assert.True(theSame);
            }
        }

        [Fact]
        public void SecuresTamperedWithPixels()
        {
            //Arrange
            var pixelSecurer = new PixelSecurer();

            //Act / Assert
            for (int brokenModifier = -(pixelSecurer.ParityFactor / 2); brokenModifier < pixelSecurer.ParityFactor / 2; brokenModifier++)
            {
                for (int i = 0; i < 256; i++)
                {
                    var inputColor = Color.FromArgb(i, i, i);
                    var withIdentifier = pixelSecurer.ToSecuredPixel(inputColor);

                    withIdentifier.ValuePixel = Color.FromArgb(withIdentifier.ValuePixel.R + brokenModifier, withIdentifier.ValuePixel.G + brokenModifier, withIdentifier.ValuePixel.B + brokenModifier);
                    withIdentifier.IdentifierPixel = Color.FromArgb(withIdentifier.IdentifierPixel.R + brokenModifier, withIdentifier.IdentifierPixel.G + brokenModifier, withIdentifier.IdentifierPixel.B + brokenModifier);

                    var outputColor = pixelSecurer.FromSecuredPixel(withIdentifier);

                    var theSame = inputColor == outputColor;

                    Assert.True(theSame);
                }
            }
        }
    }
}
