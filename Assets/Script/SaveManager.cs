using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using ru.lifanoff.Options;

namespace ru.lifanoff {

    /// <summary>
    /// Класс-синглтон, для хранения настроек игры
    /// </summary>
    [Serializable]
    public class SaveManager {
        [NonSerialized]
        public static string FILEPATH = Path.GetFullPath(@"./saves/CourseworkSave.bin"); // Полный путь к сейв-файлу
        [NonSerialized]
        public static string FILEDIR = Path.GetDirectoryName(FILEPATH); // Директория в который расположен сейв-файл
        [NonSerialized]
        public static string FILENAME = Path.GetFileName(FILEPATH); // Только имя и расширение сейв-файл

        /// <summary>Единственный экземпляр класса <seealso cref="SaveManager"/></summary>
        private static SaveManager instance;
        /// <summary>Единственный экземпляр класса <seealso cref="SaveManager"/></summary>
        public static SaveManager Instance {
            get { return instance; }
        }

        static SaveManager() {
            if (instance == null) {
                instance = new SaveManager();
            }

            instance.Load();
        }

        private SaveManager() { }


        #region Сохраняемые данные
        /// <summary>Ссылка на единственный экземпляр класса OptionsManager</summary>
        private OptionsManager optionsManager = OptionsManager.Instance;
        #endregion


        /// <summary>
        /// Сохранить (сериализовать) данные в файл <seealso cref="FILEPATH"/>
        /// </summary>
        public void Save() {
            if (!Directory.Exists(FILEDIR)) {
                Directory.CreateDirectory(FILEDIR);
            }

            using (FileStream fs = new FileStream(FILEPATH, FileMode.OpenOrCreate)) {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, instance);
            }
        }

        /// <summary>
        /// Выгрузить (десериализовать) данные из файла <seealso cref="FILEPATH"/>
        /// </summary>
        public bool Load() {
            if (File.Exists(FILEPATH)) {
                try {
                    using (FileStream fs = new FileStream(FILEPATH, FileMode.Open)) {
                        BinaryFormatter bf = new BinaryFormatter();
                        Initialize((SaveManager)bf.Deserialize(fs));
                    }

                    return true;
                } catch (Exception) {
                    return false;
                }
            }

            return false;
        }


        /// <summary>
        /// Инициализация текущих значений параметрами из сейв-файла. 
        /// Если в старой версии сейва нет какого-либо свойства, котрое есть в новой версии сейва,
        /// то этому свойству в новом сейве будет присвоено значение по-умолчанию вместо null.
        /// Так сделано для обратной совместимости разных версий сейвов.
        /// </summary>
        /// <param name="oldSaver">Сохраненный на диске десериализованный файл</param>
        private void Initialize(SaveManager oldSaver) {
            try {
                optionsManager.controlOptions.keyButtons = oldSaver.optionsManager.controlOptions.keyButtons;
            } catch (Exception) { }

            try {
                optionsManager.graphicsOptions.antialiasing = oldSaver.optionsManager.graphicsOptions.antialiasing;
            } catch (Exception) { }

            try {
                optionsManager.graphicsOptions.isFullscreen = oldSaver.optionsManager.graphicsOptions.isFullscreen;
            } catch (Exception) { }

            try {
                optionsManager.graphicsOptions.resolution = oldSaver.optionsManager.graphicsOptions.resolution;
            } catch (Exception) { }

            try {
                optionsManager.graphicsOptions.textureQuality = oldSaver.optionsManager.graphicsOptions.textureQuality;
            } catch (Exception) { }

            try {
                optionsManager.graphicsOptions.vSync = oldSaver.optionsManager.graphicsOptions.vSync;
            } catch (Exception) { }

            try {
                optionsManager.musicOptions.musicValue = oldSaver.optionsManager.musicOptions.musicValue;
            } catch (Exception) { }

        }

    }//class
}//namespace
