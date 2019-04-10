using System;
using UnityEngine;

using ru.lifanoff.Maze;

namespace ru.lifanoff.PS {
    [RequireComponent(typeof(ParticleSystem))]
    public class PSDustController : MonoBehaviour {
        //Сгенерированный лабиринт
        private Maze.Maze maze = null;

        private ParticleSystem _particleSystem = null;

        #region Unity events
        void Start() {
            _particleSystem = GetComponent<ParticleSystem>();
            maze = SecondaryFunctions.GetMaze()?.GetComponent<Maze.Maze>();

            if (maze == null) return;

            InitParticleSystem();
        }
        #endregion

        #region Настройка системы частиц
        /// <summary>Настройка системы частиц</summary>
        private void InitParticleSystem() {
            InitShapeModule();
            InitCountParticles();
        }

        private void InitShapeModule() {
            ParticleSystem.ShapeModule shapeModule = _particleSystem.shape;
            shapeModule.enabled = true;
            shapeModule.shapeType = ParticleSystemShapeType.Box;
            shapeModule.scale = new Vector3(
                maze.GetMazeStructure.sizeX * Chunk.CHUNK_SIZE,
                Chunk.CHUNK_SIZE * 2f,
                maze.GetMazeStructure.sizeY * Chunk.CHUNK_SIZE
            );

            _particleSystem.transform.position = new Vector3(
                shapeModule.scale.x / 2f,
                shapeModule.scale.y / 2f,
                (shapeModule.scale.z / 2f) - 2f
            );
        }

        private void InitCountParticles() {
            int newMaxParticles = Convert.ToInt32(maze.GetMazeStructure.sizeX * maze.GetMazeStructure.sizeY * Chunk.CHUNK_SIZE);
            float newStartLifetime = newMaxParticles / _particleSystem.emission.rateOverTime.constant;

            ParticleSystem.MainModule mainModule = _particleSystem.main;
            mainModule.startLifetime = newStartLifetime;
            mainModule.maxParticles = newMaxParticles;
        }
        #endregion

    }
}