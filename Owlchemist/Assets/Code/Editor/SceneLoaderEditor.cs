using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(World))]
public class SceneLoaderEditor : Editor
{
    /*public override void OnInspectorGUI()
    {
        World w = target as World;

        if (GUILayout.Button("Load Scenes"))
        {
            w.SetSceneVariables();
            w.StartCoroutine(LoadScenes(w));
        }

        base.OnInspectorGUI();
    }

    private IEnumerator LoadLevel(string path)
    {
        Debug.Log(path);
        yield return EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
    }

    private IEnumerator LoadScenes(World w)
    {
        int arrayLength = w.defaultScenesVariable.value.Length;
        for (int i = 0; i < arrayLength; i++)
        {
            Debug.Log("ArrayLength: " + arrayLength + " index: " + i);
            Scene sceneToLoad = EditorSceneManager.GetSceneByName(w.defaultScenesVariable.value[i]);
            if (!sceneToLoad.isLoaded)
            {
                yield return w.StartCoroutine(LoadLevel(w.scenes[i].path));
            }
        }
    }*/
}
