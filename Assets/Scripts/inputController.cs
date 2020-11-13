using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputController : MonoBehaviour
{
    private const float JUMP_amount = 90f;

    static inputController instance;

    public static inputController GetInstance()
    {
        return instance;
    }

    enum State
    {
        waitingToStart,
        Playing,
        Dead,
    }
    State state;

    public event EventHandler onDied;
    public event EventHandler onStart;
    private Rigidbody2D rigidbody2D;

    void Awake()
    {
        instance = this;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.waitingToStart;
    }

    void Update()
    {
        switch (state)
        {
            case State.waitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (onStart != null) onStart(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Jump();
                }

                transform.eulerAngles = new Vector3(0, 0, rigidbody2D.velocity.y * 0.2f);
                break;
            case State.Dead:
                break;
        }
    }

    private void Jump()
    {
        rigidbody2D.velocity = Vector2.up * JUMP_amount;
        SoundManager.playSound(SoundManager.Sounds.BirdJump);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.Dead;
        if (onDied != null) onDied(this, EventArgs.Empty);
        SoundManager.playSound(SoundManager.Sounds.Lose);
    }
}
