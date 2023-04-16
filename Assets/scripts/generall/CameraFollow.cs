using UnityEngine;

// CameraFollow is attached to camera and makes it folow the player.
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float cameraBuffer = 0.3f;
    [SerializeField] private Camera camera;
    [SerializeField] private float zoomSpeed = 1.0f;
    [SerializeField] private float minZoom = 1.0f;
    [SerializeField] private float maxZoom = 10.0f;

    // Update is called once per frame.
    void Update()
    {
        // Following player
        if (camera.orthographicSize * (1 - cameraBuffer) < player.position.y - camera.transform.position.y)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, player.position.y - camera.orthographicSize * (1 - cameraBuffer), camera.transform.position.z);
        }
        else if (-camera.orthographicSize * (1 - cameraBuffer) > player.position.y - camera.transform.position.y)
        {
            camera.transform.position = new Vector3(camera.transform.position.x, player.position.y + camera.orthographicSize * (1 - cameraBuffer), camera.transform.position.z);
        }
        if (camera.orthographicSize * camera.aspect * (1 - cameraBuffer) < player.position.x - camera.transform.position.x)
        {
            camera.transform.position = new Vector3(player.position.x - camera.orthographicSize * camera.aspect * (1 - cameraBuffer), camera.transform.position.y, camera.transform.position.z);
        }
        else if (-camera.orthographicSize * camera.aspect * (1 - cameraBuffer) > player.position.x - camera.transform.position.x)
        {
            camera.transform.position = new Vector3(player.position.x + camera.orthographicSize * camera.aspect * (1 - cameraBuffer), camera.transform.position.y, camera.transform.position.z);
        }

        // Zooming in and out.
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (zoomDelta != 0)
        {
            float newZoom = Mathf.Clamp(camera.orthographicSize - zoomDelta, minZoom, maxZoom);
            camera.orthographicSize = newZoom;
        }
    }
}
