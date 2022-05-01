using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class PostProcessEffectsScript : MonoBehaviour
{
    [SerializeField] private float minBloomIntensity;
    [SerializeField] private float maxBloomIntensity;
    [SerializeField] private float maxSpeed;

    private Volume volume;
    private Bloom bloom;

    private void Awake()
    {
        volume = GetComponent<Volume>();

        volume.profile.TryGet<Bloom>(out bloom);
    }

    private void Update()
    {
        //Calculating and updating the bloom intensity based on the object's speed

        float speedToMaxSpeedRatio;
        float adjustedBloomIntensity;

        speedToMaxSpeedRatio = Mathf.Clamp(PlayerManager.Instance.PlayerSpeed / maxSpeed, 0f, 1f);
        adjustedBloomIntensity = minBloomIntensity + (maxBloomIntensity - minBloomIntensity) * speedToMaxSpeedRatio;

        bloom.intensity.value = adjustedBloomIntensity;
    }
}
