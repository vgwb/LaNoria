namespace vgwb.lanoria
{
    public enum HexDirection
    {
        NW, NE, E, SE, SW, W
    }

    public static class HexDirectionExtensions
    {

        public static HexDirection Opposite(this HexDirection direction) =>
            (int)direction < 3 ? (direction + 3) : (direction - 3);

        public static HexDirection Previous(this HexDirection direction) =>
            direction == HexDirection.NW ? HexDirection.W : (direction - 1);

        public static HexDirection Next(this HexDirection direction) =>
            direction == HexDirection.W ? HexDirection.NW : (direction + 1);

        public static HexDirection Previous2(this HexDirection direction)
        {
            direction -= 2;
            return direction >= HexDirection.NW ? direction : (direction + 6);
        }

        public static HexDirection Next2(this HexDirection direction)
        {
            direction += 2;
            return direction <= HexDirection.W ? direction : (direction - 6);
        }
    }
}
