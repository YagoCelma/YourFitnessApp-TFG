using UnityEngine;
using UnityEngine.UI;

public class PanelEjercicio : MonoBehaviour
{
    [Header("Series")]
    public Transform contenedorSeries;
    public GameObject filaInputsPrefab;

    private int numeroSerie = 1;

    public void AgregarSerie()
    {
        numeroSerie++;

        
        GameObject nuevaFila = Instantiate(filaInputsPrefab,contenedorSeries);

        FilaSerie fila = nuevaFila.GetComponent<FilaSerie>();
        fila.SetNumeroSerie(numeroSerie);
    }

    public void EliminarSerie()
    {

        if (contenedorSeries.childCount <= 1) return;

        Destroy(contenedorSeries.GetChild(contenedorSeries.childCount - 1).gameObject);

        numeroSerie--;

        LayoutRebuilder.ForceRebuildLayoutImmediate(contenedorSeries.GetComponent<RectTransform>());
    }
}
