using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOverWindow : MonoBehaviour
{
    public Text scoreText;
    public Text highscoreText;

    void Start()
    {
        Hide();
        inputController.GetInstance().onDied += Bird_onDied;
    }

    private void Bird_onDied(object sender, System.EventArgs e)
    {
        Show();
        scoreText.text = Level.GetInstance().getPipesPassed().ToString();

        if (Level.GetInstance().getPipesPassed() > score.getHighscore())
        {
            highscoreText.text = "NEW HIGHSCORE!";
        }
        else
        {
            highscoreText.text = "HIGHSCORE: " + score.getHighscore().ToString();
        }
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
    void Show()
    {
        gameObject.SetActive(true);
    }

    public void onRetryClicked()
    {
        SoundManager.playSound(SoundManager.Sounds.ButtonClick);
        Loader.Load(Loader.Scene.GameScene);
    }

    public void onMainmenuClicked()
    {
        SoundManager.playSound(SoundManager.Sounds.ButtonClick);
        Loader.Load(Loader.Scene.MainMenu);
    }
}
