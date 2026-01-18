using UnityEngine;

public class CrearRutina : MonoBehaviour
{
    public GameObject panelRutina;
    public Transform padre;

    public void CrearNuevaRutina()
    {
        Instantiate(panelRutina, padre);
    }
}
