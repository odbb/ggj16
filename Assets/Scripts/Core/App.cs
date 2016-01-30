using UnityEditor;
using UnityEngine;

public class App : ScriptableObject
{
    public Sprite iconTexture;
    public AppBehaviour appBehaviourPrefab;
    public string appName;

    [MenuItem("Assets/Create/App")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<App>();
    }
}