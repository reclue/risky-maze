using UnityEngine;

namespace ru.lifanoff {

    /// <summary>
    /// Управление звуковым сопровождением
    /// </summary>
    public class SoundController : MonoBehaviour {

        [Header("Музыка для сцены Game")]
        [Tooltip("Фоновый звук ветра")]
        [SerializeField] private AudioClip gameBackgroundAmbience = null;
        [Tooltip("Набор фоновой музыки")]
        [SerializeField] private AudioClip[] gameBackgroundMusics = null;


        [Header("Звуки подбора предметов")]
        [Tooltip("Общий звук")]
        [SerializeField] private AudioClip commonPickupSound = null;
        [Tooltip("Поднятие ключа от выхода")]
        [SerializeField] private AudioClip exitKeyPickupSound = null;

        [Header("Озвучка игрока")]
        [Tooltip("Озвучка игрока, идущего по грязи")]
        [SerializeField] private AudioClip gamePlayerSlowdownSound = null;
        [Tooltip("Озвучка походки игрока")]
        [SerializeField] private AudioClip gamePlayerWalkingSound = null;
        [Tooltip("Озвучка бега игрока")]
        [SerializeField] private AudioClip gamePlayerRunningSound = null;

        [Tooltip("Звук начала прыжка игрока")]
        [SerializeField] private AudioClip gamePlayerStartJumpSound = null;
        [Tooltip("Набор звуков окончания прыжка игрока")]
        [SerializeField] private AudioClip[] gamePlayerEndJumpSounds = null;
        [SerializeField] private AudioClip[] gamePlayerPain = null;

        [Header("Звуки для остальных сцен")]
        [Tooltip("Фоновая музыка")]
        [SerializeField] private AudioClip commonBackgroundMusic = null;


        /// <summary>Единственный экземпляр класса <seealso cref="SoundController"/></summary>
        public static SoundController Instance { get; private set; }

        private SoundController() { }


        #region Источники звука
        /// <summary>Источник звука для коротких звуков</summary>
        private AudioSource oneShotAudioSource;
        /// <summary>Источник звука для игрока</summary>
        private AudioSource playerAudioSource;
        /// <summary>Источник звука для музыки</summary>
        private AudioSource musicAudioSource;
        /// <summary>Источник звука для фонового шума</summary>
        private AudioSource ambianceAudioSource;
        #endregion

        /// <summary>Текущая фоновая музыка в игре</summary>
        private AudioClip currentBackgroundMusic;

        #region Unity events
        void Awake() {
            if (Instance == null) {
                AddAudioSources();
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);

            Instance.InitSettings();
        }

        void Start() {
            Instance.InitSettings();
        }

        void Update() {
            if (GameController.Instance.currentSceneName == Unchangeable.GAME_SCENE_NAME) {
                if (Instance.musicAudioSource.clip == commonBackgroundMusic) {
                    currentBackgroundMusic = gameBackgroundMusics[Random.Range(0, gameBackgroundMusics.Length)];
                    Instance.StopMusic();
                    Instance.PlayMusic(currentBackgroundMusic);

                    Instance.StopAmbiance();
                    Instance.PlayAmbiance();
                }
            } else {
                if (Instance.musicAudioSource.clip != commonBackgroundMusic) {
                    Instance.StopAmbiance();
                    Instance.StopMusic();
                    Instance.PlayMusic(commonBackgroundMusic);
                }
            }
        }
        #endregion

        /// <summary>Добавить в текущий игровой объект источники звуков</summary>
        private void AddAudioSources() {
            oneShotAudioSource = gameObject.AddComponent<AudioSource>();
            playerAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource = gameObject.AddComponent<AudioSource>();
            ambianceAudioSource = gameObject.AddComponent<AudioSource>();

            oneShotAudioSource.playOnAwake = false;
            playerAudioSource.playOnAwake = false;
            musicAudioSource.playOnAwake = false;
            ambianceAudioSource.playOnAwake = false;

            oneShotAudioSource.loop = false;
            playerAudioSource.loop = true;
            musicAudioSource.loop = true;
            ambianceAudioSource.loop = true;

            oneShotAudioSource.Stop();
            playerAudioSource.Stop();
            musicAudioSource.Stop();
            ambianceAudioSource.Stop();
        }


        /// <summary>Специфические настройки</summary>
        private void InitSettings() {
            ChangeVolume(SaveManager.Instance.optionsManager.musicOptions.musicVolume);
        }


        /// <summary>Изменить громкость звукового сопровождения</summary>
        public void ChangeVolume(float volume) {
            volume = Mathf.Clamp01(volume);

            oneShotAudioSource.volume = volume;
            playerAudioSource.volume = volume;
            musicAudioSource.volume = volume / 2f;
            ambianceAudioSource.volume = volume;
        }

        #region Pickup Sounds
        /// <summary>Проиграить звук подбора любого предмета</summary>
        public void PlayCommonPickup() {
            if (commonPickupSound != null) {
                oneShotAudioSource.PlayOneShot(commonPickupSound);
            }
        }

        /// <summary>Проиграить звук подбора ключа от выхода</summary>
        public void PlayExitKeyPickup() {
            if (commonPickupSound != null) {
                oneShotAudioSource.PlayOneShot(exitKeyPickupSound);
            }
        }
        #endregion

        #region Player Sounds
        /// <summary>Проиграить звук окончания прыжка игрока</summary>
        /// <param name="chance">Вероятность проигрывания звука [0, 100]</param>
        public void PlayStartJumpPlayer(int chance = 100) {
            StopPlayerAudioSource();

            if (Random.Range(0, 100) < Mathf.Clamp(chance, 0, 100)) {
                oneShotAudioSource.PlayOneShot(gamePlayerStartJumpSound);
            }
        }

        /// <summary>Проиграить звук прыжка игрока</summary>
        public void PlayEndJumpPlayer() {
            AudioClip audioClip = null;
            while (audioClip == null) {
                audioClip = gamePlayerEndJumpSounds[Random.Range(0, gamePlayerEndJumpSounds.Length)];
            }

            oneShotAudioSource.PlayOneShot(audioClip);
        }

        /// <summary>Проиграить звук урона игрока</summary>
        public void PlayPainPlayer() {
            AudioClip audioClip = null;
            while (audioClip == null) {
                audioClip = gamePlayerPain[Random.Range(0, gamePlayerPain.Length)];
            }

            oneShotAudioSource.PlayOneShot(audioClip);
        }

        /// <summary>Проиграить звук игрока, идущего по грязи</summary>
        public void PlaySlowdownPlayer() {
            if (playerAudioSource.clip != gamePlayerSlowdownSound) {
                StopPlayerAudioSource();
                playerAudioSource.clip = gamePlayerSlowdownSound;
                playerAudioSource.Play();
            }
        }

        /// <summary>Проиграить звук идущего игрока</summary>
        public void PlayWalkingPlayer() {
            if (playerAudioSource.clip != gamePlayerWalkingSound) {
                StopPlayerAudioSource();
                playerAudioSource.clip = gamePlayerWalkingSound;
                playerAudioSource.Play();
            }
        }

        /// <summary>Проиграить звук бега игрока</summary>
        public void PlayRunningPlayer() {
            if (playerAudioSource.clip != gamePlayerRunningSound) {
                StopPlayerAudioSource();
                playerAudioSource.clip = gamePlayerRunningSound;
                playerAudioSource.Play();
            }
        }

        public void StopPlayerAudioSource() {
            if (playerAudioSource.isPlaying) {
                playerAudioSource.clip = null;
                playerAudioSource.Stop();
            }
        }

        public void PausePlayerAudioSource() {
            if (playerAudioSource.isPlaying) {
                playerAudioSource.Pause();
            }
        }

        public void UnPausePlayerAudioSource() {
            if (!playerAudioSource.isPlaying) {
                playerAudioSource.UnPause();
            }
        }
        #endregion Player Sounds


        #region Music
        /// <summary>Включить фоновую музыку</summary>
        /// <param name="audioClip">Проигрываемая фоновая музыка</param>
        public void PlayMusic(AudioClip audioClip) {
            musicAudioSource.clip = audioClip;
            musicAudioSource.Play();
        }

        /// <summary>Остановить музыку</summary>
        public void StopMusic() {
            if (musicAudioSource.isPlaying) {
                musicAudioSource.clip = null;
                musicAudioSource.Stop();
            }
        }
        #endregion Music


        #region Ambiance
        /// <summary>Включить фоновую музыку</summary>
        /// <param name="audioClip">Проигрываемая фоновая музыка</param>
        public void PlayAmbiance() {
            ambianceAudioSource.clip = gameBackgroundAmbience;
            ambianceAudioSource.Play();
        }

        /// <summary>Остановить музыку</summary>
        public void StopAmbiance() {
            if (ambianceAudioSource.isPlaying) {
                ambianceAudioSource.clip = null;
                ambianceAudioSource.Stop();
            }
        }
        #endregion Ambiance

    }//class
}//namespace
