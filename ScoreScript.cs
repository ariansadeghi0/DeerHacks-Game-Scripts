using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    private TextMeshProUGUI ScoreText;

    private void Awake()
    {
        ScoreText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        
        ScoreText.text = GameManager.Instance.Score.ToString("F2");

    }
}
