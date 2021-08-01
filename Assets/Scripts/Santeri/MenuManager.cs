using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject menuParent;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuParent.SetActive(!menuParent.activeInHierarchy);
            Time.timeScale = menuParent.activeInHierarchy ? 0 : 1;
        }
    }
}
