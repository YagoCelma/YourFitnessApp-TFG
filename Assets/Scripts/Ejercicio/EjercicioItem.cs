using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EjercicioItem : MonoBehaviour
{
    public TMP_Text tituloEjercicio;
    public TMP_InputField tituloInput;

    public Transform contenedorSeries;
    public GameObject seriePrefab;

    public void Inicializar(string nombre)
    {
        if (tituloEjercicio != null)
        {
            tituloEjercicio.text = nombre;
        }
    }

    public void AgregarSerie()
    {
        Instantiate(seriePrefab, contenedorSeries);
    }

    public string ObtenerNombre()
    {
        if (tituloInput != null && !string.IsNullOrWhiteSpace(tituloInput.text))
        {
            return tituloInput.text;
        }
        
        if (tituloEjercicio != null && !string.IsNullOrWhiteSpace(tituloEjercicio.text))
        {
            return tituloEjercicio.text;
        }

        return null; 
    }

    public List<Dictionary<string, int>> ObtenerSeries()
    {
        List<Dictionary<string, int>> lista = new();

        foreach (Transform serie in contenedorSeries)
        {
            TMP_InputField[] inputs = serie.GetComponentsInChildren<TMP_InputField>();

            if (inputs.Length < 3) continue;

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