using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector3 offsetVector;

    private void Update()
    {
        Vector3 targetPos = playerController.transform.position + offsetVector;
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }
}
