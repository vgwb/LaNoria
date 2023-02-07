using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class BoardManager : SingletonMonoBehaviour<BoardManager>
    {
        public GridManager GridManager;
        public GameObject ProjectsContainer;
        public GameObject MapContainer;
        public GameObject MapOutlandContainer;
        public GameObject WallsContainer;

        void Start()
        {

        }

        public void ShowMap(bool status)
        {
            MapContainer.SetActive(status);
        }

        public void ShowOutland(bool status)
        {
            MapOutlandContainer.SetActive(status);
        }

        public void EmptyProjectsContainer()
        {
            foreach (Transform t in ProjectsContainer.transform) {
                Destroy(t.gameObject);
            }
        }

        public float GetProjectsHeight()
        {
            float height = 0.0f;
            if (ProjectsContainer != null) {
                height = ProjectsContainer.transform.position.y;
            }

            return height;
        }

        public void TileCollidesWithOutland(bool status)
        {

        }

    }
}
