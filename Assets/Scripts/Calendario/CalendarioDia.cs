using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarioDia : MonoBehaviour
{
    public TMP_Text numeroDia;
    public Image fondoDia;

    public Color colorNormal = Color.white;
    public Color colorEntrenado = Color.green;
    public Color colorHoy = Color.blue;

    public Color colorTextoNormal = Color.black;
    public Color colorTextoResaltado = Color.white;

    public void Configurar(int dia, bool entrenado, bool esHoy)
    {
        numeroDia.text = dia.ToString();

        if (esHoy)
        {
            fondoDia.color = colorHoy;
            numeroDia.color = colorTextoResaltado;
        }
        else if (entrenado)
        {
            fondoDia.color = colorEntrenado;
            numeroDia.color = colorTextoResaltado;
        }
        else
        {
            fondoDia.color = colorNormal;
            numeroDia.color = colorTextoNormal;
        }
    }
}