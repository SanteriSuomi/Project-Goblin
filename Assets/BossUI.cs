using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUI : MonoBehaviour
{
    GameObject canvas;

    void Awake()
    {
        canvas = gameObject;
    }

    public void Hide()
    {
        canvas.SetActive(false);
    }
}
