using UnityEngine;
using TMPro;

public class RutinaItem : MonoBehaviour
{
    public TMP_Text nombreRutina;
    public TMP_Text numeroEjerciciosText;

    private string rutinaId;

    public void Inicializar(string id, string nombre, int numeroEjercicios)
    {
        rutinaId = id;
        nombreRutina.text = nombre;
        numeroEjerciciosText.text = numeroEjercicios + " ejercicios";
    }

    public void OnClick()
    {
        
    }
}
