using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField][Range(0, 30)] public float initialScore = 10;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    public event Action StartGame;
    public event Action<bool, bool> EndGame; // bools for whether game was finished successfully, and if new highscore was set
    public event Action ResetGame;

    public string GameState { get; set; } = "Idle"; // "Idle" or "InPlay" or "ResultsIdle"
    public float Score { get; set; }    //The score
    public float Highscore { get; set; }    //The highscore
    public float OldHighscore { get; set; } //The previous highscore
    public int Attempts { get; set; } //Number of attempts for this level
    public int Finishes { get; set; } //Number of finishes for this level
    public float FinishScoresSum { get; set; }    //The sum of all finish scores, used with finishes to calculate avg finish score
    public float OldFinishScoresSum { get; set; }
    public float SessionFinishScoresSum { get; set; }
    public int SessionFinishes { get; set; }

    void Awake()
    {
        _instance = this;
        Score = initialScore;
    }
    private void Update()
    {
        if (GameState == "InPlay")
        {
            Score -= Time.deltaTime;
            if (Score <= 0)
            {
                Score = 0;
                GameEnded(false);
            }
        }
    }
    public void GameStarted()
    {
        GameState = "InPlay";
        StartGame?.Invoke();
    }
    public void GameEnded(bool finished)
    {
        if (finished)
        {
            //Adjusting values
            Attempts += 1;
            Finishes += 1;
            OldFinishScoresSum = FinishScoresSum;
            FinishScoresSum += Score;
            SessionFinishScoresSum += Score;
            SessionFinishes += 1;

            if (Score > Highscore)
            {
                //Setting new highscore
                OldHighscore = Highscore;
                Highscore = Score;
                EndGame?.Invoke(finished, true);
            }
        }
        else
        {
            Attempts += 1;
        }
        TakeScreenshot();
        GameState = "ResultsIdle";
        EndGame?.Invoke(finished, false);
    }
    public void ResetValues()
    {
        GameState = "Idle";
        Score = initialScore;
        ResetGame?.Invoke();
    }
    public void GetLevelValues()
    {
        //Getting values
        OldHighscore = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "OldHighscore", 0f);
        Highscore = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "Highscore", 0f);
        Attempts = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "Attempts", 0);
        Finishes = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "Finishes", 0);
        FinishScoresSum = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "FinishScoresSum", 0f);
        OldFinishScoresSum = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "OldFinishScoresSum", 0f);
        SessionFinishScoresSum = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "SessionFinishScoresSum", 0f);
        SessionFinishes = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "SessionFinishes", 0);
    }

    public void SetLevelValues()
    {
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "OldHighscore", OldHighscore);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "Highscore", Highscore);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Attempts", Attempts);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "Finishes", Finishes);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "FinishScoresSum", FinishScoresSum);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "OldFinishScoresSum", OldFinishScoresSum);
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "SessionFinishScoresSum", SessionFinishScoresSum);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "SessionFinishes", SessionFinishes);

        PlayerPrefs.Save();
    }
    public void LoadScene(string scene)
    {
        //Deleting some PlayerPref values in prep for new session
        PlayerPrefs.DeleteKey(scene + "SessionFinishScoresSum");
        PlayerPrefs.DeleteKey(scene + "SessionFinishes");

        SceneManager.LoadScene(scene);
    }
    private void TakeScreenshot()
    {
        try
        {
            GameObject camera = GameObject.Find("ScreenshotCamera");
            camera.GetComponent<ScreenshotScript>().TakeScreenshot();
        }
        catch
        {
            Debug.Log("Screenshot failed");
        }
    }
}
