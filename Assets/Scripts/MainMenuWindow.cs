using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuWindow : MonoBehaviour
{
    private void Awake()
    {
        
    }

    public void Play()
    {
        SoundManager.playSound(SoundManager.Sounds.ButtonClick);
        Loader.Load(Loader.Scene.GameScene);
    }

    public void Quit()
    {
        SoundManager.playSound(SoundManager.Sounds.ButtonClick);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
