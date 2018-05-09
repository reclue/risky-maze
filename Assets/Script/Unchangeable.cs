using System.Collections.Generic;
using UnityEngine;

namespace ru.lifanoff {

    /// <summary>
    /// Статичный класс, содержащий общие константные значения
    /// </summary>
    public static class Unchangeable {

        #region Time.timeScale
        public const float DEFAULT_TIMESCALE = 1f;
        // public const float PAUSE_TIMESCALE = .001f;
        #endregion

        #region Tags
        public const string PLAYER_TAG = "Player";
        public const string CAMERA_PLAYER_TAG = "PlayerCamera";
        public const string PLAYER_HUD_TAG = "PlayerHUD";
        public const string GAME_CONTROLLER_TAG = "GameController";
        #endregion


        #region Layers
        public const string DEFAULT_LAYER = "Default";
        public const string UI_LAYER = "UI";
        // public const string DRAW_ALWAYS_LAYER = "DrawAlways";
        // public const string WATER_LAYER = "Water";
        #endregion


        #region Colors
        public const string FOG_COLOR = "969797FF";
        #endregion

        #region Player
        public static float DISTANCE_PLAYER_RAYCAST = 2.05f;
        #endregion

        #region Scene
        public const string PRELOADER_SCENE_NAME = "PreLoader";
        public const string INTRO_SCENE_NAME = "Intro";
        public const string GAME_SCENE_NAME = "Game";
        public const string MAIN_MENU_SCENE_NAME = "MainMenu";
        public const string RESULT_SCENE_NAME = "Result";
        #endregion

        #region Chunk
        public const float CHUNK_SIZE = 4f; // 4 юнита
        #endregion


    }

}