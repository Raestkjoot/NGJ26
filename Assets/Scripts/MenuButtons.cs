using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : Singleton<MenuButtons>
{
    [SerializeField] private BirdController _player;

    private void Start()
    {
        _player.enabled = false;
    }

    public void StartGame()
    {
        _player.enabled = true;
        Destroy(gameObject);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
