using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class score
{
    public static void Start()
    {
        inputController.GetInstance().onDied += Bird_onDied;
    }

    private static void Bird_onDied(object sender, System.EventArgs e)
    {
        updateHighscore(Level.GetInstance().getPipesPassed());
    }

    public static int getHighscore()
    {
        return PlayerPrefs.GetInt("highscore");
    }

    public static bool updateHighscore(int score)
    {
        int currHighscore = getHighscore();
        if (score > currHighscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }
}
