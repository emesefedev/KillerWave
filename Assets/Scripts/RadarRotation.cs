using UnityEngine;

public class RadarRotation : MonoBehaviour {

    private Quaternion rotation;

    private void Awake()
    {
        rotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
