using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ru.lifanoff.Trap {
    public class PebbleController : MonoBehaviour {
        private System.Random rnd = SecondaryFunctions.GetNewRandom();

        [Header("Звуки")]
        [Tooltip("Источник звуков")]
        [SerializeField] private AudioSource audioSource = null;
        [Tooltip("Звуки удара камня")]
        [SerializeField] private AudioClip[] hitPebbleSounds = null;

        #region Unity events
        void OnCollisionEnter(Collision collision) {
            if (audioSource == null) return;

            if (hitPebbleSounds != null && hitPebbleSounds.Length > 1) {
                if (audioSource.isPlaying) {
                    audioSource.Stop();
                }

                audioSource.clip = hitPebbleSounds[rnd.Next(0, hitPebbleSounds.Length)];
            }

            audioSource.Play();
        }

        void OnCollisionExit(Collision collision) {
            if (audioSource == null) return;

            audioSource.Stop();
        }
        #endregion

    }
}
