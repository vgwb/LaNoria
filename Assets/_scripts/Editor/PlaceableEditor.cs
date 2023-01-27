using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using vgwb;

namespace vgwb.lanoria
{
    [CustomEditor(typeof(Tile))]
    public class ProjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var myScript = (Tile)target;

            if (GUILayout.Button("Init placeable")) {
                myScript.InitPlaceable();
            }
        }
    }
}
