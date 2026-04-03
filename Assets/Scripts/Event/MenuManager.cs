using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject pauseMenu;
    bool isPaused;
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        if (pauseMenu != null) pauseMenu.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.Instance.LoadScene("StartScene");
    }

    public void Exit()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        if (pauseMenu != null) pauseMenu.SetActive(true);
    }

    private void Update()
    {
        if (InputManager.pauseWasPressedThisFrame && !isPaused)
        {
            Pause();
        }
    }
}
