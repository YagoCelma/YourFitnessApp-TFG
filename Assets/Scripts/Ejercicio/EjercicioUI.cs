using TMPro;
using UnityEngine;

public class EjercicioUI : MonoBehaviour
{
    public TMP_Text nombreEjercicio;

    public TMP_InputField pesoInput;
    public TMP_InputField repsInput;
    public TMP_InputField seriesInput;

    public void Configurar(string nombre, int peso, int reps, int series)
    {
        nombreEjercicio.text = nombre;

        pesoInput.text = peso.ToString();
        repsInput.text = reps.ToString();
        seriesInput.text = series.ToString();
    }

    public string ObtenerNombre()
    {
        return nombreEjercicio.text;
    }

    public int ObtenerPeso()
    {
        int.TryParse(pesoInput.text, out int peso);
        return peso;
    }

    public int ObtenerReps()
    {
        int.TryParse(repsInput.text, out int reps);
        return reps;
    }

    public int ObtenerSeries()
    {
        int.TryParse(seriesInput.text, out int series);
        return series;
    }
}
