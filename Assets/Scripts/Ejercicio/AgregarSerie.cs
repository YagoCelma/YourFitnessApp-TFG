using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class AgregarSerie : MonoBehaviour
{
    public GameObject filaPrefab;
    public Transform contenedorSeries;
    public void CrearNuevaSerie()
    {
        GameObject nuevaFila = Instantiate(filaPrefab, contenedorSeries);

        // OPCIONAL: limpiar inputs
        foreach (TMP_InputField input in nuevaFila.GetComponentsInChildren<TMP_InputField>())
            input.text = "";

        // FORZAR REFRESH DEL LAYOUT
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contenedorSeries);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contenedorSeries.parent);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contenedorSeries.root);

        Canvas.ForceUpdateCanvases();

    }
}
