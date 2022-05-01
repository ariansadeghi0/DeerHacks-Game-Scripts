using UnityEngine;


[RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
public class BackgroundFlashEffectScript : MonoBehaviour
{
    [SerializeField] private float minLightIntensity;
    [SerializeField] private float maxLightIntensity;
    [SerializeField] private float maxSpeed;

    private UnityEngine.Rendering.Universal.Light2D backgroundLight;

    private void Awake()
    {
        backgroundLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    void Update()
    {
        //Calculating and updating the light intensity based on the object's speed

        float speedToMaxSpeedRatio;
        float adjustedLightIntensity;

        speedToMaxSpeedRatio = Mathf.Clamp(PlayerManager.Instance.PlayerSpeed / maxSpeed, 0f, 1f);
        adjustedLightIntensity = minLightIntensity + (maxLightIntensity - minLightIntensity) * speedToMaxSpeedRatio;

        backgroundLight.intensity = adjustedLightIntensity;
    }
}
