using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class Wall : MonoBehaviour
    {
        public MeshRenderer WallMesh;
        public GameObjectEfx MyEfx;
        public List<AreaId> Areas;

        public bool IsMyArea(AreaId area)
        {
            return Areas.Contains(area);
        }

        public void GetWallMesh()
        {
            WallMesh = GetComponentInChildren<MeshRenderer>();
        }

        public Vector3 GetWallForward()
        {
            return WallMesh.transform.forward;
        }

        public Vector3 GetWallPosition()
        {
            return WallMesh.transform.position;
        }

        public void AddArea(AreaId area)
        {
            Areas.Add(area);
        }

        public void CleanAreas()
        {
            Areas.Clear();
        }

        public void ChangeMaterial(Material mat)
        {
            if (WallMesh != null) {
                WallMesh.material = mat;
            }
        }

        public void PlayEfx()
        {
            if (MyEfx != null) {
                MyEfx.Play();
            }
        }

        public void StopEfx()
        {
            if (MyEfx != null) {
                MyEfx.Stop();
            }
        }
    } 
}
