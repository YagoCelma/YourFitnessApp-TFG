using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorPrincipal : MonoBehaviour
{
    public void Ejercicio()
    {
        SceneManager.LoadScene(3);
    }

    public void Rutinas()
    {
        SceneManager.LoadScene(7);
    }

    public void Progreso()
    {
        SceneManager.LoadScene(5);
    }

    public void Alarmas()
    {
        SceneManager.LoadScene(6);
    }

    public void Perfil()
    {
        SceneManager.LoadScene(8);
    }
}
