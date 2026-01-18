using UnityEngine;

public class AñadirEjercicio : MonoBehaviour
{

    public GameObject panelEjercicio;
    public Transform padre;

    public void agregarEjercicio()
    {
        Instantiate(panelEjercicio, padre);
    }

}
