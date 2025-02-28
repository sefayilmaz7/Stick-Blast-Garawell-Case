using System;
using UnityEngine;

namespace GarawellGames.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private SoundEffect[] soundEffects;
        
        public void PlayAnySound(SoundType type)
        {
            foreach (var effect in soundEffects)
            {
                if (effect.type == type)
                {
                    if (!NullCheck(effect.clip))
                    {
                        if (NullCheck(audioSource))
                        {
                            audioSource = gameObject.AddComponent<AudioSource>();
                        }

                        audioSource.PlayOneShot(effect.clip);
                    }
                    else
                    {
                        Debug.LogError("Audio Clip is Null!" * Colorize.Red);
                    }
                }
            }
        }
        
        private bool NullCheck(object T)
        {
            return T == null;
        }

        [Serializable]
        public enum SoundType
        {
            BLOCK_SELECT,
            BLOCK_PLACED,
            CELL_FILLED,
            ROW_COLUMN_FILLED,
            SUCCES,
            FAIL,
            BUTTON_CLICK,
            SPAWN_BLOCK
        }
    }

    [Serializable]
    public class SoundEffect
    {
        public AudioManager.SoundType type;
        public AudioClip clip;
    }
}