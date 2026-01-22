using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnButtonLoadLevel1()
    {
        SceneManager.LoadScene("level1");
    }

    public void OnButtonLoadLevel2()
    {
        SceneManager.LoadScene("level2");
    }

    public void OnButtonLoadLevel3()
    {
        SceneManager.LoadScene("level3");
    }

        public void OnButtonLoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

            public void OnButtonLoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
