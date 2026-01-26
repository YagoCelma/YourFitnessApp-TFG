using UnityEngine;
using UnityEngine.UI;

public class AlarmaToggle : MonoBehaviour
{
    public Toggle toggle;
    public Image background;
    public RectTransform handle;

    public Color onColor = new Color(0.25f, 0.65f, 1f);   // azul ON
    public Color offColor = new Color(0.27f, 0.30f, 0.38f); // gris OFF

    Vector2 handleOnPos = new Vector2(30, 0);
    Vector2 handleOffPos = new Vector2(-30, 0);

    void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleChanged);
        OnToggleChanged(toggle.isOn);
    }

    void OnToggleChanged(bool state)
    {
        background.color = state ? onColor : offColor;
        handle.anchoredPosition = state ? handleOnPos : handleOffPos;
    }
}
