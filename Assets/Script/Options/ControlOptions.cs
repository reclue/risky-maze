using System;
using System.Collections.Generic;
using UnityEngine;

namespace ru.lifanoff.Options {

    /// <summary>
    /// Класс для настроек клавиш управления
    /// </summary>
    [Serializable]
    public class ControlOptions {
        public Dictionary<KeyName, KeyCode> keyButtons = new Dictionary<KeyName, KeyCode> {
            { KeyName.UP, KeyCode.W },
            { KeyName.DOWN, KeyCode.S },
            { KeyName.LEFT, KeyCode.A },
            { KeyName.RIGHT, KeyCode.D },
            { KeyName.JUMP, KeyCode.Space },
            { KeyName.USE, KeyCode.Mouse0 },
        };
    }//class
}//namespace
