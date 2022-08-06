using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private RotationPoint pointPrefab;
    [SerializeField]
    PlayerController playerController;

    private RaycastHit[] hits = new RaycastHit[1];

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.RaycastNonAlloc(Camera.main.ScreenPointToRay(Input.mousePosition), hits) > 0)
            {
                RotationPoint currentPoint = Instantiate(pointPrefab, hits[0].point, Quaternion.identity);

                if (currentPoint.transform.position.x > playerController.transform.position.x)
                {
                    playerController.SetTargetParent(currentPoint, true);
                }
                else
                {
                    playerController.SetTargetParent(currentPoint, false);
                }
            }
        }
    }
}
