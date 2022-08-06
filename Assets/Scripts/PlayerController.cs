using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed, speedRotateCar, driftAngle, speedDrift,radiusOffset, startDistDrfit;
    [SerializeField]
    private Transform parentTarget;
    private RotationPoint currentRotationPoint;
    private Vector3 vectorTarget;
    [SerializeField]
    private Transform playerMesh;
    private float distance;
    [SerializeField]
    private bool isDirRight;
    [SerializeField]
    private float lastAmount;

    private float lengthCircle, currentCirclePass;


    private void Start()
    {
        lengthCircle = Mathf.PI * startDistDrfit * 2;
    }

    void Update()
    {
        if (parentTarget != null && currentRotationPoint!=null)
        {
            distance = Vector3.Distance(transform.position, parentTarget.position);

            if (distance <= startDistDrfit)
            {
                currentCirclePass += speed * Time.deltaTime;
                lastAmount = currentRotationPoint.SetImageAmount(currentCirclePass / lengthCircle);

                parentTarget.transform.Rotate(Vector3.up * (isDirRight ? -1 : 1) * speedDrift * Time.deltaTime);
                transform.parent = parentTarget;
            }
            else
            {
                currentCirclePass = 0f;
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                Quaternion originalRot = transform.rotation;
                transform.LookAt(vectorTarget);
                Quaternion newRot = transform.rotation;
                transform.rotation = originalRot;
                transform.rotation = Quaternion.Lerp(transform.rotation, newRot, speedRotateCar * Time.deltaTime);
            }
        }
    }

    public void SetTargetParent(RotationPoint targetPoint, bool driftRight)
    {
        if (lastAmount >= 0.75f && lastAmount <= 1.1f)
        {
            Debug.Log("Yes");
        }
        else
        {
            Debug.Log("Lose");
        }

        isDirRight = driftRight;

        if(currentRotationPoint != null)
        Destroy(currentRotationPoint.gameObject);

        parentTarget = targetPoint.ParentForDrift;
        currentRotationPoint = targetPoint;
        transform.parent = null;
        playerMesh.localEulerAngles = Vector3.zero;
        parentTarget.LookAt(new Vector3(transform.position.x, parentTarget.position.y, transform.position.z));
        vectorTarget = targetPoint.ParentForDrift.TransformPoint(Vector3.right * (isDirRight ? -1 : 1) * radiusOffset);
    }

    private void OnDrawGizmos()
    {
        if (distance <= startDistDrfit)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        if (parentTarget != null)
            Gizmos.DrawLine(transform.position, parentTarget.position);

        if(vectorTarget!=null)
        Gizmos.DrawWireSphere(vectorTarget, 1f);
    }

}
