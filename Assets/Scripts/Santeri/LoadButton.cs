using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadButton : MonoBehaviour
{
    [SerializeField]
    string textString;
    [SerializeField]
    string sceneName;

    Button button;
    TMP_Text text;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        text = GetComponentInChildren<TMP_Text>();
        text.text = textString;
    }

    void OnClick()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
