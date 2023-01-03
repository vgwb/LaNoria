using UnityEngine;

namespace vgwb
{
    [ExecuteInEditMode]
    public class HexNeighbours : MonoBehaviour
    {

        public Hex hex
        {
            get {
                return Hex.FromWorld(transform.position);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            foreach (Hex neighbour in hex.Neighbours()) {
                Gizmos.DrawSphere(neighbour.ToWorld(), .25f);
            }
        }

    }
}
