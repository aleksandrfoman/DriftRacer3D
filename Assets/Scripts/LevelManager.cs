using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private RotationPoint pointPrefab;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    private UiController uiController;
    [SerializeField]
    private LayerMask groundMask;
    private int currentScore;
    private int currentFactor;

    private RaycastHit[] hits = new RaycastHit[1];

    private void Start()
    {
        ResetUiFactor();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.RaycastNonAlloc(Camera.main.ScreenPointToRay(Input.mousePosition), hits, Mathf.Infinity, groundMask) > 0)
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

    public void UpdateScore(int value)
    {
        Debug.Log("ScoreUpdate+= " + value);
        currentScore += value * currentFactor;
        uiController.SetScoreText(currentScore);
    }

    public void UpdateUiFactor()
    {
        currentFactor++;
        if (currentFactor == 1)
        {
            uiController.ActivateFactorPanel(false);
        }
        else
        {
            uiController.ActivateFactorPanel(true);
        }
        uiController.SetFactorText(currentFactor);
    }

    public void ResetUiFactor()
    {
        currentFactor = 1;
        uiController.ActivateFactorPanel(false);
    }
}
