using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PanelEjercicio : MonoBehaviour
{
    [Header("Series")]
    public Transform contenedorSeries;
    public GameObject filaInputsPrefab;

    private int numeroSerie = 1;

    public void AgregarSerie()
    {
        numeroSerie++;

        GameObject nuevaFila = Instantiate(filaInputsPrefab, contenedorSeries);

        FilaSerie fila = nuevaFila.GetComponent<FilaSerie>();
        if (fila != null)
        {
            fila.SetNumeroSerie(numeroSerie);
        }

        foreach (TMP_InputField input in nuevaFila.GetComponentsInChildren<TMP_InputField>())
        {
            if (input.gameObject.name == "Input_Serie") 
            {
                continue; 
            }
            
            input.text = "0";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contenedorSeries.GetComponent<RectTransform>());
    }

    public void EliminarSerie()
    {
        if (contenedorSeries.childCount <= 1) return;

        Destroy(contenedorSeries.GetChild(contenedorSeries.childCount - 1).gameObject);

        numeroSerie--;

        LayoutRebuilder.ForceRebuildLayoutImmediate(contenedorSeries.GetComponent<RectTransform>());
    }
}