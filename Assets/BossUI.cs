using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUI : MonoBehaviour
{
	GameObject canvas;

    void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("BossUI");
        //Hide();
    }

    public void Hide() {
    	canvas.SetActive(false);
    	Debug.Log("fuck");
    }

    public void Show() {
    	canvas.SetActive(true);
    }

}
