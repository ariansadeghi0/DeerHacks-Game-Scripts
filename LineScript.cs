using UnityEngine;

public class LineScript : MonoBehaviour
{
    public Color initialColor;
    public Color triggeredColor;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    void Start()
    {
        spriteRenderer.color = initialColor;
        GameManager.Instance.ResetGame += LightReset;
    }

    public void LightUp()
    {
        spriteRenderer.color = triggeredColor;
    }

    private void LightReset()
    {
        spriteRenderer.color = initialColor;
    }
}
