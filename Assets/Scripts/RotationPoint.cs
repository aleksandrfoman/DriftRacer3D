using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RotationPoint : MonoBehaviour
{
    public Transform ParentForDrift;
    [SerializeField]
    private Image image;
    [SerializeField]
    private int scoreForPoint;
    [SerializeField]
    public Canvas canvasCircle;
    [SerializeField]
    private Canvas arrow;
    public int ScoreForPoint => scoreForPoint;

    
    public void SwitchArrow()
    {
        arrow.transform.localScale = new Vector3(0.01f, -0.01f, 0.01f);
    }

    public float SetImageAmount(float value, bool clockwise)
    {
        image.fillAmount = value;
        image.fillClockwise = clockwise;
        return value;
    }

    public void SetImageColor(Color color)
    {
        image.DOColor(color, 0.25f);
    }
}
