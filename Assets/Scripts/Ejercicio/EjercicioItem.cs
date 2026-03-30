using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EjercicioItem : MonoBehaviour
{
    public TMP_Text tituloEjercicio;

    public Transform contenedorSeries;
    public GameObject seriePrefab;

    public void Inicializar(string nombre)
    {
        tituloEjercicio.text = nombre;
    }

    public void AgregarSerie()
    {
        Instantiate(seriePrefab, contenedorSeries);
    }

    public string ObtenerNombre()
    {
        return tituloEjercicio.text;
    }

    public List<Dictionary<string, int>> ObtenerSeries()
    {
        List<Dictionary<string, int>> lista = new();

        foreach (Transform serie in contenedorSeries)
        {
            TMP_InputField[] inputs = serie.GetComponentsInChildren<TMP_InputField>();

            Debug.Log($"Input[1]: {inputs[1].text}");
            Debug.Log($"Input[2]: {inputs[2].text}");

            if (inputs.Length < 2) continue;

            int kg = 0;
            int reps = 0;

            int.TryParse(inputs[1].text, out kg);  
            int.TryParse(inputs[2].text, out reps);

            lista.Add(new Dictionary<string, int>
        {
            { "kg", kg },
            { "reps", reps }
        });
        }

        return lista;
    }
}