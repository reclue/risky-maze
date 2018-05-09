using UnityEngine;
using UnityEngine.UI;

namespace ru.lifanoff.Options {

    /// <summary>
    /// Класс, для управления панелью опций и меню
    /// </summary>
    public class OptionsController : MonoBehaviour {

        #region Игровые объекты, которые инициализируются в панели Inspector редактора
        [Header("Opitons Menu Layout Panel")]
        [SerializeField] private GameObject graphicsPanel;
        [SerializeField] private GameObject musicPanel;
        [SerializeField] private GameObject controlPanel;

        [Header("Canvas Panel")]
        [SerializeField] private GameObject mainMenuCanvas;
        [SerializeField] private GameObject optionsMenuCanvas;

        [Header("Объекты мз панели GraphicsPanel для сериализации")]
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Dropdown resolutionDropdown;
        [SerializeField] private Dropdown textureDropdown;
        [SerializeField] private Dropdown antialiasingDropdown;
        [SerializeField] private Dropdown vSyncDropdown;

        [Header("Объекты мз панели MusicPanel для сериализации")]
        [SerializeField] private Slider musicSlider;

        [Header("Объекты мз панели ControlPanel для сериализации")]
        [SerializeField] private Button upKeyButton;
        [SerializeField] private Button downKeyButton;
        [SerializeField] private Button leftKeyButton;
        [SerializeField] private Button rightKeyButton;
        [SerializeField] private Button jumpKeyButton;
        [SerializeField] private Button useKeyButton;
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

            // Применить настройки игры из файла-сохранения
            ApplySaveOptions();
        }
        #endregion



        #region Функции, которые запускаются при нажатии кнопок в Main Menu
        /// <summary>Реакция на нажатие кнопки Game в главном меню (MainMenu)</summary>
        public void MainMenuGameButton() {
            GameController.Instance.GoToNextScene(Unchangeable.GAME_SCENE_NAME);
        }

        /// <summary>Реакция на нажатие кнопки Options в главном меню (MainMenu)</summary>
        public void MainMenuOptionsButton() {
            mainMenuCanvas.SetActive(false);
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

            musicSlider.value = optionsInstance.musicOptions.musicValue;

            /* Обновить названия клавиш в кнопках */
            upKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.UP].ToString();
            downKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.DOWN].ToString();
            leftKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.LEFT].ToString();
            rightKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.RIGHT].ToString();
            jumpKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.JUMP].ToString();
            useKeyButton.GetComponentInChildren<Text>().text = optionsInstance.controlOptions.keyButtons[KeyName.USE].ToString();

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

            optionsInstance.musicOptions.musicValue = musicSlider.value;

            saveInstance.Save();
        }

        /// <summary>Применить настройки из файла-сохранения</summary>
        private void ApplySaveOptions() {
            Screen.fullScreen = optionsInstance.graphicsOptions.isFullscreen;
            Resolution currentResolution = resolutions[optionsInstance.graphicsOptions.resolution];
            Screen.SetResolution(currentResolution.width, currentResolution.height, optionsInstance.graphicsOptions.isFullscreen);
            QualitySettings.masterTextureLimit = optionsInstance.graphicsOptions.textureQuality;
            /* Параметр optionsInstance.graphicsOptions.antialiasing сохраняет не само значение antialiasing,
             * а текущую выбранную позицию в списке всех возможных вариантов antialiasing.
             * Поэтому, чтобы получить реальное значение для QualitySettings.antiAliasing,
             * происходит возведение двойки в степень optionsInstance.graphicsOptions.antialiasing */
            QualitySettings.antiAliasing = (int)Mathf.Pow(2, optionsInstance.graphicsOptions.antialiasing);
            QualitySettings.vSyncCount = optionsInstance.graphicsOptions.vSync;
        }

    }//class
}//namespace