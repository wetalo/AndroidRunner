using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScore
{

    public int score;
    public string date;

    //For loading an existing High Score
    public HighScore(int score, string date)
    {
        this.score = score;
        this.date = date;
    }

    //For creating a new High Score
    public HighScore(int score)
    {
        this.score = score;
        this.date = DateTime.Now.ToString();
    }
    
}
