using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vgwb.framework;

namespace vgwb.lanoria
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        public SoundsListDefinition SoundsList;

        public AudioSource audioSource1;
        public AudioSource audioSource2;
        public AudioSource audioSource3;
        public AudioSource audioSourceMusic;

        public void PlayMusic(AudioEnum musicId)
        {
            var sound = SoundsList.Sounds.Find(item => item.id == musicId);
            if (sound != null) {
                var audioSource = getSource(sound);

                audioSource.clip = sound.audioClip;
                audioSource.volume = sound.Volume;
                if (!AppManager.I.AppSettings.MusicEnabled) {
                    audioSource.Play();
                }
            }
        }

        public void StopMusic()
        {
            audioSourceMusic.Stop();
        }

        public void ToggleMusic(bool status)
        {
            if (status) {
                audioSourceMusic.Play();
            } else {
                audioSourceMusic.Stop();
            }
        }

        public void PlaySfx(AudioEnum sfx)
        {
            // Debug.Log("PlaySfx " + sfx);
            if (!AppManager.I.AppSettings.SfxEnabled)
                return;

            var sound = SoundsList.Sounds.Find(item => item.id == sfx);
            if (sound != null) {
                var audioSource = getSource(sound);

                audioSource.clip = sound.audioClip;
                audioSource.volume = sound.Volume;
                audioSource.Play();
            } else {
                Debug.LogError("PlaySfx does not exist sound " + sfx.ToString());
            }
        }

        private AudioSource getSource(SoundItem sound)
        {
            switch (sound.Channel) {
                case AudioChannel.Sfx1:
                    return audioSource1;
                case AudioChannel.Sfx2:
                    return audioSource2;
                case AudioChannel.Sfx3:
                    return audioSource3;
                case AudioChannel.Music:
                    return audioSourceMusic;
                default:
                    return null;
            }
        }
    }
}
