#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

/// <summary>
/// This button can be used to load the bootstrap scene before the active editor scene. This way we can start
/// from any scene and this script will automatically initialize all managers and such base systems.
/// </summary>
[InitializeOnLoad]
public static class BootstrapButton
{
    private const string BOOTSTRAP_SCENE_PATH = "Assets/Scenes/Bootstrap.unity";
    private const string CURRENT_EDITOR_SCENE = "currentEditorScene";
    private const string IS_BOOTSTRAP = "isBootstrap";

    static BootstrapButton()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
        
    [MainToolbarElement("Bootstrap/Bootstrap Play Button", defaultDockPosition = MainToolbarDockPosition.Middle)]
    public static MainToolbarElement PlayWithBootstrapButton()
    {
        var icon = (Texture2D)EditorGUIUtility.Load("Assets/Art/Sprites/EditorIcons/play_plus_icon.png");
        var content = new MainToolbarContent(icon);
        return new MainToolbarButton(content, EnterPlayModeWithCore);
    }
    
    private static void EnterPlayModeWithCore()
    {
        // Save current open scene and start from the bootstrap scene instead
        string sceneName = SceneManager.GetActiveScene().name;
        EditorPrefs.SetString(CURRENT_EDITOR_SCENE, sceneName);
        EditorPrefs.SetBool(IS_BOOTSTRAP, true);
        SceneAsset bootstrapScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(BOOTSTRAP_SCENE_PATH);
        EditorSceneManager.playModeStartScene = bootstrapScene;
        
        EditorApplication.EnterPlaymode();
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredPlayMode:
                if (EditorPrefs.GetBool(IS_BOOTSTRAP))
                {
                    string curEditorScene = EditorPrefs.GetString(CURRENT_EDITOR_SCENE);
                    SceneManager.LoadScene(curEditorScene, LoadSceneMode.Single);
                }
                break;
            case PlayModeStateChange.EnteredEditMode:
                string scenePath = SceneManager.GetActiveScene().path;
                EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                EditorPrefs.SetBool(IS_BOOTSTRAP, false);
                break;
        }
    }
}
#endif