using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitingToStartWindow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        inputController.GetInstance().onStart += Bird_onStart;
    }

    private void Bird_onStart(object sender, System.EventArgs e)
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
}
