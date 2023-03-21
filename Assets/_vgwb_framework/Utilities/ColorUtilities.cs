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

        // Taken from here: http://wiki.unity3d.com/index.php?title=HexConverter
        public static string ColorToHex(Color32 _color, bool _addHashPrefix = false)
        {
            string hex = _color.r.ToString("X2") + _color.g.ToString("X2") + _color.b.ToString("X2");
            return _addHashPrefix ? "#" + hex : hex;
        }
    }
}
