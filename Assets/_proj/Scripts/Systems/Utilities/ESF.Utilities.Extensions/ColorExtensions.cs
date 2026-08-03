using UnityEngine;

namespace ESF.Utilities.Extensions
{
    public static class ColorExtensions 
    {
        public static Color WithAlpha(this Color color, float value)
        {
            color.a = value;
            return color;
        }
        public static Color WithRed(this Color color, float value)
        {
            color.r = value;
            return color;
        }
        public static Color WithGreen(this Color color, float value)
        {
            color.g = value;
            return color;
        }
        public static Color WithBlue(this Color color, float value)
        {
            color.b = value;
            return color;
        }
        
    }
}