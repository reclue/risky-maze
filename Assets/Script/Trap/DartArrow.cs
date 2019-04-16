using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ru.lifanoff.Player;

namespace ru.lifanoff.Trap {
    public class DartArrow : MonoBehaviour {

        [SerializeField] private Transform raytraceTransform = null;

        [Header("Озвучка")]
        [SerializeField] private AudioSource audioSource = null;
        [SerializeField] private AudioClip clickArrowSound = null;
        [SerializeField] private AudioClip flyArrowSound = null;
        [SerializeField] private AudioClip hitArrowSound = null;

        private bool isTrigger = false;

        void Start() {
            StartCoroutine(CheckPlayerOrPebble());
        }

        void OnTriggerEnter(Collider other) {
            if (other.CompareTag(Unchangeable.ARROW_TRAP_TAG)) {
                Destroy(gameObject);
            }

            if (!other.CompareTag(Unchangeable.PLAYER_TAG) &&
                !other.CompareTag(Unchangeable.PEBBLE_TAG) &&
                !other.CompareTag(Unchangeable.TRAP_TAG) &&
                !other.CompareTag(Unchangeable.EXIT_KEY_TAG)) {
                isTrigger = true;
            }
        }

        void OnCollisionEnter(Collision collision) {
            if (collision.transform.CompareTag(Unchangeable.ARROW_TRAP_TAG)) {
                Destroy(gameObject);
            }

            if (!collision.transform.CompareTag(Unchangeable.PLAYER_TAG) &&
                !collision.transform.CompareTag(Unchangeable.PEBBLE_TAG) &&
                !collision.transform.CompareTag(Unchangeable.TRAP_TAG) &&
                !collision.transform.CompareTag(Unchangeable.EXIT_KEY_TAG)) {
                isTrigger = true;
            }
        }

        private IEnumerator CheckPlayerOrPebble() {
            Ray ray = new Ray(raytraceTransform.position, raytraceTransform.forward);
            RaycastHit currentHit = new RaycastHit();
            float distance = 3f;
            float waitSecond = Mathf.Clamp(Convert.ToSingle(SecondaryFunctions.GetNewRandom().NextDouble()), 0.4f, 0.5f);

            while (true) {
                if (Physics.Raycast(ray, out currentHit, distance)) {
                    if (currentHit.transform.CompareTag(Unchangeable.PLAYER_TAG) ||
                        currentHit.transform.CompareTag(Unchangeable.PEBBLE_TAG)) {
                        audioSource.clip = clickArrowSound;
                        audioSource.Play();
                        yield return new WaitForSeconds(0.5f - waitSecond);
                        audioSource.Stop();
                        audioSource.clip = flyArrowSound;
                        audioSource.loop = true;
                        audioSource.Play();
                        StartCoroutine(MoveArrow());
                        break;
                    }
                }

                yield return new WaitForSeconds(waitSecond);
            }
        }


        private IEnumerator MoveArrow() {
            Vector3 direction = raytraceTransform.forward;
            Destroy(raytraceTransform.gameObject);

            Rigidbody rb = GetComponent<Rigidbody>();

            float speed = .15f;

            while (!isTrigger) {
                rb.MovePosition(transform.position + direction * speed);
                yield return null;
            }

            Destroy(GetComponent<DamagePlayer>());
            Destroy(GetComponent<Collider>());
            Destroy(GetComponent<Rigidbody>());

            audioSource.Stop();
            audioSource.clip = hitArrowSound;
            audioSource.loop = false;
            audioSource.Play();

            while (audioSource.isPlaying) {
                yield return new WaitForSeconds(0.1f);
            }

            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            Destroy(GetComponent<DartArrow>());
        }

    }
}
