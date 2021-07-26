using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject menuParent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuParent.SetActive(!menuParent.activeInHierarchy);
            Time.timeScale = menuParent.activeInHierarchy ? 0 : 1;
        }
    }
}
