using UnityEditor;
using UnityEditor.SceneManagement;

namespace NetcodeForGameObjects.Editor
{
    [InitializeOnLoad]
    public static class StartSceneLoader
    {
        static StartSceneLoader()
        {
            EditorApplication.playModeStateChanged += LoadStartScene;
        }

        private static void LoadStartScene(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }

            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                if (EditorSceneManager.GetActiveScene().buildIndex != 0)
                {
                    EditorSceneManager.LoadScene(0);
                }
            }
        }
    }
}
