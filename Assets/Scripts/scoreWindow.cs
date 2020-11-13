using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreWindow : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;

    private void Start()
    {
        highScoreText.text = "HIGHSCORE: " + score.getHighscore().ToString();
        inputController.GetInstance().onDied += Bird_onDied;
        inputController.GetInstance().onStart += Bird_onStart;
        Hide();
    }

    private void Bird_onStart(object sender, EventArgs e)
    {
        Show();
    }

    private void Bird_onDied(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    void Show()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        scoreText.text = Level.GetInstance().getPipesPassed().ToString();
    }
}
