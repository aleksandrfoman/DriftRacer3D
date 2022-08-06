using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationPoint : MonoBehaviour
{
    public Transform ParentForDrift;
    [SerializeField]
    private Image image;

    public float SetImageAmount(float value)
    {
        image.fillAmount = value;
        return value;
    }
}
