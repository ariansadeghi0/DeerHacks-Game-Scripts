using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;


    public Vector2 PlayerPosition { get; private set; }
    public Vector2 PlayerVelocity { get; private set; }
    public float PlayerSpeed { get; private set; }

    private Vector2 InitialPlayerPosition;
    private Vector2 lastFramePos;
    private int collisionCount = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.GetLevelValues();
        InitialPlayerPosition = transform.position;

        GameManager.Instance.ResetGame += GameReset;
    }

    void Update()
    {
        //Calculating object velocity and speed
        PlayerPosition = transform.position;
        PlayerVelocity = (PlayerPosition - lastFramePos) / Time.deltaTime;
        PlayerSpeed = PlayerVelocity.magnitude;
        lastFramePos = PlayerPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Line"))
        {
            collisionCount += 1;
            other.GetComponent<LineScript>().LightUp();
        }
        else if (other.CompareTag("Danger") && GameManager.Instance.GameState == "InPlay")
        {
            GameManager.Instance.GameEnded(false);
        }
        else if (other.CompareTag("Finish") && GameManager.Instance.GameState == "InPlay")
        {
            GameManager.Instance.GameEnded(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Line"))
        {
            collisionCount -= 1;
            
            if (collisionCount == 0 && GameManager.Instance.GameState == "InPlay")
            {
                Debug.Log("out");
                GameManager.Instance.GameEnded(false);
            }
        }
    }
    private void GameReset()
    {
        transform.position = InitialPlayerPosition;
    }
}
