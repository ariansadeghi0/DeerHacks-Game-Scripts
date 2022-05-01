using UnityEngine;

public class followScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerManager.Instance.PlayerPosition;
    }
}
