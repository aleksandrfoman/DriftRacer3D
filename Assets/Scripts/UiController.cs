using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [Header("ScorePanel")]
    [SerializeField]
    private GameObject factorPanel;
    [SerializeField]
    private TMP_Text factorText;
    [SerializeField]
    private TMP_Text scoreText;
    [Header("StarPanel")]
    [SerializeField]
    private TMP_Text starText;
    [Header("Panels")]
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject losePanel;
    [SerializeField]
    private GameObject gamePanel;


    public void ActiavateLose()
    {
        losePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void ActiavateWin()
    {
        winPanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    public void SetScoreText(int value)
    {
        scoreText.text = "Score: "+value.ToString();
    }
    public void SetFactorText(int value)
    {
        factorText.text = "x" + value.ToString();
    }

    public void SetStarText(int currentStar,int maxStar)
    {
        starText.text = currentStar.ToString() + "/" + maxStar.ToString();
    }
    public void ActivateFactorPanel(bool value)
    {
        factorPanel.SetActive(value);
    }
}
