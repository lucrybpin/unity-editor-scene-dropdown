using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace EditorSceneDropdown
{
    public class EditorSceneLoader : EditorWindow
    {
        [MenuItem("Window/Editor Scene Loader")]
        static void OpenWindow()
        {
            GetWindow<EditorSceneLoader>().Show();
        }

        private string selectedScenePath;

        private void OnGUI()
        {
            // EditorGUILayout.LabelField("Scene Selection", EditorStyles.boldLabel);

            var buildScenes = GetBuildScenes();
            if (buildScenes.Count == 0)
            {
                EditorGUILayout.HelpBox("No scenes in Build Settings", MessageType.Warning);
                return;
            }

            int currentIndex = 0;
            for (int i = 0; i < buildScenes.Count; i++)
            {
                if (buildScenes[i].path == selectedScenePath)
                {
                    currentIndex = i;
                    break;
                }
            }

            int newIndex = EditorGUILayout.Popup("Scene", currentIndex, GetSceneNames(buildScenes));
            selectedScenePath = buildScenes[newIndex].path;

            if (GUILayout.Button("Load Scene"))
            {
                LoadScene();
            }
        }

        private List<EditorBuildSettingsScene> GetBuildScenes()
        {
            var scenes = new List<EditorBuildSettingsScene>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(scene);
                }
            }
            return scenes;
        }

        private string[] GetSceneNames(List<EditorBuildSettingsScene> scenes)
        {
            string[] names = new string[scenes.Count];
            for (int i = 0; i < scenes.Count; i++)
            {
                names[i] = Path.GetFileNameWithoutExtension(scenes[i].path);
            }
            return names;
        }

        private void LoadScene()
        {
            if (!string.IsNullOrEmpty(selectedScenePath))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(selectedScenePath);
                }
            }
            else
            {
                Debug.LogWarning("No scene selected.");
            }
        }
    }
}