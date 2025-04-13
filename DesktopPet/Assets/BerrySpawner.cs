using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class BerrySpawner : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public GameObject berryPrefab;

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var newPlane in args.added)
        {
            SpawnBerriesOnPlane(newPlane);
        }
    }

    void SpawnBerriesOnPlane(ARPlane plane)
    {
        Bounds bounds = plane.GetComponent<Renderer>().bounds;

        Debug.Log("SpawnedBerry");
        Vector3 randomPos = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.center.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        Vector3 closestPoint = plane.GetComponent<Collider>().ClosestPoint(randomPos);
        Instantiate(berryPrefab, closestPoint, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
    }
}
