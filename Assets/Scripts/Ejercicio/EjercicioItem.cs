using TMPro;
using UnityEngine;

public class EjercicioItem : MonoBehaviour
{
    public TMP_Text tituloEjercicio;

    /*public TMP_InputField serieInput;
    public TMP_InputField kgInput;
    public TMP_InputField repsInput;*/

    public void Inicializar(string nombre)
    {
        tituloEjercicio.text = nombre;
    }
}