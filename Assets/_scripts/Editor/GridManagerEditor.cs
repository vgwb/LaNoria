using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace vgwb.lanoria
{
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var myScript = (GridManager)target;

            if (GUILayout.Button("Initialize Grid")) {
                myScript.InitCells();
            }
        }
    } 
}
