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



    //private Vector3 vectorTarget;



    [SerializeField]
    private Transform playerMesh;
    private float distance;
    private bool isDirRight;
    private float lastAmount;
    private float lengthCircle, currentCirclePass;

    [SerializeField]
    private float rayDistance;

    private RaycastHit[] hitsForward = new RaycastHit[1];
    private RaycastHit[] hitsBack = new RaycastHit[1];


    [SerializeField]
    private new Rigidbody rigidbody;
    [SerializeField]
    private Vector3 offsetRayVectorForward, offsetRayVectorBack;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float impulsePower, impulseUpPower, torqPower;
    private bool isDrift;
    private float angle;
    [SerializeField]
    private float speedRotateAngle;

    [SerializeField]
    private TrailRenderer trailRendererLeft, trailRendererRight;
    [SerializeField]
    private float width = 1f;

    private void Start()
    {
        lengthCircle = Mathf.PI * startDistDrfit * 2;
    }

    void Update()
    {
        if (Physics.RaycastNonAlloc(playerMesh.position + offsetRayVectorForward, Vector3.down, hitsForward, rayDistance, layerMask) > 0 ||
            Physics.RaycastNonAlloc(playerMesh.position + offsetRayVectorBack, Vector3.down, hitsBack, rayDistance, layerMask) > 0) 
        {

        }
        else if(levelManager.IsGame)
        {
           Crashed();
        }

        if (parentTarget != null && currentRotationPoint != null && levelManager.IsGame)
        {
            
            distance = Vector3.Distance(transform.position, parentTarget.position);

            if (distance <= startDistDrfit)
            {
                currentCirclePass += speed * Time.deltaTime;
                if(lastAmount<=1.01f)
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



                trailRendererLeft.emitting = true;
                trailRendererRight.emitting = true;

                transform.parent = parentTarget;

                RotatePlayer(true);
                if (isDrift)
                {
                    angle = Mathf.Lerp(angle, 45f, speedRotateAngle * Time.deltaTime);
                    //Debug.Log("AngleOnRotate"+angle);
                    playerMesh.localEulerAngles = new Vector3(0f, -angle * (isDirRight ? -1 : 1), 0f);

                    if(angle >= 44.9f)
                    {
                        isDrift = false;
                    }
                }
            }
            else
            {
                angle = Mathf.Lerp(angle, 0f, speedRotateAngle * Time.deltaTime);
                playerMesh.localEulerAngles = new Vector3(0f, -angle * (isDirRight ? -1 : 1), 0f);
                
                trailRendererLeft.emitting = false;
                trailRendererRight.emitting = false;
                
                currentCirclePass = 0f;
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

                RotatePlayer(false);
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
        
        playerMesh.localEulerAngles = Vector3.zero;
        transform.parent = null;
        isDrift = true;
        
        //angle = 0f;
        
        //parentTarget.LookAt(new Vector3(transform.position.x, parentTarget.position.y, transform.position.z));
        //vectorTarget = targetPoint.ParentForDrift.TransformPoint(Vector3.right * (isDirRight ? -1 : 1) * radiusOffset);
    }


    private void RotatePlayer(bool drift)
    {
        Quaternion originalRot = transform.rotation;
        transform.LookAt(currentRotationPoint.transform);
        Quaternion newRot = transform.rotation;
        transform.rotation = originalRot;
        Quaternion procentPos = Quaternion.Lerp(transform.rotation, newRot, 0.45f);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, speedRotateCar * Time.deltaTime);

        if (drift == false)
        {
            var dir = Vector3.Normalize(currentRotationPoint.transform.position - transform.position);
            var dot = Vector3.Dot(transform.forward, dir);
            if (dot <= 0.5f)
            {

                trailRendererLeft.emitting = true;
                trailRendererRight.emitting = true;
            }
            else
            {

                trailRendererLeft.emitting = false;
                trailRendererRight.emitting = false;
            }
        }
    }


    private void Crashed()
    {
        trailRendererLeft.emitting = false;
        trailRendererRight.emitting = false;
        trailRendererLeft.transform.parent = null;
        trailRendererRight.transform.parent = null;


        rigidbody.useGravity = true;
        rigidbody.freezeRotation = false;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(transform.up * impulseUpPower, ForceMode.Impulse);
        rigidbody.AddForce(transform.forward * impulsePower, ForceMode.Impulse);
        rigidbody.AddTorque(Vector3.forward * torqPower, ForceMode.Impulse);
        transform.parent = null;

        if(currentRotationPoint != null)
        Destroy(currentRotationPoint.gameObject);

        levelManager.Lose();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Obstacle currentObstacle = collision.gameObject.GetComponent<Obstacle>();
        if (currentObstacle)
        {
            Crashed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Star star = other.gameObject.GetComponent<Star>();
        if (star)
        {
            star.TakeStar();
            levelManager.UpdateStars();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (parentTarget != null)
            Gizmos.DrawLine(transform.position, parentTarget.position);

        // if (vectorTarget != null)
        //     Gizmos.DrawWireSphere(vectorTarget, 1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(playerMesh.position + offsetRayVectorForward, Vector3.down * rayDistance);
        Gizmos.DrawRay(playerMesh.position + offsetRayVectorBack, Vector3.down * rayDistance);
    }
}
