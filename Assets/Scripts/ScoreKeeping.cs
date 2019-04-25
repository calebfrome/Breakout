using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeping : MonoBehaviour
{
    static Dictionary<int, int> bricksRemaining;
    int lives = 3;
    int score = 0;
    static int baseScoreBoost = 0;  // score increase for a given frame, before bonuses
    double bonus = 1.0;  // bonus multiplier
    int numPaddleHits = 0;  // total number of times the ball hit the paddle
    static int consecutiveBricksHit = 0;  // number of bricks hit since last paddle hit
    static int consecutiveBricksBroken = 0;  // number of bricks broken since last paddle hit
    // breaking all bricks of each type triggers an increased bonus for some number of frames
    // this dict stores the number of frames remaining for each brick type bonus
    static Dictionary<int, int> brickTypeBonusFrames;
    public Text livesText;
    public Text scoreText;
    public GameObject gameOver;
    public GameObject youWon;
    public GameObject apertureLight;

    // Start is called before the first frame update
    void Start()
    {
        bricksRemaining = new Dictionary<int, int>(4);
        bricksRemaining.Add(9, 28);   // stone
        bricksRemaining.Add(10, 28);  // light blue
        bricksRemaining.Add(12, 7);   // metal
        bricksRemaining.Add(18, 20);  // pink

        brickTypeBonusFrames = new Dictionary<int, int>(4);
        brickTypeBonusFrames.Add(9, 0);   // stone
        brickTypeBonusFrames.Add(10, 0);  // light blue
        brickTypeBonusFrames.Add(12, 0);  // metal
        brickTypeBonusFrames.Add(18, 0);  // pink
    }

    // Update is called once per frame
    void Update()
    {
        switch(CheckGameOver())
        {
            case -1:  // player loses
                EndGame(false);
                break;
            case 1:  // player wins
                EndGame(true);
                break;
            default:  // game is still in progress
                UpdateScore();
                break;
        }
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
        }

        else if (collision.gameObject.name.Equals("Deadzone"))
        {
            lives--;
            livesText.text = "Lives: " + lives;
        }

        print(collision.gameObject.name);
    }

    // allows the Bricks class to communicate collision information
    public static void reportCollision(int brickLayer, bool crit)
    {
        consecutiveBricksHit++;
        if (crit)
        {
            consecutiveBricksBroken++;
            bricksRemaining[brickLayer]--;
            print(bricksRemaining[9] + " " + bricksRemaining[10] + " " + bricksRemaining[12] + " " + bricksRemaining[18]);

            // score 100 for breaking a brick
            baseScoreBoost = 100;
            
            // score 1000 for breaking all bricks of a certain type
            int bonusFrames = 100;
            if (bricksRemaining[brickLayer] == 0) baseScoreBoost += 1000;

            // double the score for stone bricks because they require 2 hits
            if (brickLayer == 10)
            {
                baseScoreBoost *= 2;
                bonusFrames *= 2;
            }
            // triple the score for metal bricks because they require 3 hits
            else if (brickLayer == 12)
            {
                baseScoreBoost *= 3;
                bonusFrames *= 3;
            }

            // add bonus frames
            brickTypeBonusFrames[brickLayer] = bonusFrames;
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
            if (framesRemaining > 0) bonus += 0.5;
        }
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

    int CheckGameOver()
    {
        if (lives == 0) return -1;  // player lost
        foreach(int val in bricksRemaining.Values)
        {
            if (val > 0) return 0; // game is still in progress
        }
        return 1;  // player won
    }

    void EndGame(bool win)
    {
        Destroy(gameObject);
        livesText.text = "";
        if (win)
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

            // update score
            scoreText.text = "Score: " + score;

            youWon.SetActive(true);
        }
        else gameOver.SetActive(true);
    }
}
