using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vgwb.lanoria
{

    public enum AudioEnum
    {
        click = 1,
        win = 2,
        lose = 3,
        open = 4,
        close = 5,
        trash = 6,
        score = 7,
        shuffle = 8,
        card = 9,
        tile_confirmed = 10,
        tile_rotate = 11,
        tile_released = 12,
        score_efx = 13,

        music_1 = 20
    }

    public enum AudioChannel
    {
        Sfx1,
        Sfx2,
        Sfx3,
        Music
    }

    [CreateAssetMenu(menuName = "VGWB/Sounds List")]
    public class SoundsListDefinition : ScriptableObject
    {
        public List<SoundItem> Sounds;
    }

    [Serializable]
    public class SoundItem
    {
        public AudioEnum id;
        public AudioChannel Channel;
        public AudioClip audioClip;

        [Range(0.0f, 1.0f)]
        public float Volume;
    }
}
