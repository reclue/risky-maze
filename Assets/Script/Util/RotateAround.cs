using System;
using System.Collections;
using UnityEngine;


namespace ru.lifanoff.Util {
    /// <summary>
    /// Класс, который вращает объект по оси y
    /// </summary>
    public class RotateAround : MonoBehaviour {
        private static System.Random rnd = SecondaryFunctions.GetNewRandom();

        private int currentSpeed = 0;

        [SerializeField] private int minSpeed = 300;
        [SerializeField] private int maxSpeed = 400;

        void Start() {
            currentSpeed = rnd.Next(minSpeed, maxSpeed);
            transform.Rotate(transform.up, rnd.Next(0, 360));
            StartCoroutine(RotateObject());
        }

        private IEnumerator RotateObject() {
            while (true) {
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * currentSpeed);
                yield return null;
            }
        }

    }

}
