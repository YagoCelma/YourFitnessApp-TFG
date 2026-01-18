using UnityEngine;

public class PrefabEjercicio : MonoBehaviour
{
    public GameObject gameObject;

    public void borrarEjercicio()
    {
        Destroy(gameObject);
    }
}
