using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    RectTransform rectTransform;
    public Slider slider;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    private void Update()
    {
        if (Mathf.Abs(rectTransform.root.eulerAngles.sqrMagnitude) > 0)
        {
            rectTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}