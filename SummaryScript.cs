using UnityEngine;
using TMPro;

public class SummaryScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI HeaderText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI AttemptsText;
    [SerializeField] private TextMeshProUGUI FinishesText;
    [SerializeField] private TextMeshProUGUI HighscoreText;
    [SerializeField] private TextMeshProUGUI HighscoreDiffText;
    [SerializeField] private TextMeshProUGUI AvgFinishScoreText;
    [SerializeField] private TextMeshProUGUI AvgDiffText;
    [SerializeField] private TextMeshProUGUI SessAvgFinishScoreText;
    [SerializeField] private TextMeshProUGUI SessAvgDiffText;
    [SerializeField] private TextMeshProUGUI FinishPercentageText;

    [Header("Colors")]
    [SerializeField] private Color32 greenColor;
    [SerializeField] private Color32 redColor;

    private RectTransform rt;
    private Vector2 initialPos;

    float _AvgFinishScore;
    decimal AvgFinishScore;
    float _AvgDiff;
    decimal AvgDiff;
    float _SessAvgFinishScore;
    decimal SessAvgFinishScore;
    float _SessAvgDiff;
    decimal SessAvgDiff;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        initialPos = rt.position;
        GameManager.Instance.EndGame += Activate;
        GameManager.Instance.ResetGame += UnActivate;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.GameState == "ResultsIdle")
        {
            GameManager.Instance.ResetValues();
        }
    }
    private void Activate(bool finished, bool setHighscore)
    {
        if (finished)
        {
            ScoreText.text = GameManager.Instance.Score.ToString("F2");
        }
        else
        {
            ScoreText.text = "DNF";
        }
        UpdateSummary(finished, setHighscore);
        rt.position = new Vector2(0, 0);
    }
    private void UnActivate()
    {
        rt.position = initialPos;
    }
    private void UpdateSummary(bool finished, bool setHighscore)
    {
        if (GameManager.Instance.FinishScoresSum != 0 && GameManager.Instance.Finishes != 0 && GameManager.Instance.OldFinishScoresSum != 0)
        {
            //Calculating AvgFinishScore stuff
            _AvgFinishScore = GameManager.Instance.FinishScoresSum / (float)GameManager.Instance.Finishes;
            AvgFinishScore = decimal.Round((decimal)_AvgFinishScore, 2);
            _AvgDiff = (float)AvgFinishScore - GameManager.Instance.OldFinishScoresSum / ((float)GameManager.Instance.Finishes - 1);
            AvgDiff = decimal.Round((decimal)_AvgDiff, 2);
        }
        else if (finished)
        {
            AvgFinishScore = decimal.Round((decimal)GameManager.Instance.Score, 2);
        }

        //Calculating SessAvgFinishScore stuff
        if (GameManager.Instance.SessionFinishScoresSum != 0 && GameManager.Instance.SessionFinishes != 0)
        {
            _SessAvgFinishScore = GameManager.Instance.SessionFinishScoresSum / (float)GameManager.Instance.SessionFinishes;
            SessAvgFinishScore = decimal.Round((decimal)_SessAvgFinishScore, 2);
            _SessAvgDiff = ((float)SessAvgFinishScore - (float)AvgFinishScore) / (float)AvgFinishScore * 100;   //Percent difference
            SessAvgDiff = decimal.Round((decimal)_SessAvgDiff, 3);
        }

        if (finished)
        {
            if (AvgDiff >= 0)
            {
                //Diff was positive
                AvgDiffText.color = greenColor;
                AvgDiffText.text = "+" + AvgDiff.ToString("F2");
            }
            else
            {
                //Diff was negative
                AvgDiffText.color = redColor;
                AvgDiffText.text = AvgDiff.ToString("F2");
            }

            if (SessAvgDiff >= 0)
            {
                //Diff was positive
                SessAvgDiffText.color = greenColor;
                SessAvgDiffText.text = "+" + SessAvgDiff.ToString("F3") + "%";
            }
            else
            {
                //Diff was negative
                SessAvgDiffText.color = redColor;
                SessAvgDiffText.text = SessAvgDiff.ToString("F3") + "%";
            }
        }
        else
        {
            AvgDiffText.text = "";
            SessAvgDiffText.text = "";
        }

        //Updating summary text values
        AttemptsText.text = GameManager.Instance.Attempts.ToString();
        FinishesText.text = GameManager.Instance.Finishes.ToString();
        FinishPercentageText.text = decimal.Round((decimal)GameManager.Instance.Finishes / (decimal)GameManager.Instance.Attempts * 100, 2).ToString("F1") + "%";
        HighscoreText.text = GameManager.Instance.Highscore.ToString("F2");
        AvgFinishScoreText.text = AvgFinishScore.ToString("F2");
        SessAvgFinishScoreText.text = SessAvgFinishScore.ToString("F2");

        if (setHighscore)   //If new highscore was set
        {
            HighscoreDiffText.color = greenColor;   //Giving color
            HighscoreDiffText.text = "+" + (GameManager.Instance.Highscore - GameManager.Instance.OldHighscore).ToString("F2");
            HeaderText.text = "NEW HIGHSCORE";
        }
        else    //If new highscore wasn't set
        {
            HighscoreDiffText.text = "";
            HeaderText.text = "SCORE";
        }

        //Saving new values to playerprefs
        GameManager.Instance.SetLevelValues();
    }
}
