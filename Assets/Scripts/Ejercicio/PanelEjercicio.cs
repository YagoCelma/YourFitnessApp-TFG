using UnityEngine;

public class PanelEjercicio : MonoBehaviour
{
    [Header("Series")]
    public Transform contenedorSeries;
    public GameObject filaInputsPrefab;

    private int numeroSerie = 1;

    public void AgregarSerie()
    {
        numeroSerie++;

        
        GameObject nuevaFila = Instantiate(
            filaInputsPrefab,
            contenedorSeries
        );

        FilaSerie fila = nuevaFila.GetComponent<FilaSerie>();
        fila.SetNumeroSerie(numeroSerie);
    }
}
