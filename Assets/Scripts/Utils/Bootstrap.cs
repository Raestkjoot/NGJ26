using UnityEngine;

/// <summary>
/// This script exists in the bootstrap scene and is responsible for loading the next scene in builds.
/// We can think of the scene at index 1 as the actual first scene in builds, 
/// since bootstrap will immediately load that once it has initialized.
/// The bootstrap scene should be scene 0.
/// </summary>
public class Bootstrap : MonoBehaviour
{
#if !UNITY_EDITOR
    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
#endif
}