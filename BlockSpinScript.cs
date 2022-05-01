using UnityEngine;

public class BlockSpinScript : MonoBehaviour
{
    [SerializeField] private bool spinEnabled;
    [SerializeField] private bool clockwiseSpin;
    [SerializeField] private float speedFactor;

    private float initialRotation;
    private float rotation;

    void Start()
    {
        if (!spinEnabled)//Destroying this script if it's functionality is not enabled
        {
            Destroy(this);
            return;
        }

        initialRotation = transform.eulerAngles.z;

        ResetToInitial();
    }

    void Update()
    {
        if (spinEnabled)
        {
            rotation = transform.rotation.eulerAngles.z;
            if (clockwiseSpin)
            {
                transform.eulerAngles = new Vector3(0, 0, rotation - speedFactor * Time.deltaTime);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, rotation + speedFactor * Time.deltaTime);
            }
        }
    }

    private void ResetToInitial()
    {
        transform.eulerAngles = new Vector3(0, 0, initialRotation);
    }
}
