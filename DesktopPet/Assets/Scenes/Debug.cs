using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DebugSession : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        ARSession session = this.GetComponent<ARSession>();
        if (session != null)
        {
            session.requestedTrackingMode = TrackingMode.PositionAndRotation;
        }
    }
}
