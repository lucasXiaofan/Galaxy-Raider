
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    public void restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void QuitGame()
    {
        print("quitgameSuccess");
        Application.Quit();
    }
}
