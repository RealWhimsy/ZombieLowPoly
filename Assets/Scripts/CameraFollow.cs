using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0;
    public Vector3 cameraOffset;

    private void LateUpdate() {
        transform.position = target.position + cameraOffset;    
    }
}
