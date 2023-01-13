using UnityEngine;

namespace vgwb.framework
{
    public static class ColorUtilities
    {
        public static Color SetValue(this Color c, float f)
        {
            Color.RGBToHSV(c, out var h, out var s, out var v);
            v = f;
            return Color.HSVToRGB(h, s, v);
        }

        public static Color SetSaturation(this Color c, float f)
        {
            Color.RGBToHSV(c, out var h, out var s, out var v);
            s = f;
            return Color.HSVToRGB(h, s, v);
        }
    }
}
