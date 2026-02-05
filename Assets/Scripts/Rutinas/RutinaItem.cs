using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RutinaItem : MonoBehaviour
{
    public TMP_Text nombreRutina;

    private string rutinaId;

    public void Iniciar(string id, string nombre)
    {
        rutinaId = id;
        nombreRutina.text = nombre;

    }

    public void OnClick()
    {
        RutinaSeleccionada.rutinaId = rutinaId;
        SceneManager.LoadScene(9);
    }
}
