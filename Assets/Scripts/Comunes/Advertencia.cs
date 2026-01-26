using UnityEngine;
using TMPro;

public class Advertencia : MonoBehaviour
{
    public GameObject panelAdvertencia;
    public TMP_Text textoMensaje;

    public void Mostrar(string mensaje)
    {
        textoMensaje.text = mensaje;
        panelAdvertencia.SetActive(true);
    }

    public void Ocultar()
    {
        panelAdvertencia.SetActive(false);
    }
}
