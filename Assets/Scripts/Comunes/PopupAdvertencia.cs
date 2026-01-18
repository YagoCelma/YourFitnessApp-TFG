using UnityEngine;
using TMPro;

public class PopupAdvertencia : MonoBehaviour
{
    
    [Header("Prefab del panel")]
    [SerializeField] private GameObject popupModal;

    [Header("Textos")]
    [SerializeField] private TMP_Text tituloText;
    [SerializeField] private TMP_Text mensajeText;

    public void Mostrar(string titulo, string mensaje)
    {
        tituloText.text = titulo;
        mensajeText.text = mensaje;
        popupModal.SetActive(true);
    }

    public void Ocultar()
    {
        if (popupModal == null) return;
        popupModal.SetActive(false);
    }

}
