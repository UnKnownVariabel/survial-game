using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float cameraBuffer = 0.3f;
    [SerializeField] private Camera camera;

    void Update()
    {
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
    }
}
