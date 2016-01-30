#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class App : ScriptableObject
{
	public Sprite iconTexture;
	public AppBehaviourBase appBehaviourPrefab;
	public string appName;
	public string sceneName;

#if UNITY_EDITOR
	[MenuItem("Assets/Create/App")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<App>();
	}
#endif
}