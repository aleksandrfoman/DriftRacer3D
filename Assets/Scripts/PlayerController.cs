using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private float speed, speedRotateCar, speedDrift, radiusOffset, startDistDrfit;
    private Transform parentTarget;
    private RotationPoint currentRotationPoint;
    private Vector3 vectorTarget;
    [SerializeField]
    private Transform playerMesh;
    private float distance;
    private bool isDirRight;
    private float lastAmount;
    private float lengthCircle, currentCirclePass;

    [SerializeField]
    private float rayDistance;

    private RaycastHit[] hits = new RaycastHit[1];

    [SerializeField]
    private new Rigidbody rigidbody;
    [SerializeField]
    private Vector3 offsetRayVector;
    [SerializeField]
    private float impulsePower, impulseUpPower, torqPower;
    [SerializeField]
    private bool isDrive;
    private void Start()
    {
        lengthCircle = Mathf.PI * startDistDrfit * 2;
        isDrive = true;
    }

    void Update()
    {
        if (Physics.RaycastNonAlloc(transform.position + offsetRayVector, Vector3.down, hits, rayDistance) > 0)
        {
            if (hits[0].transform != null)
            {
                Debug.Log(hits[0].transform.name);
            }
        }
        else if(isDrive)
        {
            rigidbody.useGravity = true;
            rigidbody.freezeRotation = false;
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.AddForce(transform.up * impulseUpPower, ForceMode.Impulse);
            rigidbody.AddForce(transform.forward * impulsePower, ForceMode.Impulse);
            rigidbody.AddTorque(Vector3.forward*torqPower, ForceMode.Impulse);
            isDrive = false;
        }

        if (parentTarget != null && currentRotationPoint != null && isDrive)
        {
            distance = Vector3.Distance(transform.position, parentTarget.position);
            if (distance <= startDistDrfit)
            {
                currentCirclePass += speed * Time.deltaTime;
                lastAmount = currentRotationPoint.SetImageAmount(currentCirclePass / lengthCircle, isDirRight);

                if (lastAmount >= 0.80f && lastAmount <= 1f)
                {
                    currentRotationPoint.SetImageColor(Color.blue);
                }
                else if (lastAmount <= 0.80f)
                {
                    currentRotationPoint.SetImageColor(Color.red);
                }
                else
                {
                    currentRotationPoint.SetImageColor(Color.red);
                }

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

        if (currentRotationPoint != null)
        {
            if (lastAmount >= 0.80f && lastAmount <= 1f)
            {
                levelManager.UpdateScore(currentRotationPoint.ScoreForPoint);
                levelManager.UpdateUiFactor();
                lastAmount = 0f;
            }
            else if (lastAmount >= 1f)
            {
                levelManager.ResetUiFactor();
                levelManager.UpdateScore(currentRotationPoint.ScoreForPoint);
                lastAmount = 0f;
            }
            else
            {
                levelManager.ResetUiFactor();
                lastAmount = 0f;
            }
            Destroy(currentRotationPoint.gameObject);
        }

        isDirRight = driftRight;
        parentTarget = targetPoint.ParentForDrift;
        currentRotationPoint = targetPoint;

        currentRotationPoint.canvasCircle.transform.LookAt(transform);
        currentRotationPoint.canvasCircle.transform.localEulerAngles += new Vector3(-90f, 0f, 0f);

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

        if (vectorTarget != null)
            Gizmos.DrawWireSphere(vectorTarget, 1f);

        Gizmos.DrawRay(transform.position+ offsetRayVector, Vector3.down*rayDistance);
    }
}
