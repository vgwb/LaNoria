using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ieedo
{

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager I;
        public SoundsListDefinition SoundsList;

        public AudioSource audioSource;
        public AudioSource audioSourceWin;

        public void Awake()
        {
            if (I == null)
                I = this;
            // audioSource = gameObject.GetComponent<AudioSource>();
        }

        public void PlaySfx(SfxEnum sfx)
        {
            //Debug.Log("PlaySfx " + sfx);
            // if (Statics.Data.Profile.Settings.SfxDisabled)
            //     return;

            var sound = SoundsList.Sounds.Find(item => item.id == sfx);
            if (sound != null) {
                if (sfx == SfxEnum.win) {
                    audioSourceWin.Play();
                }
                else {
                    audioSource.clip = sound.audioClip;
                    audioSource.volume = sound.Volume;
                    audioSource.Play();
                }
            }
            else {
                Debug.LogError("PlaySfx does not exist sound " + sfx.ToString());
            }
        }
    }
}
