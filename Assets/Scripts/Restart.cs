using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void RestartLevel()
    {
        MobStatusUpdater mobStatusUpdater = FindObjectOfType<MobStatusUpdater>();
        if (mobStatusUpdater != null)
        {
            mobStatusUpdater.ResetMobCounter();
        }

        GameTimer gameTimer = FindObjectOfType<GameTimer>();
        if (gameTimer != null)
        {
            gameTimer.RestartTimer();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}