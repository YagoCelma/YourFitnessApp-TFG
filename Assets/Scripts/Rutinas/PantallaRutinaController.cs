using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using UnityEngine;

public class PantallaRutinaController : MonoBehaviour
{
    public TMP_Text nombreRutinaTexto;

    public GameObject ejercicioPrefab;
    public Transform contenedorEjercicios;

    FirebaseAuth auth;
    FirebaseFirestore db;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

        CargarRutina();
        CargarEjercicios();
    }

    void CargarRutina()
    {
        string uid = auth.CurrentUser.UserId;

        db.Collection("users")
        .Document(uid)
        .Collection("rutinas")
        .Document(RutinaSeleccionada.rutinaId)
        .GetSnapshotAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (!task.Result.Exists) return;

            string nombre = task.Result.GetValue<string>("nombre");

            nombreRutinaTexto.text = nombre;
        });
    }

    void CargarEjercicios()
    {
        string uid = auth.CurrentUser.UserId;

        db.Collection("users")
        .Document(uid)
        .Collection("rutinas")
        .Document(RutinaSeleccionada.rutinaId)
        .Collection("ejercicios")
        .GetSnapshotAsync()
        .ContinueWithOnMainThread(task =>
        {
            foreach (var doc in task.Result.Documents)
            {
                string nombreEjercicio = doc.GetValue<string>("nombre");

                CrearEjercicio(nombreEjercicio);
            }
        });
    }

    void CrearEjercicio(string nombre)
    {
        GameObject nuevo = Instantiate(ejercicioPrefab, contenedorEjercicios);

        EjercicioItem item = nuevo.GetComponent<EjercicioItem>();

        item.Inicializar(nombre);
    }
}