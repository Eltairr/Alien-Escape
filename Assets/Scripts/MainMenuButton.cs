using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void GoToMainMenu()
    {
        //Stop the music before going to the Main Menu
        if (BackgroundMusicManager.instance != null)
        {
            BackgroundMusicManager.instance.StopMusic();
        }

        SceneManager.LoadScene("MainMenu");
    }
}
