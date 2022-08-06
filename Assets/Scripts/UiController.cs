using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField]
    private GameObject factorPanel;
    [SerializeField]
    private TMP_Text factorText;
    [SerializeField]
    private TMP_Text scoreText;

    public void SetScoreText(int value)
    {
        scoreText.text = "Score: "+value.ToString();
    }
    public void SetFactorText(int value)
    {
        factorText.text = "x" + value.ToString();
    }

    public void ActivateFactorPanel(bool value)
    {
        factorPanel.SetActive(value);
    }
}
