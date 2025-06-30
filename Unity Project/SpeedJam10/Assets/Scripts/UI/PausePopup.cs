using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePopup : MonoBehaviour
{
    public GameManager GManager = null;

    public void OpenPopup()
    {
        GManager.SetPaused(true);
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void ClosePopup()
    {
        GManager.SetPaused(false);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnResumeButton()
    {
        ClosePopup();
    }

    public void OnMainMenuButton()
    {
        GManager.SetPaused(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
