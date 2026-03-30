using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarioDia : MonoBehaviour
{
    public TMP_Text numeroDia;
    public Image fondoDia;

    private Color colorNormal = Color.white;
    private Color colorEntrenado = new Color(0.337f, 0.706f, 0.353f);
    private Color colorHoy = new Color(0.129f, 0.588f, 0.953f);

    private Color colorTextoNormal = Color.black;
    private Color colorTextoResaltado = Color.white;

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