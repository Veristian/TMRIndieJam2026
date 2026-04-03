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
        HideCursor();
    }
    public void Restart()
    {
        SceneManager.Instance.LoadScene("StartScene");
        Time.timeScale = 1;
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
        RevealCursor();
    }

    private void Update()
    {
        if (InputManager.pauseWasPressedThisFrame && !isPaused)
        {
            Pause();
        }
    }

    private void Start()
    {
        HideCursor();
    }

    void HideCursor()
    {
        // Locks cursor to center and hides it automatically
        Cursor.lockState = CursorLockMode.Locked;
        // Explicitly hide it (Locked state hides it anyway)
        Cursor.visible = false;    

    }
    void RevealCursor()
    {
        // Locks cursor to center and hides it automatically
        Cursor.lockState = CursorLockMode.None;
        // Explicitly hide it (Locked state hides it anyway)
        Cursor.visible = true;    

    }
}
