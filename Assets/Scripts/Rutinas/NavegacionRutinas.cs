using UnityEngine;
using UnityEngine.SceneManagement;

public class NavegacionRutinas : MonoBehaviour
{
    public void ClickNuevaRutina()
    {
        RutinaSeleccionada.rutinaId = "";
        SceneManager.LoadScene(9); 
    }
}