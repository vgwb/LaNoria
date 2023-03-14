using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace vgwb.lanoria
{
    public class UI_PanelAbout : MonoBehaviour
    {
        public Button BtnClose;
        public Button BtnWebsite;

        public GameObject ProjectsContainer;
        public GameObject ProjectCardPrefab;

        private GameObject _projectCard;

        void Start()
        {
            BtnClose.onClick.AddListener(() => AppManager.I.OnHome());
            BtnClose.onClick.AddListener(() => SoundManager.I.PlaySfx(AudioEnum.click));
            BtnWebsite.onClick.AddListener(() => OnWebsite());
        }

        public void OpenPanel()
        {
            populateProjects();
            gameObject.SetActive(true);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            // Debug.Log("CIAO CIAO");
        }

        void populateProjects()
        {
            emptyProjectsContainers();

            var projects = GameData.I.Projects.Projects;
            foreach (var project in projects) {
                if (project.Active) {
                    _projectCard = Instantiate(ProjectCardPrefab);
                    _projectCard.transform.SetParent(ProjectsContainer.transform, false);
                    _projectCard.GetComponent<ProjectCard>().Init(project);
                }
            }
        }

        void emptyProjectsContainers()
        {
            foreach (Transform t in ProjectsContainer.transform) {
                Destroy(t.gameObject);
            }
        }

        private void OnWebsite()
        {
            Application.OpenURL("https://www.malaga.es/lanoria/");
        }

    }

}
