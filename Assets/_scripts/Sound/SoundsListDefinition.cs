using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{

    public enum SfxEnum
    {
        click = 1,
        win = 2,
        lose = 3,
        open = 4,
        close = 5,
        trash = 6,
        score = 7,
        shuffle = 8,
        card = 9
    }

    [CreateAssetMenu(menuName = "VGWB/Sounds List")]
    public class SoundsListDefinition : ScriptableObject
    {
        public List<SoundItem> Sounds;
    }

    [Serializable]
    public class SoundItem
    {
        public SfxEnum id;
        public AudioClip audioClip;

        [Range(0.0f, 1.0f)]
        public float Volume = 1.0f;
    }
}
