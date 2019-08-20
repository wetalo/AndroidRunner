using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool isPaused;
    public bool isMuted = true;

    public GameObject pauseMenu;
    public List<HighScore> highScores;

    public int maxNumHighScores;
        

	// Use this for initialization
	void Start () {
        Time.timeScale = 1.0f;
        isPaused = false;
        LoadHighScores();
        DisplayHighScoresOnUI();
        pauseMenu.SetActive(false);
        //Screen.SetResolution(720, 1080, true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PauseButton()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        } else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    public void MuteButton()
    {
        isMuted = !isMuted;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    void LoadHighScores()
    {
        HighScore[] highScoreArray = SaveSystem.LoadHighScores();

        highScores = new List<HighScore>();
        if (highScoreArray != null)
        {
            foreach(HighScore score in highScoreArray)
            {
                highScores.Add(score);
            }
        }
        SortHighScores();

    }

    void SaveHighScores()
    {
        HighScore[] highScoreArray = highScores.ToArray();
        SaveSystem.SaveHighScores(highScoreArray);
    }

    public void SubmitNewHighScore(int score)
    {
        HighScore newScore = new HighScore(score);
        highScores.Add(newScore);
        SortHighScores();
        int currentMax = maxNumHighScores;
        if(maxNumHighScores > highScores.Count)
        {
            currentMax = highScores.Count;
        }
        highScores = highScores.GetRange(0, currentMax);
        SaveHighScores();
        DisplayHighScoresOnUI();
    }

    void DisplayHighScoresOnUI()
    {
        string highscoreString = "";
        foreach (HighScore highscore in highScores)
        {
            highscoreString += highscore.score + " -- " + highscore.date + "\n";
        }
        GameObject.Find("ScoreText").GetComponent<Text>().text = highscoreString;
    }

    void SortHighScores()
    {
        highScores.Sort(
            delegate (HighScore score1, HighScore score2)
            {
                return score1.score.CompareTo(score2.score);
            }
        );
        highScores.Reverse();
    }
}
