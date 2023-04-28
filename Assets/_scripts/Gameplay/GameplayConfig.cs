using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;
using QuickOutline;
using DG.Tweening;

namespace vgwb.lanoria
{
    [CreateAssetMenu(menuName = "VGWB/Gameplay Config")]
    public class GameplayConfig : SingletonScriptableObject<GameplayConfig>
    {
        [Header("Rotation Settings")]
        public float RotationStep = 60.0f;
        [Header("Tiles")]
        public Color MovingColor;
        public Color OverlapColor;
        public Outline.Mode PlaceableOutlineMode = Outline.Mode.OutlineVisible;
        public float OutlineWidth = 7.0f;
        public float DragYOffset = 0.2f;
        public float OnConfirmYOffset = 0.1f;

        [Header("Walls")]
        public Material BaseMat;
        public Material HighlightMat;

        [Header("Projects")]
        public Color BlankColor = Color.white;

        [Header("Deck Rules")]
        [Range(1, 4)]
        public int HandSize = 4;

        [Header("Areas")]
        public Vector3 LabelDebugOffset;
        public int LabelDebugFontSize = 15;
        public float CellEfxHeight = 0.4f;
        public float CellEfxAlpha = 0.5f;

        [Header("Bridges")]
        public float BridgeEfxHeight = 0.8f;

        [Header("Score")]
        public int AreaBonus = 20;
        public int AdjacencyBonus = 1;
        public int Place2Bonus = 4;
        public int Place3Bonus = 8;
        public int Place4Bonus = 12;

        [Header("Score Efx")]
        public float FadeInTimeScore = 0.25f;
        public float MovementYOffset = 100.0f;
        public float MovementTime = 2.0f;

        [Header("UI")]
        public float SlideProjectPanelTime = 0.8f;
        public float SlideProjectPanelPixelIn = 50.0f;
        public Vector3 UIProjectRotation = new Vector3(0.0f, -90.0f, 60.0f);

        [Header("UI transitions")]
        public float ResetCameraRotYOnPlay = 1.0f;
        public float FadeInGameCanvas = 1.0f;
        public float CardAppearsTime = 1.0f;

        [Header("Tutorial")]
        public float CardsEnterTime = 1.0f;
        public Ease CardsEnterCurve = Ease.Linear;

        [Header("Tutorial highlight")]
        [Range(0.1f, 1.0f)]
        public float BounceMinDim = 0.6f;
        public float BounceDuration = 0.6f;

        [Header("Camera In Home")]
        public float CameraHomeSpeed = 1.0f;
        public float CameraHomeWait = 3.0f;
        public Ease CameraHomeCurve = Ease.Linear;
    }
}
