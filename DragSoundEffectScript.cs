using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]

public class DragSoundEffectScript : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxPitch;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();//Getting audiosource component
    }

    private void Update()
    {
        //Calculating and updating the pitch of the audio source

        float speedToMaxSpeedRatio;
        float adjustedPitch;//The new pitch value

        speedToMaxSpeedRatio = Mathf.Clamp(PlayerManager.Instance.PlayerSpeed / maxSpeed, 0f, maxSpeed/*This should be 1f, however the audio sounds better like this*/);
        adjustedPitch = maxPitch * speedToMaxSpeedRatio;

        audioSource.pitch = adjustedPitch;//Setting the new pitch
    }
}
