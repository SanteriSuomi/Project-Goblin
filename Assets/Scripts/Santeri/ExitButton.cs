using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExitButton : MonoBehaviour
{
    [SerializeField]
    string textString;

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
        Application.Quit();
    }
}