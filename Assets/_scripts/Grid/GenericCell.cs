using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public abstract class GenericCell : MonoBehaviour
    {
        [SerializeField] protected MeshRenderer mesh;

        public Hex hex => Hex.FromWorld(transform.position);
        private Hex localHex => Hex.FromWorld(transform.localPosition);

        public Vector3 HexPosition
        {
            get => hex.ToWorld(0.0f);
            set { }
        }

        protected virtual void Awake()
        {
            BaseSetup();
        }

        public void ApplyMaterial(Material mat)
        {
            if (mesh != null) {
                mesh.material = mat;
            }
        }

        protected virtual void BaseSetup()
        {
            if (mesh == null) {
                mesh = GetComponentInChildren<MeshRenderer>();
            }
        }
    }
}
