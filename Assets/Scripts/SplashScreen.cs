using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5)
            VideoFinished();
    }

    private void VideoFinished()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }
}
