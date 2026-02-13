using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{

    public GameObject pauseMenuCanvas;
    public GameObject controlsMenuCanvas;
    private bool isPaused;
    private bool showingControls;

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        MainMenuController.instance.QuitGame();
    }

    public void ShowControls()
    {
        controlsMenuCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        showingControls = true;
    }

    public void HideControls()
    {
        controlsMenuCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        showingControls = false;
    }

    public void TogglePauseMenu()
    {
        if (!isPaused && !showingControls)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else if (isPaused && !showingControls)
        {
            ResumeGame();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPaused = false;
        showingControls = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }
}
