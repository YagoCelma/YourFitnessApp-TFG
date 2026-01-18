using UnityEngine;
using TMPro;

public class AdvertenciaLargo : MonoBehaviour
{

    public GameObject panelAdvertencia;
    public TMP_Text tituloPanel;
    public TMP_Text mensajePanel;


    public void Mostrar(string titulo, string textoMensaje)
    {
        tituloPanel.text = titulo;
        mensajePanel.text = textoMensaje;
        panelAdvertencia.SetActive(true);
    }

    public void Ocultar()
    {
        panelAdvertencia.SetActive(false);
    }
}
