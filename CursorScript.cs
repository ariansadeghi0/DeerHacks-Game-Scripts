using UnityEngine;

public class CursorScript : MonoBehaviour
{
    private void Awake()
    {
        //Hiding actual cursor
        Cursor.visible = false;
    }
    void Update()
    {
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = newPosition;
    }
}
