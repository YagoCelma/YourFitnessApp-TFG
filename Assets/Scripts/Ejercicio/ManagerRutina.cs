using UnityEngine;
using UnityEngine.UI;

public class ManagerRutina : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject panelEjercicioPrefab;

    [Header("UI")]
    public Transform content;

    public void AgregarEjercicio()
    {

        GameObject nuevoEjercicio = Instantiate(panelEjercicioPrefab, content);

        // Forzar refresco de layout (CLAVE)
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)content);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)content.parent);
        Canvas.ForceUpdateCanvases();
    }

 
}
