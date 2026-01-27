using UnityEngine;

public class PrefabAlarma : MonoBehaviour
{
    public GameObject alarma;

    public void borrarAlarma()
    {
        Destroy(alarma);
    }
}
