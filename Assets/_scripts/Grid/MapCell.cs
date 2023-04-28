using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{
    public class MapCell : MonoBehaviour
    {
        public GameObject Prefab;
        public GameObject Prop;
        public SpriteRenderer Icon;

        public void SetIcon()
        {
            var mesh = GetComponentInChildren<MeshRenderer>();
            if (mesh != null) {
                if (mesh.sharedMaterial != null) {
                    string[] nameSplit = mesh.sharedMaterial.name.Split(" ");
                    string nameMatch = nameSplit[0];
                    var icon = GameData.I.Categories.GetIconByMaterialName(nameMatch);
                    Icon.sprite = icon;
                }
            }
        }

        public void EnableIcon(bool enable)
        {
            if (Icon != null) {
                Icon.gameObject.SetActive(enable);
            }
        }
    }
}
