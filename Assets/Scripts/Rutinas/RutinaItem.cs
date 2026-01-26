using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RutinaItem : MonoBehaviour
{
    public string rutinaId;
    public TextMeshProUGUI nombreTexto;

    public void Iniciar(string id, string nombre)
    {
        rutinaId = id;
        nombreTexto.text = nombre;
    }

    public void OnClickRutina()
    {
        //Para guardar la rutina
        RutinaSeleccionada.rutinaId = rutinaId;

        SceneManager.LoadScene(9);
    }
}
