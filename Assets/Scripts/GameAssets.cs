using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public Sprite pipeHeadSprite;
    public Transform pPipeHead;
    public Transform pPipeBody;
    public Transform pGround;
    public Transform pCloud_1;
    public Transform pCloud_2;
    public Transform pCloud_3;

    public soundClip[] soundClips;

    [Serializable]
    public class soundClip
    {
        public SoundManager.Sounds sound;
        public AudioClip audioClip;
    }
}
