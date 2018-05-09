using System;

namespace ru.lifanoff.Options {

    /// <summary>
    /// Класс-синглтон, который хранит настройки для класса <see cref="OptionsController"/>
    /// </summary>
    [Serializable]
    public class OptionsManager {

        /// <summary>Единственный экземпляр класса <seealso cref="OptionsManager"/></summary>
        private static OptionsManager instance;
        /// <summary>Единственный экземпляр класса <seealso cref="OptionsManager"/></summary>
        public static OptionsManager Instance {
            get { return instance; }
        }

        static OptionsManager() {
            if (instance == null) {
                instance = new OptionsManager();
            }
        }

        private OptionsManager() { }


        #region Menu Options
        /// <summary>Хранилище настроек графики</summary>
        private GraphicsOptions _graphicsOptions = new GraphicsOptions();
        /// <summary>Хранилище настроек графики</summary>
        public GraphicsOptions graphicsOptions {
            get { return _graphicsOptions; }
        }

        /// <summary>Хранилище настроек музыки и звуков</summary>
        private MusicOptions _musicOptions = new MusicOptions();
        /// <summary>Хранилище настроек музыки и звуков</summary>
        public MusicOptions musicOptions {
            get { return _musicOptions; }
        }
        /// <summary>Хранилище используемых в игре клавиш</summary>
        private ControlOptions _controlOptions = new ControlOptions();
        /// <summary>Хранилище используемых в игре клавиш</summary>
        public ControlOptions controlOptions {
            get { return _controlOptions; }
        }
        #endregion

    }//class
}//namespace
