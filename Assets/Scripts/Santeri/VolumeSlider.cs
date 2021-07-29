using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    Slider slider;
    [SerializeField]
    TextMeshProUGUI text;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        AudioListener.volume = slider.value;
        text.text = (int)(slider.value * 100) + " %";
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(float val)
    {
        AudioListener.volume = val;
        text.text = (int)(val * 100) + " %";
    }
}
