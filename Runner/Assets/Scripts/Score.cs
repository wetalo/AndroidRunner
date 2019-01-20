using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct LevelScore
{
    public int level;
    public int scoreRequired;
}

public class Score : MonoBehaviour {

    public float score = 0.0f;
    public Text scoreText;
    
    private int difficultyLevel = 1;
    public int maxDifficultyLevel = 10;
    private int scoreToNextLevel = 10;

    private bool isDead = false;

    [SerializeField]
    List<LevelScore> levelScores;

	// Use this for initialization
	void Start () {
        	
	}
	
	// Update is called once per frame
	void Update () {

        if (isDead)
        {
            return;
        }

        if(score >= scoreToNextLevel)
        {
            LevelUp();
        }

        score += Time.deltaTime * difficultyLevel;
        scoreText.text = "Cash: " + ((int)score).ToString() + "$";
	}

    void LevelUp()
    {
        if(difficultyLevel >= maxDifficultyLevel)
        {
            return;
        }
        //scoreToNextLevel *= 2;
        difficultyLevel++;
        scoreToNextLevel = GetNextLevelScore();
        GetComponent<PlayerMotor>().SetSpeed(difficultyLevel * 2);
    }

    public void OnDeath()
    {
        isDead = true;
    }

    int GetNextLevelScore()
    {
        foreach(LevelScore levelScore in levelScores)
        {
            if(levelScore.level == difficultyLevel)
            {
                return levelScore.scoreRequired;
            }
        }

        return 0;
    }

    public void GetPoints(float pointsToGet)
    {
        score += pointsToGet;
    }
}
