using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

[InitializeOnLoad]
public class SceneSwitchLeftButton
{
    static SceneSwitchLeftButton()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if(GUILayout.Button(new GUIContent("▶", "Start Playing from first scene"), new GUIStyle(GUI.skin.button){stretchWidth = true, stretchHeight = true}))
        {
            PlayFromPrelaunchScene();
        }
    }
    
    public static void PlayFromPrelaunchScene()
    {
        if ( EditorApplication.isPlaying == true )
        {
            EditorApplication.isPlaying = false;
            return;
        }
        EditorApplication.SaveCurrentSceneIfUserWantsTo();
        string path = "Assets/Scenes/HomeScene.unity";
        EditorApplication.OpenScene(path);
        EditorApplication.isPlaying = true;
    }
}
