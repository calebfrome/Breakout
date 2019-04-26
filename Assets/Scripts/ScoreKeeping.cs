using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreKeeping : MonoBehaviour
{
    static Dictionary<int, int> bricksRemaining;
    int score = 0;
    static int baseScoreBoost = 0;  // score increase for a given frame, before bonuses
    static double bonus = 1.0;  // bonus multiplier
    int numPaddleHits = 0;  // total number of times the ball hit the paddle
    static int consecutiveBricksHit = 0;  // number of bricks hit since last paddle hit
    static int consecutiveBricksBroken = 0;  // number of bricks broken since last paddle hit
    // breaking all bricks of each type triggers an increased bonus for some number of frames
    // this dict stores the number of frames remaining for each brick type bonus
    static Dictionary<int, int> brickTypeBonusFrames;
    public Text scoreText;
    static string msg = "";
    static int messageFrames = -1;
    int lives = 0;
    bool gameActive = false;
    public Text messageText;
    public Text livesText;
    public GameObject gameOver;
    public GameObject youWon;
    public GameObject gameScore;
    public GameObject apertureLight;
    public GameObject ball;
    public GameObject paddle;
    public GameObject roundPaddle;
    public GameObject stonePaddle1;
    public GameObject stonePaddle2;
    public GameObject curvedPaddleSphere;
    public GameObject[] curvedPaddleOther;
    Vector3 paddlePos;
    Vector3 roundPaddlePos;
    Vector3 stonePaddle1Pos;
    Vector3 stonePaddle2Pos;
    Vector3 curvedPaddleSpherePos;
    Vector3[] curvedPaddleOtherPos;
    public GameObject[] bricks;
    private Rigidbody rb;
    static bool brokenFlag;

    // Start is called before the first frame update
    void Start()
    {
        // get starting locations of all paddle elements
        paddlePos = paddle.transform.position;
        roundPaddlePos = roundPaddle.transform.position;
        stonePaddle1Pos = stonePaddle1.transform.position;
        stonePaddle2Pos = stonePaddle2.transform.position;
        curvedPaddleSpherePos = curvedPaddleSphere.transform.position;
        curvedPaddleOtherPos = new Vector3[curvedPaddleOther.Length];
        for (int i=0; i<curvedPaddleOther.Length; i++)
        {
            curvedPaddleOtherPos.SetValue(curvedPaddleOther[i].transform.position, i);
        }

        bricksRemaining = new Dictionary<int, int>(4);
        bricksRemaining.Add(9, 28);   // stone
        bricksRemaining.Add(10, 28);  // water
        bricksRemaining.Add(12, 7);   // metal
        bricksRemaining.Add(18, 20);  // wood

        brickTypeBonusFrames = new Dictionary<int, int>(4);
        brickTypeBonusFrames.Add(9, 0);   // stone
        brickTypeBonusFrames.Add(10, 0);  // water
        brickTypeBonusFrames.Add(12, 0);  // metal
        brickTypeBonusFrames.Add(18, 0);  // wood

        // get ball rigidbody
        rb = ball.GetComponent<Rigidbody>();

        // prep scene
        ResetScene();

        // set start message
        msg = "<spacebar> to start";

        brokenFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (messageFrames > 0) messageFrames--;
        if (messageFrames == 0) msg = "";
        messageText.text = msg;

        if (!gameActive && Input.GetKeyDown("space"))
        {
            ActivateGame();
        }

        UpdateScore();

        brokenFlag = true;
        if (gameActive)
        {
            print("broken bricks:");
            foreach(GameObject brick in bricks)
            {
                if (!brick.activeSelf) print(brick.name);
            }
            brokenFlag = false;
        }

        //check if player wins
        foreach (int val in bricksRemaining.Values)
        {
            if (val > 0) return; // game is still in progress
        }

        // player wins
        WinGame();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Paddle") ||
            collision.gameObject.name.Equals("Round Paddle") ||
            collision.gameObject.transform.parent.name.Equals("Stone Paddle") ||
            collision.gameObject.transform.parent.name.Equals("Curved Paddle"))
        {
            numPaddleHits++;
            consecutiveBricksHit = 0;
            consecutiveBricksBroken = 0;
            // clear message
            messageFrames = -1;
            msg = "";
        }

        else if (collision.gameObject.name.Equals("Deadzone")) LoseLife();
    }

    // allows the Bricks class to communicate collision information
    public static void reportCollision(int brickLayer, bool crit)
    {
        consecutiveBricksHit++;  // we hit a brick
        if (consecutiveBricksHit >= 10)
        {
            messageFrames = 100;
            msg = "Combo Bonus!";
        }
        if (crit)
        {
            consecutiveBricksBroken++;
            bricksRemaining[brickLayer]--;
            if (consecutiveBricksBroken > 10)
            {
                messageFrames = 100;
                msg = "Double Combo Bonus!";
            }

            // debug
            print(bricksRemaining[9] + " " + bricksRemaining[10] + " " + bricksRemaining[12] + " " + bricksRemaining[18]);

            // score 100 for breaking a brick
            baseScoreBoost = 100;
            
            // score 1000 for breaking all bricks of a certain type
            int bonusFrames = 100;
            if (bricksRemaining[brickLayer] == 0) baseScoreBoost += 1000;

            // water brick bonus
            if (baseScoreBoost >= 1000 && brickLayer == 10)
            {
                messageFrames = 100;
                msg = "Diver Bonus!";
            }

            // wood brick bonus
            if (baseScoreBoost >= 1000 && brickLayer == 18)
            {
                messageFrames = 100;
                msg = "Carpenter Bonus!";
            }


            // double the score for stone bricks because they require 2 hits
            if (brickLayer == 9)
            {
                baseScoreBoost *= 2;
                bonusFrames *= 2;
                if (baseScoreBoost >= 1000)
                {
                    messageFrames = 200;
                    msg = "Miner Bonus!";
                }
            }
            // triple the score for metal bricks because they require 3 hits
            else if (brickLayer == 12)
            {
                baseScoreBoost *= 3;
                bonusFrames *= 3;
                if (baseScoreBoost >= 1000)
                {
                    messageFrames = 100;
                    msg = "Blacksmith Bonus!";
                }
            }

            // add bonus frames
            brickTypeBonusFrames[brickLayer] = bonusFrames;

            print("broke a brick in layer " + brickLayer);
            brokenFlag = true;
        }
    }

    void UpdateScore()
    {
        // compute bonus multiplier
        // default is no bonus (x1.0)
        bonus = 1.0;
        // add bonus for bricks hit since last paddle hit
        bonus += (0.1 * consecutiveBricksHit);
        // add bonus for bricks broken since last paddle hit
        bonus += (0.2 * consecutiveBricksBroken);
        // add bonus for completing brick types
        foreach(int framesRemaining in brickTypeBonusFrames.Values)
        {
            bonus += Math.Min(0.5, framesRemaining / 10.0);
        }
        // adjust ball light radius based on bonus
        float bonusAsFloat = (float)bonus;
        apertureLight.transform.localScale = new Vector3(1 + bonusAsFloat/3, 1, 1 + bonusAsFloat/3);

        // update score
        score += (int)(baseScoreBoost * bonus);
        scoreText.text = "Score: " + score;
        // reset score variables
        baseScoreBoost = 0;
        // decrement bonus frames
        int[] keys = { 9, 10, 12, 18 };
        foreach (int key in keys)
        {
            if (brickTypeBonusFrames[key] > 0)
            {
                brickTypeBonusFrames[key]--;
            }
        }
    }

    void LoseLife()
    {
        lives--;
        livesText.text = "Lives: " + lives;
        // display continue msg until further notice
        messageFrames = -1;
        msg = "<spacebar> to continue";
        ResetScene();

        // lose game
        if (lives == 0)
        {
            livesText.text = "";
            gameOver.SetActive(true);
        }
    }

    void WinGame()
    {
        // add bonus for minimal paddle use
        int paddleBonus = 10000;
        int threshold = 25;
        while (numPaddleHits > threshold && paddleBonus > 0)
        {
            paddleBonus -= 1000;
            threshold += 25;
        }
        score += paddleBonus;

        // add time bonus
        int timeBonus = 10000;
        threshold = 60;
        while(Time.time > threshold && timeBonus > 0)
        {
            timeBonus -= 1000;
            threshold += 60;
        }
        score += timeBonus;

        // update UI
        scoreText.text = "Score: " + score;
        // show play again msg until further notice
        messageFrames = -1;
        msg = "<spacebar> to play again";
        youWon.SetActive(true);
        ResetScene();
    }

    // make paddles and ball stationary and placed in the bottom center
    void ResetScene()
    {
        // set game state to inactive
        gameActive = false;

        // set message
        messageFrames = -1;
        if (lives == 0) msg = "<spacebar> to play again";
        else msg = "<spacebar> to continue";

        // move ball back to center (x == 0)
        ball.transform.position = new Vector3(0f, -7.75f, 0f);

        // move paddles back to center (x == 0)
        paddle.transform.position = paddlePos;
        roundPaddle.transform.position = roundPaddlePos;
        stonePaddle1.transform.position = stonePaddle1Pos;
        stonePaddle2.transform.position = stonePaddle2Pos;
        curvedPaddleSphere.transform.position = curvedPaddleSpherePos;
        for (int i=0; i<curvedPaddleOther.Length; i++)
        {
            curvedPaddleOther[i].transform.position = curvedPaddleOtherPos[i];
        }
        
        // stop ball motion
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // set the ball in motion after losing a life / starting a new game
    void ActivateGame()
    {
        gameActive = true; 

        // reset lives, score, and bricks if new game
        if (lives == 0)
        {
            lives = 3;
            score = 0;
            // set all bricks to active
            print("set all bricks to active");
            foreach (GameObject brick in bricks)
            {
                brick.SetActive(true);
                //print(brick.name + brick.activeSelf);
            }
        }

        // update UI
        livesText.text = "Lives: " + lives;
        msg = "";
        gameOver.SetActive(false);
        youWon.SetActive(false);
        gameScore.SetActive(true);

        // set ball in motion
        rb.velocity = new Vector3(1.0f * 10, 5.0f, 0.0f);
    }
}
