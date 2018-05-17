using System;

namespace ru.lifanoff.Options {

    /// <summary>
    /// Класс для настроек графики
    /// </summary>
    [Serializable]
    public class GraphicsOptions {
        public bool isFullscreen = true;
        public int resolution = 2;
        public int textureQuality = 2;
        public int antialiasing = 0;
        public int vSync = 0;
    }//class
}//namespace
