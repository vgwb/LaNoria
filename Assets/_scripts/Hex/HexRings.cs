using UnityEngine;

namespace vgwb
{
    [ExecuteInEditMode]
    public class HexRings : MonoBehaviour
    {

        [Range(1, 10)]
        public int minRadius;
        [Range(1, 10)]
        public int maxRadius;

        public Hex hex {
            get {
                return Hex.FromWorld(transform.position);
            }
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.cyan;
            foreach (Hex hex in Hex.Spiral(this.hex, minRadius, maxRadius)) {
                Gizmos.DrawSphere(hex.ToWorld(), .25f);
            }
        }

    }
}
