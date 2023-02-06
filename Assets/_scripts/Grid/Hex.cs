using System.Collections.Generic;
using UnityEngine;

// using Axial coordinates
// see https://www.redblobgames.com/grids/hexagons/
// r = row
// q = col

namespace vgwb.lanoria
{
    [System.Serializable]
    public struct Hex
    {
        public int q;
        public int r;

        public static float RADIUS = 1f;
        public static Vector2 Q_BASIS = new Vector2(2f, 0);
        public static Vector2 R_BASIS = new Vector2(1f, Mathf.Sqrt(3));
        public static Vector2 Q_INV = new Vector2(1f / 2, -Mathf.Sqrt(3) / 6);
        public static Vector2 R_INV = new Vector2(0, Mathf.Sqrt(3) / 3);

        public Hex(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        public Hex(float q, float r) :
            this(Mathf.RoundToInt(q), Mathf.RoundToInt(r))
        {
        }

        public static Hex zero = new Hex(0, 0);

        public static Hex FromPlanar(Vector2 planar)
        {
            float q = Vector2.Dot(planar, Q_INV) / RADIUS;
            float r = Vector2.Dot(planar, R_INV) / RADIUS;
            return new Hex(q, r);
        }

        public static Hex FromWorld(Vector3 world)
        {
            return FromPlanar(new Vector2(world.x, world.z));
        }

        public static Hex operator +(Hex a, Hex b)
        {
            return new Hex(a.q + b.q, a.r + b.r);
        }

        public static Hex operator -(Hex a, Hex b)
        {
            return new Hex(a.q - b.q, a.r - b.r);
        }

        public static Hex[] AXIAL_DIRECTIONS = new Hex[] {
            new Hex(1, 0),
            new Hex(1, -1),
            new Hex(0, -1),
            new Hex(-1, 0),
            new Hex(-1, 1),
            new Hex(0, 1),
        };

        public static IEnumerable<Hex> Ring(Hex center, int radius)
        {
            Hex current = center + new Hex(0, -radius);
            foreach (Hex dir in AXIAL_DIRECTIONS) {
                for (int i = 0; i < radius; i++) {
                    yield return current;
                    current = current + dir;
                }
            }
        }

        public static IEnumerable<Hex> Spiral(Hex center, int minRadius, int maxRadius)
        {
            if (minRadius == 0) {
                yield return center;
                minRadius += 1;
            }
            for (int r = minRadius; r <= maxRadius; r++) {
                var ring = Ring(center, r);
                foreach (Hex hex in ring) {
                    yield return hex;
                }
            }
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

        public IEnumerable<Hex> Neighbours()
        {
            foreach (Hex dir in AXIAL_DIRECTIONS) {
                yield return this + dir;
            }
        }

        public Hex GetNeighbour(HexDirection dir)
        {
            Hex incr = AXIAL_DIRECTIONS[(int)dir];
            return this + incr;
        }

        public Hex GetNeighbour(int dir)
        {
            Hex incr = AXIAL_DIRECTIONS[dir % AXIAL_DIRECTIONS.Length];
            return this + incr;
        }

        public override bool Equals(System.Object obj)
        {
            Hex hex = (Hex)obj;
            return (q == hex.q) && (r == hex.r);
        }

        public override int GetHashCode()
        {
            return q * 37 + r * 31;
        }

        public override string ToString()
        {
            return q + "," + r;
        }

    }
}
