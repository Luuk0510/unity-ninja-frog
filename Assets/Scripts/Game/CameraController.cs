using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (Checkpoint.LastCheckpointPosition != Vector3.zero)
        {
            transform.position = Checkpoint.LastCheckpointPosition + offset;
        }
        else
        {
            transform.position = player.position + offset;
        }
    }

    private void Update()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void ResetCameraPosition()
    {
        Vector3 newCameraPosition = player.position + offset;
        transform.position = newCameraPosition;
        velocity = Vector3.zero;
    }
}
