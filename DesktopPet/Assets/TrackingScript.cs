using UnityEngine;
public class TrackingScript : MonoBehaviour
{
    [SerializeField] Transform TrackedObject;
    [SerializeField] Transform CameraTransform;
    [SerializeField] float TimeToTrack = 0.1f; // lower = faster tracking

    void Update()
    {
        if (TrackedObject != null)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                TrackedObject.position,
                TimeToTrack * Time.deltaTime
            );
        }

        transform.LookAt(CameraTransform);
    }
}