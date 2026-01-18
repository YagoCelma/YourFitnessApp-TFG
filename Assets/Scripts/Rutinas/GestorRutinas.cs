using UnityEngine;

public class GestorRutinas : MonoBehaviour
{
    public GameObject panelTusRutinas;
    public GameObject panelNuevaRutina;

    public void IrNuevaRutina()
    {
        panelTusRutinas.SetActive(false);
        panelNuevaRutina.SetActive(true);
    }

    public void IrTusRutinas()
    {
        panelNuevaRutina.SetActive(false);
        panelTusRutinas.SetActive(true);
    }
}
