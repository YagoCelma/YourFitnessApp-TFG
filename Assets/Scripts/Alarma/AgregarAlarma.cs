using UnityEngine;

public class AgregarAlarma : MonoBehaviour
{
    public GameObject alarmaPrefab;
    public Transform transform;

    public void agregarAlarma()
    {
        Instantiate(alarmaPrefab, transform);
    }

}
