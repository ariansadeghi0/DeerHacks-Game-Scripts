using UnityEngine;

public class GrabScript : MonoBehaviour
{
    [SerializeField] private bool grabAllowed = true;

    private bool isBeingHeld = false;
    private float holdOffsetX, holdOffsetY;

    private void Start()
    {
        GameManager.Instance.EndGame += GameEnded;
        GameManager.Instance.ResetGame += GameReset;
    }
    private void Update()
    {
        if (isBeingHeld)//Checking if object is being held
        {
            Vector2 currentMousePos;
            currentMousePos = Input.mousePosition;
            currentMousePos = Camera.main.ScreenToWorldPoint(currentMousePos);

            //Moving object
            gameObject.transform.localPosition = new Vector2(currentMousePos.x - holdOffsetX, currentMousePos.y - holdOffsetY);
        }
    }

    private void OnMouseDown()
    {
        if (grabAllowed)
        {
            //If left clicked
            if (Input.GetMouseButtonDown(0))
            {
                HoldObject();
            }
        }
    }
    private void OnMouseOver()
    {
        if (grabAllowed)
        {
            //If object is grabbed using a key
            if (Input.GetKeyDown(KeyCode.A))
            {
                HoldObject();
            }
            else if (Input.GetKeyUp(KeyCode.A))//If object is released
            {
                isBeingHeld = false;
            }
        }
    }
    private void OnMouseUp()
    {
        if (grabAllowed)
        {
            isBeingHeld = false;
        }
    }

    private void HoldObject()
    {
        Vector2 initialMousePos;
        initialMousePos = Input.mousePosition;//Getting mouse position
        initialMousePos = Camera.main.ScreenToWorldPoint(initialMousePos);//converting position to world position

        //Getting values for start position
        holdOffsetX = initialMousePos.x - transform.localPosition.x;
        holdOffsetY = initialMousePos.y - transform.localPosition.y;

        isBeingHeld = true; //Is being held

        if (GameManager.Instance.GameState != "InPlay")
        {
            GameManager.Instance.GameStarted();
        }
    }
    private void GameEnded(bool finished, bool setHighscore)
    {
        isBeingHeld = false;
        grabAllowed = false;
    }
    private void GameReset()
    {
        grabAllowed = true;
    }
}
