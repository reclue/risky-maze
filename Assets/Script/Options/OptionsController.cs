using UnityEngine;
using UnityEngine.UI;

namespace ru.lifanoff.Options {

    /// <summary>
    /// Класс, для управления панелью опций и меню
    /// </summary>
    public class OptionsController : MonoBehaviour {

        #region Игровые объекты, которые инициализируются в панели Inspector редактора
        [Header("Opitons Menu Layout Panel")]
        [SerializeField] private GameObject graphicsPanel = null;
        [SerializeField] private GameObject musicPanel = null;
        [SerializeField] private GameObject controlPanel = null;

        [Header("Canvas Panel")]
        [SerializeField] private GameObject mainMenuCanvas = null;
        [SerializeField] private GameObject optionsMenuCanvas = null;
        [SerializeField] private GameObject difficultCanvas = null;

        [Header("Объекты из панели GraphicsPanel для сериализации")]
        [SerializeField] private Toggle fullscreenToggle = null;
        [SerializeField] private Dropdown resolutionDropdown = null;
        [SerializeField] private Dropdown textureDropdown = null;
        [SerializeField] private Dropdown antialiasingDropdown = null;
        [SerializeField] private Dropdown vSyncDropdown = null;

        [Header("Объекты из панели MusicPanel для сериализации")]
        [SerializeField] private Slider musicSlider = null;

        [Header("Объекты из панели ControlPanel для сериализации")]
        [SerializeField] private Slider mouseSensitivityX = null;
        [SerializeField] private Slider mouseSensitivityY = null;
        [SerializeField] private Button upKeyButton = null;
        [SerializeField] private Button downKeyButton = null;
        [SerializeField] private Button leftKeyButton = null;
        [SerializeField] private Button rightKeyButton = null;
        [SerializeField] private Button runKeyButton = null;
        [SerializeField] private Button jumpKeyButton = null;
        [SerializeField] private Button useKeyButton = null;
        [SerializeField] private Button throwPebbleKeyButton = null;
        #endregion

        /// <summary>Список разрешений экрана</summary>
        private Resolution[] resolutions;

        /// <summary>Единственный экземпляр класса <seealso cref="SaveManager"/></summary>
        private SaveManager saveInstance;
        /// <summary>Единственный экземпляр класса <seealso cref="OptionsManager"/></summary>
        private OptionsManager optionsInstance;

        #region Unity Events
        void Start() {
            saveInstance = SaveManager.Instance;
            optionsInstance = OptionsManager.Instance;

            CursorController.Instance.CursorIsHide = false;

            /* Заполнение списка разрешений экрана */
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            foreach (Resolution resolution in resolutions) {
                resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
            }

            /* Настройка меню, которое должно отображаться изначально при старте игры */
            mainMenuCanvas.SetActive(true);
            optionsMenuCanvas.SetActive(false);
            difficultCanvas.SetActive(false);

            // Применить настройки игры из файла-сохранения
            ApplySaveOptions();
        }
        #endregion


        #region Функции, которые запускаются при нажатии кнопок в Difficult
        /// <summary>Реакция на нажатие кнопки Back в меню выбора сложности игры</summary>
        public void DifficultBackButton() {
            mainMenuCanvas.SetActive(true);
            optionsMenuCanvas.SetActive(false);
            difficultCanvas.SetActive(false);
        }

        /// <summary>Запуск игры в легком режиме</summary>
        public void DifficultEasyGameButton() {
            GameController.Instance.difficulMode = DifficultMode.EASY;
            GameController.Instance.GoToNextScene(Unchangeable.GAME_SCENE_NAME);
        }

        /// <summary>Запуск игры в среднем режиме</summary>
        public void DifficultMediumGameButton() {
            GameController.Instance.difficulMode = DifficultMode.MEDIUM;
            GameController.Instance.GoToNextScene(Unchangeable.GAME_SCENE_NAME);
        }

        /// <summary>Запуск игры в сложном режиме</summary>
        public void DifficultHardGameButton() {
            GameController.Instance.difficulMode = DifficultMode.HARD;
            GameController.Instance.GoToNextScene(Unchangeable.GAME_SCENE_NAME);
        }
        #endregion


        #region Функции, которые запускаются при нажатии кнопок в Main Menu
        /// <summary>Реакция на нажатие кнопки Game в главном меню (MainMenu)</summary>
        public void MainMenuGameButton() {
            mainMenuCanvas.SetActive(false);
            optionsMenuCanvas.SetActive(false);
            difficultCanvas.SetActive(true);
        }

        /// <summary>Реакция на нажатие кнопки Options в главном меню (MainMenu)</summary>
        public void MainMenuOptionsButton() {
            mainMenuCanvas.SetActive(false);
            difficultCanvas.SetActive(false);
            /* При переходе из main menu в options надо сделать видимым слой graphics, 
             * а остальные слои скрыть. Для получения желаемого результат можно
             * использовать метод OptionMenuGraphicsButton() */
            OptionMenuGraphicsButton();
            optionsMenuCanvas.SetActive(true);
        }

        /// <summary>Реакция на нажатие кнопки Exit в главном меню (MainMenu)</summary>
        public void MainMenuExitButton() {
            Application.Quit();
        }
        #endregion


        #region Книпки Apply и Back в меню опций
        /// <summary>Реакция на нажатие кнопки ApplyButton в меню опций</summary>
        public void OptionMenuApplyButton() {
            SaveOptions();
            ApplySaveOptions();
        }

        /// <summary>Реакция на нажатие кнопки BackButton в меню опций</summary>
        public void OptionMenuBackButton() {
            mainMenuCanvas.SetActive(true);
            optionsMenuCanvas.SetActive(false);
            difficultCanvas.SetActive(false);
        }
        #endregion


        #region Функции для кнопок в LayoutTrigger, переключающие Layouts панели
        /// <summary>Деактивировать все панели</summary>
        private void AllDeactivate() {
            graphicsPanel.SetActive(false);
            musicPanel.SetActive(false);
            controlPanel.SetActive(false);
        }

        /// <summary>Активировать только панель Graphics и отобразить акутальные настройки</summary>
        public void OptionMenuGraphicsButton() {
            AllDeactivate();
            InsertDefaultOptions();
            graphicsPanel.SetActive(true);
        }

        /// <summary>Активировать только панель Music и отобразить акутальные настройки</summary>
        public void OptionMenuMusicButton() {
            AllDeactivate();
            InsertDefaultOptions();
            musicPanel.SetActive(true);
        }

        /// <summary>Активировать только панель Control и отобразить акутальные настройки</summary>
        public void OptionMenuControlButton() {
            AllDeactivate();
            InsertDefaultOptions();
            controlPanel.SetActive(true);
        }
        #endregion


        /// <summary>Присвоить значения настройкам в соответствии с данными в сейве</summary>
        private void InsertDefaultOptions() {
            fullscreenToggle.isOn = optionsInstance.graphicsOptions.isFullscreen;
            resolutionDropdown.value = optionsInstance.graphicsOptions.resolution;
            textureDropdown.value = optionsInstance.graphicsOptions.textureQuality;
            antialiasingDropdown.value = optionsInstance.graphicsOptions.antialiasing;
            vSyncDropdown.value = optionsInstance.graphicsOptions.vSync;

            musicSlider.value = optionsInstance.musicOptions.musicVolume;

            mouseSensitivityX.value = optionsInstance.controlOptions.mouseSensitivityX;
            mouseSensitivityY.value = optionsInstance.controlOptions.mouseSensitivityY;

            /* Обновить названия клавиш в кнопках */
            upKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.UP].ToString();
            downKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.DOWN].ToString();
            leftKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.LEFT].ToString();
            rightKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.RIGHT].ToString();
            runKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.RUN].ToString();
            jumpKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.JUMP].ToString();
            useKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.USE].ToString();
            throwPebbleKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.THROW_PEBBLE].ToString();

            /* Обновить измененные выпадающие списки (Dropdown) */
            resolutionDropdown.RefreshShownValue();
            textureDropdown.RefreshShownValue();
            antialiasingDropdown.RefreshShownValue();
            vSyncDropdown.RefreshShownValue();
        }


        /// <summary>Сохранить (сериализовать) новые значения настроек</summary>
        private void SaveOptions() {
            optionsInstance.graphicsOptions.isFullscreen = fullscreenToggle.isOn;
            optionsInstance.graphicsOptions.resolution = resolutionDropdown.value;
            optionsInstance.graphicsOptions.textureQuality = textureDropdown.value;
            optionsInstance.graphicsOptions.antialiasing = antialiasingDropdown.value;
            optionsInstance.graphicsOptions.vSync = vSyncDropdown.value;

            optionsInstance.musicOptions.musicVolume = musicSlider.value;

            optionsInstance.controlOptions.mouseSensitivityX = mouseSensitivityX.value;
            optionsInstance.controlOptions.mouseSensitivityY = mouseSensitivityY.value;

            saveInstance.Save();
        }

        /// <summary>Применить настройки из файла-сохранения</summary>
        private void ApplySaveOptions() {
            Screen.fullScreen = optionsInstance.graphicsOptions.isFullscreen;
            Resolution currentResolution = resolutions[optionsInstance.graphicsOptions.resolution];
            Screen.SetResolution(currentResolution.width, currentResolution.height, optionsInstance.graphicsOptions.isFullscreen);

            QualitySettings.vSyncCount = optionsInstance.graphicsOptions.vSync;
            QualitySettings.masterTextureLimit = optionsInstance.graphicsOptions.textureQuality;

            /* Параметр optionsInstance.graphicsOptions.antialiasing сохраняет не само значение antialiasing,
             * а текущую выбранную позицию в списке всех возможных вариантов antialiasing.
             * Поэтому, чтобы получить реальное значение для QualitySettings.antiAliasing,
             * нужно возвести двойку в степень optionsInstance.graphicsOptions.antialiasing */
            QualitySettings.antiAliasing = (int)Mathf.Pow(2, optionsInstance.graphicsOptions.antialiasing);

            SoundController.Instance.ChangeVolume(optionsInstance.musicOptions.musicVolume);
        }

    }//class
}//namespace