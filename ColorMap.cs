using System;
using System.Collections.Generic;
using Avalonia.Media;

namespace AvaloniaMouseHeatMap
{
    internal static class ColorMap
    {
        static ColorMap()
        {
            //mAllColors = GenerateColorMap(Colors.Cyan, Colors.Magenta, 20);
        }

        public static Color GetColorForValue(double val, double maxVal)
        {
            if (val > maxVal - 1)
                val = maxVal - 1;

            double valPerc = val / maxVal;// value%
            double colorPerc = 1d / (mAllColors.Count - 1);// % of each block of color. the last is the "100% Color"
            double blockOfColor = valPerc / colorPerc;// the integer part repersents how many block to skip
            int blockIdx = (int)Math.Truncate(blockOfColor);// Idx of 
            double valPercResidual = valPerc - (blockIdx * colorPerc);//remove the part represented of block 
            double percOfColor = valPercResidual / colorPerc;// % of color of this block that will be filled

            Color cTarget = mAllColors[blockIdx];
            Color cNext = cNext = mAllColors[blockIdx + 1];

            var deltaR = cNext.R - cTarget.R;
            var deltaG = cNext.G - cTarget.G;
            var deltaB = cNext.B - cTarget.B;

            var R = cTarget.R + (deltaR * percOfColor);
            var G = cTarget.G + (deltaG * percOfColor);
            var B = cTarget.B + (deltaB * percOfColor);

            return Color.FromArgb(150, (byte)R, (byte)G, (byte)B);
        }


        public static List<Color> GenerateColorMap(Color color1, Color color2, int size)
        {
            List<Color> colorMap = new List<Color>();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    colorMap.Add(LerpColor(color1, color2, (float)i / (size - 1)));
                }
            }
            return colorMap;
        }

        private static Color LerpColor(Color color1, Color color2, float t)
        {
            return new Color(
                (byte)(color1.R * (1 - t) + color2.R * t),
                (byte)(color1.G * (1 - t) + color2.G * t),
                (byte)(color1.B * (1 - t) + color2.B * t),
                (byte)(color1.A * (1 - t) + color2.A * t));
        }

        static List<Color> mAllColors = new List<Color>()
        {
                Color.FromRgb(0xFF, 0xFF, 0xFF), // White
                Color.FromRgb(0, 0, 0xFF) ,//Blue
                Color.FromRgb(0, 0xFF, 0xFF) ,//Cyan
                Color.FromRgb(0, 0xFF, 0) ,//Green
                Color.FromRgb(0xFF, 0xFF, 0) ,//Yellow
                Color.FromRgb(0xFF, 0, 0) ,//Red
                Color.FromRgb(0x8B, 0, 0) // Dark Red
        };
    }
}