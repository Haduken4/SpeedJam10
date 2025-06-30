using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public string SceneToLoad = "";

    public void ClickButton()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneToLoad);
    }
}
