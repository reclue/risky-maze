using System.Collections.Generic;
using UnityEngine;

namespace ru.lifanoff {

    /// <summary>
    /// Статичный класс, содержащий константы
    /// </summary>
    public static class Unchangeable {

        #region Time.timeScale
        public const float DEFAULT_TIMESCALE = 1f;
        public const float PAUSE_TIMESCALE = 0f;
        #endregion

        #region Tags
        public const string PLAYER_TAG = "Player";
        public const string CAMERA_PLAYER_TAG = "PlayerCamera";
        public const string PLAYER_HUD_TAG = "PlayerHUD";
        public const string MESSAGE_TO_PLAYER_TAG = "MessageToPlayer";
        public const string GAME_CONTROLLER_TAG = "GameController";
        public const string SOUND_CONTROLLER_TAG = "SoundController";
        public const string MAZE_TAG = "Maze";
        public const string TRAP_TAG = "Trap";
        public const string ARROW_TRAP_TAG = "ArrowTrap";
        public const string PEBBLE_TAG = "Pebble";
        public const string EXIT_KEY_TAG = "ExitKey";
        #endregion

        #region Layers
        public const string DEFAULT_LAYER = "Default";
        public const string UI_LAYER = "UI";
        public const string DRAW_ALWAYS_LAYER = "Wall";
        public const string PLAYER_LAYER = "Player";
        #endregion

        #region Player
        public const float DISTANCE_PLAYER_RAYCAST = 2.05f;
        public const float SHOT_FORCE = 195f;
        public const int DEFAULT_COUNT_PEBBLES_EASY = 6;
        public const int DEFAULT_COUNT_PEBBLES_MEDIUM = 10;
        public const int DEFAULT_COUNT_PEBBLES_HARD = 15;
        public const int DEFAULT_COUNT_LIVES_EASY = 3;
        public const int DEFAULT_COUNT_LIVES_MEDIUM = 5;
        public const int DEFAULT_COUNT_LIVES_HARD = 8;
        #endregion

        #region Objects
        // Контрольная точка по Y, пересекая которую, объект должен быть перенесен на точку респауна
        public const float RESPAWN_POSITION_Y = -256f;
        #endregion

        #region Scene
        public const string PRELOADER_SCENE_NAME = "PreLoader";
        public const string INTRO_SCENE_NAME = "Intro";
        public const string GAME_SCENE_NAME = "Game";
        public const string MAIN_MENU_SCENE_NAME = "MainMenu";
        public const string RESULT_SCENE_NAME = "Result";
        public const string GAME_OVER_SCENE_NAME = "GameOver";
        #endregion

        #region Chunk
        /// <summary>Размер обного составного "блока" лабиринта</summary>
        public const float CHUNK_SIZE = 4f;
        #endregion

        #region Input Manager
        public const string HORIZONTAL_INPUT = "Horizontal";
        public const string LEFT_RIGHT_INPUT = HORIZONTAL_INPUT;
        public const string VERTICAL_INPUT = "Vertical";
        public const string UP_DOWN_INPUT = VERTICAL_INPUT;
        public const string FIRE2_INPUT = "Fire2";
        public const string FIRE3_INPUT = "Fire3";
        public const string JUMP_INPUT = "Jump";
        public const string MOUSE_X_INPUT = "Mouse X";
        public const string MOUSE_Y_INPUT = "Mouse Y";
        public const string MOUSE_SCROLLWHEEL_INPUT = "Mouse ScrollWheel";
        public const string ENTER_INPUT = "Submit";
        public const string RETURN_INPUT = ENTER_INPUT;
        public const string CANCEL_INPUT = "Cancel";
        public const string ESCAPE_INPUT = CANCEL_INPUT;
        public const string RUN_INPUT = "Run";
        public const string USE_INPUT = "Use";
        public const string DROP_PEBBLE_INPUT = "DropPebble";
        #endregion
    }

}