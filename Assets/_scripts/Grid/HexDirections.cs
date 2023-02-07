namespace vgwb.lanoria
{
    public enum HexDirection
    {
        NW = 0, NE = 1, E = 2, SE = 3, SW = 4, W = 5
    }

    public static class HexDirectionExtensions
    {

        public static HexDirection GetByDelta(this HexDirection direction, int delta)
        {
            if (delta == 1) {
                return Next(direction);
            } else if (delta == 2) {
                return Next2(direction);
            } else if (delta == 3) {
                return Opposite(direction);
            } else if (delta == -1) {
                return Previous(direction);
            } else if (delta == -2) {
                return Previous2(direction);
            }
            return direction;
        }

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
