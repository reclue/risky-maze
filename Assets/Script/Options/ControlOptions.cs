using System.Collections.Generic;
using UnityEngine;

namespace ru.lifanoff.Options {

    /// <summary>
    /// Класс для настроек управления
    /// </summary>
    [System.Serializable]
    public class ControlOptions {
        public float mouseSensitivityX = 1.5f;
        public float mouseSensitivityY = 1.5f;

        public Dictionary<KeyName, KeyCode> keyButtons = new Dictionary<KeyName, KeyCode> {
            { KeyName.UP, KeyCode.W },
            { KeyName.DOWN, KeyCode.S },
            { KeyName.LEFT, KeyCode.A },
            { KeyName.RIGHT, KeyCode.D },
            { KeyName.RUN, KeyCode.LeftShift },
            { KeyName.JUMP, KeyCode.Space },
            { KeyName.USE, KeyCode.E },
            { KeyName.THROW_PEBBLE, KeyCode.Mouse0 },
        };
    }//class
}//namespace
