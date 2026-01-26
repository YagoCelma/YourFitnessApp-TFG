using UnityEngine;
using UnityEngine.SceneManagement;

public class AñadirEjercicio : MonoBehaviour
{

    public GameObject panelRutina;
    public Transform padre;

    public void agregarRutina()
    {
        Instantiate(panelRutina, padre);
    }

    public void NuevaRutina()
    {
        //RutinaSeleccionada.rutinaId = null;
        SceneManager.LoadScene(9);

    }


}
