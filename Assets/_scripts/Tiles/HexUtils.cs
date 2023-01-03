using System.Collections.Generic;
using UnityEngine;

namespace vgwb
{
    [System.Serializable]
    public struct HexUtils
    {
        public static float RADIUS = 1f;
        public static Vector2 Q_BASIS = new Vector2(2f, 0);
        public static Vector2 R_BASIS = new Vector2(1f, Mathf.Sqrt(3));
        public static Vector2 Q_INV = new Vector2(1f / 2, -Mathf.Sqrt(3) / 6);
        public static Vector2 R_INV = new Vector2(0, Mathf.Sqrt(3) / 3);

        public static HexUtils FromPlanar(Vector2 planar)
        {
            float q = Vector2.Dot(planar, Q_INV) / RADIUS;
            float r = Vector2.Dot(planar, R_INV) / RADIUS;
            return new HexUtils(q, r);
        }

        public static HexUtils FromWorld(Vector3 world)
        {
            return FromPlanar(new Vector2(world.x, world.z));
        }

        public static HexUtils zero = new HexUtils(0, 0);

        public static HexUtils operator +(HexUtils a, HexUtils b)
        {
            return new HexUtils(a.q + b.q, a.r + b.r);
        }

        public static HexUtils operator -(HexUtils a, HexUtils b)
        {
            return new HexUtils(a.q - b.q, a.r - b.r);
        }

        public static HexUtils[] AXIAL_DIRECTIONS = new HexUtils[] {
        new HexUtils(1, 0),
        new HexUtils(0, 1),
        new HexUtils(-1, 1),
        new HexUtils(-1, 0),
        new HexUtils(0, -1),
        new HexUtils(1, -1),
    };

        public static IEnumerable<HexUtils> Ring(HexUtils center, int radius)
        {
            HexUtils current = center + new HexUtils(0, -radius);
            foreach (HexUtils dir in AXIAL_DIRECTIONS) {
                for (int i = 0; i < radius; i++) {
                    yield return current;
                    current = current + dir;
                }
            }
        }

        public static IEnumerable<HexUtils> Spiral(HexUtils center, int minRadius, int maxRadius)
        {
            if (minRadius == 0) {
                yield return center;
                minRadius += 1;
            }
            for (int r = minRadius; r <= maxRadius; r++) {
                var ring = Ring(center, r);
                foreach (HexUtils hex in ring) {
                    yield return hex;
                }
            }
        }

        public int q;
        public int r;

        public HexUtils(float q, float r) :
            this(Mathf.RoundToInt(q), Mathf.RoundToInt(r))
        { }

        public HexUtils(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        public Vector2 ToPlanar()
        {
            return (Q_BASIS * q + R_BASIS * r) * RADIUS;
        }

        public Vector3 ToWorld(float y = 0f)
        {
            Vector2 planar = ToPlanar();
            return new Vector3(planar.x, y, planar.y);
        }

        public IEnumerable<HexUtils> Neighbours()
        {
            foreach (HexUtils dir in AXIAL_DIRECTIONS) {
                yield return this + dir;
            }
        }

        public HexUtils GetNeighbour(int dir)
        {
            HexUtils incr = AXIAL_DIRECTIONS[dir % AXIAL_DIRECTIONS.Length];
            return this + incr;
        }

        public override bool Equals(System.Object obj)
        {
            HexUtils hex = (HexUtils)obj;
            return (q == hex.q) && (r == hex.r);
        }

        public override int GetHashCode()
        {
            return q * 37 + r * 31;
        }

        public override string ToString()
        {
            return "(" + q + ";" + r + ")";
        }

    }
}
