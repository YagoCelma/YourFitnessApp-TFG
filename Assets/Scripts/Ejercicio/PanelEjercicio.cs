using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PanelEjercicio : MonoBehaviour
{
    [Header("Series")]
    public Transform contenedorSeries;
    public GameObject filaInputsPrefab;


    public void AgregarSerie()
    {
        GameObject nuevaFila = Instantiate(filaInputsPrefab, contenedorSeries);

        foreach (TMP_InputField input in nuevaFila.GetComponentsInChildren<TMP_InputField>())
        {
            if (input.gameObject.name == "Input_Serie") 
            {
                continue; 
            }
            input.text = "0";
        }

        ActualizarNumeracion();

        LayoutRebuilder.ForceRebuildLayoutImmediate(contenedorSeries.GetComponent<RectTransform>());
    }

    public void EliminarSerie()
    {
        if (contenedorSeries.childCount <= 1) return;

        GameObject ultimaFila = contenedorSeries.GetChild(contenedorSeries.childCount - 1).gameObject;
        
        ultimaFila.transform.SetParent(null);
        Destroy(ultimaFila);

        ActualizarNumeracion();

        LayoutRebuilder.ForceRebuildLayoutImmediate(contenedorSeries.GetComponent<RectTransform>());
    }

    public void ActualizarNumeracion()
    {
        int contador = 1;
        
        foreach (Transform hijo in contenedorSeries)
        {
            FilaSerie fila = hijo.GetComponent<FilaSerie>();
            if (fila != null)
            {
                fila.SetNumeroSerie(contador);
                contador++;
            }
        }
    }
}