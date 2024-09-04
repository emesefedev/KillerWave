using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector3 initialCameraPosition = new Vector3(0, 0, 300);

    private void Start()
    {
        CameraSetup();
    }

    private void CameraSetup() 
    {
        mainCamera.transform.position = initialCameraPosition;
        mainCamera.transform.rotation = Quaternion.identity;

        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mainCamera.backgroundColor = Color.black;
    }
}
