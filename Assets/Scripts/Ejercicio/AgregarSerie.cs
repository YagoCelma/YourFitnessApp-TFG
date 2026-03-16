using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AgregarSerie : MonoBehaviour
{
    public GameObject filaPrefab;
    public Transform contenedorSeries;

    public void CrearNuevaSerie()
    {
        GameObject nuevaFila = Instantiate(filaPrefab, contenedorSeries);

        foreach (TMP_InputField input in nuevaFila.GetComponentsInChildren<TMP_InputField>())
            input.text = "";

        LayoutRebuilder.ForceRebuildLayoutImmediate(contenedorSeries.GetComponent<RectTransform>());
    }

    public void EliminarSerie()
    {
        if (contenedorSeries.childCount <= 1) return;

        Destroy(contenedorSeries.GetChild(contenedorSeries.childCount - 1).gameObject);

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            contenedorSeries.GetComponent<RectTransform>()
        );
    }
}

