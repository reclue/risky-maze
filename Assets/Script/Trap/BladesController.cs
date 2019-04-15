using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ru.lifanoff.Trap {
    public class BladesController : MonoBehaviour {
        private List<Vector3> coordinates = new List<Vector3>();

        void Start() {
            InitCoordinates();
            StartCoroutine(MoveBlades());
        }

        private void InitCoordinates() {
            for (int x = -1; x <= 1; x++) {
                for (int z = -1; z <= 1; z++) {
                    Vector3 pos = transform.position;
                    pos.x += Convert.ToSingle(x);
                    pos.z += Convert.ToSingle(z);
                    coordinates.Add(pos);
                }
            }
        }

        private IEnumerator MoveBlades() {
            Vector3 targetPosition = Vector3.zero;
            float speed = Mathf.Clamp(Convert.ToSingle(SecondaryFunctions.GetNewRandom().NextDouble() * 3.5), 2f, 3.5f);

            while (true) {
                do {
                    targetPosition = coordinates[UnityEngine.Random.Range(0, coordinates.Count)];
                    yield return null;
                } while (targetPosition == transform.position);
                
                while (transform.position != targetPosition) {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

                    yield return null;
                }

                yield return new WaitForSeconds(1.23f);
            }
        }

    }
}
