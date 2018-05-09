using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace ru.lifanoff {

    /// <summary>
    /// Информация о сцене(сценах)
    /// </summary>
    public static class SceneInformation {

        /// <summary>Список активных сцен в меню "File > BuildSettings"</summary>
        private static List<string> activeNames = new List<string>();
        /// <summary>Список активных сцен в меню "File > BuildSettings"</summary>
        public static List<string> ActiveNames {
            get {
                return activeNames;
            }
        }

        static SceneInformation() {
            MakeActiveNames(); // Сразу назначить в _activeNames назвние всех активных сцен
        }

        /// <summary>Составить список активных сцен. Т.е. сцен, которые расположеные в меню "File > BuildSettings"</summary>
        private static void MakeActiveNames() {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++) {
                string scenePathName = SceneUtility.GetScenePathByBuildIndex(i);
                activeNames.Add(Regex.Replace(scenePathName, @"^Assets/(.+)\.unity$", @"$1", RegexOptions.IgnoreCase));
            }
        }

        /// <summary>Получить название текущей сцены</summary>
        public static string GetCurrentSceneName() {
            return SceneManager.GetActiveScene().name;
        }
    }

}