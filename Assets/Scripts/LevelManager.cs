using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pointPrefab;
    [SerializeField]
    PlayerController playerController;

    private RaycastHit[] hits = new RaycastHit[1];

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.RaycastNonAlloc(Camera.main.ScreenPointToRay(Input.mousePosition), hits) > 0)
            {
                GameObject currentObj = Instantiate(pointPrefab, hits[0].point, Quaternion.identity);

                if (currentObj.transform.position.x > playerController.transform.position.x)
                {
                    playerController.SetTargetParent(currentObj.transform, true);
                    Debug.Log("Right");
                }
                else
                {
                    playerController.SetTargetParent(currentObj.transform, false);
                    Debug.Log("Left");
                }
            }
        }
    }
}
