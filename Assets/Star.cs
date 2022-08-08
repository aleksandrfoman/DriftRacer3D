using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField]
    private float speed;
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime*speed);
    }

    public void TakeStar()
    {
        Destroy(gameObject);
    }
}
