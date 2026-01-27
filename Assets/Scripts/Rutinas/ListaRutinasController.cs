using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

public class ListaRutinasController : MonoBehaviour
{
    public GameObject rutinaPrefab;
    public Transform contenedoresRutinas;

    FirebaseAuth auth;
    FirebaseFirestore db;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

        CargarRutina();
    }

    void CargarRutina()
    {
        if(auth.CurrentUser == null)
        {
            Debug.LogError("No hay un usuario logeado");
            return;
        }

        string uid = auth.CurrentUser.UserId;

        db.Collection("users").Document(uid).Collection("rutinas").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al cargar rutinas: " + task.Exception);
                return;
            }

            //Para recorrer toda la rutina
            foreach(var doc in task.Result.Documents)
            {
                string rutinaId = doc.Id;
                string nombre = doc.GetValue<string>("nombre");

                CrearRutina(rutinaId, nombre);
            }
        });
    }

    void CrearRutina(string rutinaId, string nombre)
    {
        GameObject item = Instantiate(rutinaPrefab, contenedoresRutinas);
        RutinaItem rutinaItem = item.GetComponent<RutinaItem>();
        rutinaItem.Iniciar(rutinaId, nombre);
    }
}
