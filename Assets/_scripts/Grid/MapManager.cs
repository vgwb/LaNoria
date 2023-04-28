using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class MapManager : MonoBehaviour
    {
        #region Var
        public GameObject MapRoot;
        public List<MapCell> RootCells;
        #endregion

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();

            AppManager.I.AppSettings.OnAccesibilityModified += OnAccesibilityModified;
            EnableIcons(AppManager.I.AppSettings.AccessibilityEnabled);
        }

        private void OnDestroy()
        {
            AppManager.I.AppSettings.OnAccesibilityModified -= OnAccesibilityModified;
        }


        /// <summary>
        /// Use only in editor!
        /// </summary>
        [Button]
        public void InitCells()
        {
            RootCells.Clear();
            var mapCells = MapRoot.transform.GetComponentsInChildren<MapCell>();
            foreach (var cell in mapCells) {
                cell.SetIcon();
                RootCells.Add(cell);
            }
        }

        [Button]
        public void HideIcons()
        {
            EnableIcons(false);
        }

        [Button]
        public void ShowIcons()
        {
            EnableIcons(true);
        }

        private void OnAccesibilityModified(bool enable)
        {
            EnableIcons(enable);
        }

        private void EnableIcons(bool enable)
        {
            foreach (var cell in RootCells) {
                cell.EnableIcon(enable);
            }
        }
    } 
}
