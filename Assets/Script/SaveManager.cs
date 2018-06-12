using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

using ru.lifanoff.Options;

namespace ru.lifanoff {

    /// <summary>
    /// Класс-синглтон, для хранения настроек игры
    /// </summary>
    [Serializable]
    public class SaveManager {
        /// <summary>Полный путь к сейв-файлу</summary>
        [NonSerialized] public static string FILEPATH = Path.GetFullPath(@"./saves/CourseworkSave.bin");
        /// <summary>Директория в который расположен сейв-файл</summary>
        [NonSerialized] public static string FILEDIR = Path.GetDirectoryName(FILEPATH);
        /// <summary>Только имя и расширение сейв-файл</summary>
        [NonSerialized] public static string FILENAME = Path.GetFileName(FILEPATH);

        /// <summary>Единственный экземпляр класса <seealso cref="SaveManager"/></summary>
        public static SaveManager Instance { get; private set; }

        static SaveManager() {
            if (Instance == null) {
                Instance = new SaveManager();
            }

            Instance.Load();
        }

        private SaveManager() { }


        /// <summary>Ссылка на единственный экземпляр класса OptionsManager</summary>
        public OptionsManager optionsManager { get; private set; } = OptionsManager.Instance;


        #region Данные для шифрования
        private const byte KEY_CRYPT = 236; // Число между 1 и 254
        #endregion

        /// <summary>
        /// Зашифровать и сохранить (сериализовать) данные в файл <seealso cref="FILEPATH"/>
        /// </summary>
        public void Save() {
            if (!Directory.Exists(FILEDIR)) {
                Directory.CreateDirectory(FILEDIR);
            }

            try {
                byte[] msArray = null;

                using (MemoryStream ms = new MemoryStream()) {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, Instance);
                    ms.Flush();

                    msArray = ms.ToArray();
                }

                if (msArray == null) return;

                for (int i = 0; i < msArray.Length; i++) {
                    msArray[i] = (byte)(msArray[i] ^ ((byte)(i % KEY_CRYPT)));
                }

                using (MemoryStream ms = new MemoryStream(msArray)) {
                    using (FileStream fs = new FileStream(FILEPATH, FileMode.OpenOrCreate)) {
                        ms.WriteTo(fs);
                    }
                }
            } catch (Exception) { }
        }

        /// <summary>
        /// Расшифровать и выгрузить (десериализовать) данные из файла <seealso cref="FILEPATH"/>
        /// </summary>
        public bool Load() {
            if (File.Exists(FILEPATH)) {
                try {
                    byte[] fileBytes = null;

                    using (FileStream fs = new FileStream(FILEPATH, FileMode.Open)) {
                        fileBytes = new byte[fs.Length];
                        fs.Read(fileBytes, 0, fileBytes.Length);
                    }

                    if (fileBytes == null) return false;

                    for (int i = 0; i < fileBytes.Length; i++) {
                        fileBytes[i] = (byte)(fileBytes[i] ^ ((byte)(i % KEY_CRYPT)));
                    }

                    using (MemoryStream ms = new MemoryStream(fileBytes)) {
                        BinaryFormatter bf = new BinaryFormatter();
                        Initialize((SaveManager)bf.Deserialize(ms));
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
                foreach (object keyName in oldSaver.optionsManager.controlOptions.keyButtons.Keys) {
                    if (keyName is KeyName) {
                        KeyName kn = (KeyName)keyName;
                        optionsManager.controlOptions.keyButtons.Add(kn, oldSaver.optionsManager.controlOptions.keyButtons[kn]);
                    }
                }
            } catch (Exception) { }

            optionsManager.controlOptions.mouseSensitivityX = oldSaver.optionsManager.controlOptions.mouseSensitivityX;
            optionsManager.controlOptions.mouseSensitivityY = oldSaver.optionsManager.controlOptions.mouseSensitivityY;

            optionsManager.graphicsOptions.antialiasing = oldSaver.optionsManager.graphicsOptions.antialiasing;
            optionsManager.graphicsOptions.isFullscreen = oldSaver.optionsManager.graphicsOptions.isFullscreen;
            optionsManager.graphicsOptions.resolution = oldSaver.optionsManager.graphicsOptions.resolution;
            optionsManager.graphicsOptions.textureQuality = oldSaver.optionsManager.graphicsOptions.textureQuality;
            optionsManager.graphicsOptions.vSync = oldSaver.optionsManager.graphicsOptions.vSync;

            optionsManager.musicOptions.musicVolume = oldSaver.optionsManager.musicOptions.musicVolume;
        }

    }//class
}//namespace
