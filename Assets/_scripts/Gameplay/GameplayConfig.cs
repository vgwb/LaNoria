using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;
using QuickOutline;

namespace vgwb.lanoria
{
    [CreateAssetMenu(menuName = "VGWB/Gameplay Config")]
    public class GameplayConfig : SingletonScriptableObject<GameplayConfig>
    {
        [Header("Tiles")]
        public Color MovingColor;
        public Color OverlapColor;
        public Outline.Mode PlaceableOutlineMode = Outline.Mode.OutlineVisible;
        public float OutlineWidth = 7.0f;
        public float DragYOffset = 0.2f;
        public float OnConfirmYOffset = 0.1f;

        [Header("Projects")]
        public Color BlankColor = Color.white;

        [Header("Deck Rules")]
        public int HandSize = 4;

        [Header("Areas")]
        public Vector3 LabelDebugOffset;
        public int LabelDebugFontSize = 15;

        [Header("Score")]
        public int TransversalityBonus = 20;
        public int SynergyBonus = 1;
        public int Hex2Points = 4;
        public int Hex3Points = 8;
        public int Hex4Points = 12;

        [Header("UI")]
        public float SlideProjectPanelTime = 0.8f;
        public float SlideProjectPanelPixelIn = 50.0f;
        public Vector3 UIProjectRotation = new Vector3(0.0f, -90.0f, 60.0f);

        [Header("UI transitions")]
        public float ResetCameraRotYOnPlay = 1.0f;
        public float FadeInGameCanvas = 1.0f;
        public float CardAppearsTime = 1.0f;

        [Header("Camera In Game")]
        public float MovementXClamp = 4.0f;
        public float MovementYClamp = 3.0f;
    }
}
