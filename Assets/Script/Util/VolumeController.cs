using System;
using System.Collections;
using UnityEngine;


namespace ru.lifanoff.Util {
    /// <summary>
    /// Класс, который контролирует звук
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class VolumeController : MonoBehaviour {

        private AudioSource audioSource = null;

        void Awake() {
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = SaveManager.Instance.optionsManager.musicOptions.musicVolume;

            StartCoroutine(CheckVolume());
        }

        private IEnumerator CheckVolume() {
            audioSource.volume = SaveManager.Instance.optionsManager.musicOptions.musicVolume;

            while (audioSource != null && audioSource.clip != null) {
                if (PauseController.isPaused) {
                    if (audioSource.isPlaying) {
                        audioSource.Pause();
                    }
                } else {
                    if (!audioSource.isPlaying) {
                        audioSource.UnPause();
                    }

                    if (audioSource.volume != SaveManager.Instance.optionsManager.musicOptions.musicVolume) {
                        audioSource.volume = SaveManager.Instance.optionsManager.musicOptions.musicVolume;
                    }
                }

                yield return null;
            }
        }
    }

}
