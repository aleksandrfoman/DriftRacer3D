using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private int starsOfLevel;
    private int currentStar;
    private bool isGame;
    public bool IsGame => isGame;

    private int currentPointCount;


    [SerializeField]
    private float minDistToNextPoint;

    private RaycastHit[] hits = new RaycastHit[1];

    private void Start()
    {
        Application.targetFrameRate = 60;
        isGame = true;
        currentPointCount = 0;
        ResetUiFactor();
        uiController.SetStarText(currentStar, starsOfLevel);
    }
    private void Update()
    { 

        if (isGame)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Physics.RaycastNonAlloc(Camera.main.ScreenPointToRay(Input.mousePosition), hits, Mathf.Infinity, groundMask) > 0)
                {
                    if (Vector3.Distance(hits[0].point, playerController.transform.position) >= minDistToNextPoint)
                    {
                        RotationPoint currentPoint = Instantiate(pointPrefab, hits[0].point, Quaternion.identity);


                        //currentPoint.transform.position.x > playerController.transform.position.x
                        currentPointCount++;
                        if (currentPointCount%2 == 0)
                        {
                            playerController.SetTargetParent(currentPoint, true);
                        }
                        else
                        {
                            playerController.SetTargetParent(currentPoint, false);
                            currentPoint.SwitchArrow();
                        }
                    }
                }
            }
        }
    }

    public void Lose()
    {
        uiController.ActiavateLose();
        isGame = false;
    }

    public void Win()
    {
        uiController.ActiavateWin();
        isGame = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateStars()
    {
        currentStar++;
        uiController.SetStarText(currentStar, starsOfLevel);
        if (currentStar == starsOfLevel)
        {
            Win();
        }
    }

    public void UpdateScore(int value)
    {
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
