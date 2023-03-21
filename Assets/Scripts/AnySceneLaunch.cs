using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnySceneLaunch : MonoBehaviour
{
    public static readonly int ANY_SCENE_LAUNCH_INDEX = 6;
    private static int targetSceneIndex = -1;


    /// <summary>
    /// This ensures we are always initialized before Awake
    /// Necessary when starting from scenes other than Main Menu. 
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void AnySceneInitialize()
    {
        targetSceneIndex = SceneManager.GetActiveScene().buildIndex;

        DeleteRootGameObjects();

        SceneManager.LoadScene(ANY_SCENE_LAUNCH_INDEX);
        FightDataLoader.TargetSceneIndex = targetSceneIndex;
    }

    private static void DeleteRootGameObjects()
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();

        List<GameObject> rootObjects = new List<GameObject>(gameObjects.Length);
        foreach(GameObject go in gameObjects) {
            if(go.transform.parent == null) {
                rootObjects.Add(go);
            }
        }
				
        foreach(GameObject root in rootObjects) {
            DestroyImmediate(root);
        }
    }
}