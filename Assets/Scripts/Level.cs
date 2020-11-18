using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    const float CAMERA_ORTHO_SIZE = 50F;
    const float PIPE_WIDTH = 7.8F;
    const float PIPE_HEAD_HEIGHT = 3.75F;
    const float PIPE_MOVE_SPEED = 30f;

    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }

    private List<Pipe> pipesList;
    private List<Transform> groundList;
    private List<Transform> cloudList;
    float cloudSpawnTimer;
    int pipesSpawned;
    int pipesPassed;
    float pipeSpawnTimer;
    float pipeSpawnTimerMax;
    float gapSize;

    public enum difficulty
    {
        easy,
        medium,
        hard,
        impossible,
    }

    enum State
    {
        waitingToStart,
        playing,
        dead,
    }
    private State state;

    private void Awake()
    {
        instance = this;
        pipesList = new List<Pipe>();
        spawnInitialGround();
        spawnInitialClouds();
        pipeSpawnTimerMax = 1f;
        setDifficulty(difficulty.easy);
        state = State.waitingToStart;
    }

    private void Start()
    {
        inputController.GetInstance().onDied += Bird_onDied;
        inputController.GetInstance().onStart += Bird_onStart;
    }

    private void Bird_onStart(object sender, EventArgs e)
    {
        state = State.playing;
    }

    private void Bird_onDied(object sender, EventArgs e)
    {
        state = State.dead;
        //Invoke("reloadScene", 1f);
    }

    void reloadScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void Update()
    {
        if (state == State.playing)
        {
            movePipes();
            spawnPipes();
            handleGround();
            handleClouds();
        }
    }

    Transform getCloudPrefab()
    {
        switch(UnityEngine.Random.Range(0, 3))
        {
            default:
            case 0: return GameAssets.GetInstance().pCloud_1;
            case 1: return GameAssets.GetInstance().pCloud_2;
            case 2: return GameAssets.GetInstance().pCloud_3;
        }
    }

    private void handleClouds()
    {
        cloudSpawnTimer -= Time.deltaTime;
        if (cloudSpawnTimer < 0)
        {
            float cloudSpawnTimerMax = 6f;
            cloudSpawnTimer = cloudSpawnTimerMax;
            Transform cloud = Instantiate(getCloudPrefab(), new Vector3(185f, 35f, 0f), Quaternion.identity);
            cloudList.Add(cloud);
        }

        for (int i = 0; i < cloudList.Count; i++)
        {
            Transform cloud = cloudList[i];
            cloud.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime * 0.7f;

            if (cloud.position.x < -185f)
            {
                Destroy(cloud.gameObject);
                cloudList.RemoveAt(i);
                i--;
            }
        }
    }

    void spawnInitialClouds()
    {
        cloudList = new List<Transform>();
        Transform cloud;
        cloud = Instantiate(getCloudPrefab(), new Vector3(0f, 35f, 0f), Quaternion.identity);
        cloudList.Add(cloud);
    }

    void spawnInitialGround()
    {
        groundList = new List<Transform>();
        Transform ground;
        ground = Instantiate(GameAssets.GetInstance().pGround, new Vector3(0f, -50f, 0f), Quaternion.identity);
        groundList.Add(ground);
        ground = Instantiate(GameAssets.GetInstance().pGround, new Vector3(240f, -50f, 0f), Quaternion.identity);
        groundList.Add(ground);
        ground = Instantiate(GameAssets.GetInstance().pGround, new Vector3(480f, -50f, 0f), Quaternion.identity);
        groundList.Add(ground);
    }

    private void handleGround()
    {
        foreach (Transform ground in groundList)
        {
            ground.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;

            if (ground.position.x < -240f)
            {
                ground.position = new Vector3(480f, ground.position.y, ground.position.z);
            }
        }
    }

    public int getPipesSpawned()
    {
        return pipesSpawned;
    }

    public int getPipesPassed()
    {
        return pipesPassed;
    }

    private void spawnPipes()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0)
        {
            pipeSpawnTimer += pipeSpawnTimerMax;

            float heightMargin = 10f;
            float minHeight = gapSize * 0.5f + heightMargin;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * 0.5f - heightMargin;

            float height = UnityEngine.Random.Range(minHeight, maxHeight);
            createGappedPipes(height, gapSize, 125f);
        }
    }

    private void movePipes()
    {
        for(int i = 0; i < pipesList.Count; i++)
        {
            Pipe pipe = pipesList[i];
            bool isToRightOfBird = pipe.getXposition() > 0f;

            pipe.Move();

            if (isToRightOfBird && pipe.getXposition() <= 0 && pipe.IsBottom())
            {
                pipesPassed++;
                SoundManager.playSound(SoundManager.Sounds.Score);
            }

            if (pipe.getXposition() < -125f)
            {
                pipe.destroySelf();
                pipesList.Remove(pipe);
                i--;
            }
        }
    }

    void setDifficulty(difficulty diff)
    {
        switch (diff)
        {
            case difficulty.easy:
                gapSize = 50f;
                pipeSpawnTimerMax = 1.4f;
                break;
            case difficulty.medium:
                gapSize = 40f;
                pipeSpawnTimerMax = 1.3f;
                break;
            case difficulty.hard:
                gapSize = 33f;
                pipeSpawnTimerMax = 1.1f;
                break;
            case difficulty.impossible:
                gapSize = 24f;
                pipeSpawnTimerMax = 1.0f;
                break;
        }
    }

    difficulty getDifficulty()
    {
        if (pipesSpawned >= 30) return difficulty.impossible;
        if (pipesSpawned >= 20) return difficulty.hard;
        if (pipesSpawned >= 10) return difficulty.medium;
        return difficulty.easy;
    }

    void createGappedPipes(float gapY, float gapSize, float xPosition)
    {
        createPipe(gapY - gapSize * 0.5f, xPosition, true);
        createPipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * 0.5f, xPosition, false);
        pipesSpawned++;
        setDifficulty(getDifficulty());
    }

    private void createPipe(float height, float xPosition, bool createBottom)
    {
        // Pipe Head setup
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pPipeHead);
        float pipeHeadYposi;
        if (createBottom)
        {
            pipeHeadYposi = -CAMERA_ORTHO_SIZE + height - (PIPE_HEAD_HEIGHT / 2f);
        }
        else
        {
            pipeHeadYposi = +CAMERA_ORTHO_SIZE - height + (PIPE_HEAD_HEIGHT / 2f);
        }
        pipeHead.position = new Vector2(xPosition, pipeHeadYposi);

        // Pipe Body setup
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pPipeBody);
        float pipeBodyYposi;
        if (createBottom)
        {
            pipeBodyYposi = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeBodyYposi = CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1f, -1f, 1f);
        }
        pipeBody.position = new Vector2(xPosition, pipeBodyYposi);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);
        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * 0.5f);

        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        pipesList.Add(pipe);
    }

    class Pipe
    {
        Transform pipeHeadTransform;
        Transform pipeBodyTransform;
        bool isBottom;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }

        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float getXposition()
        {
            return pipeHeadTransform.position.x;
        }

        public bool IsBottom()
        {
            return isBottom;
        }

        public void destroySelf()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }
    }
}
