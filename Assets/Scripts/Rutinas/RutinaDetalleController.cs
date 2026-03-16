using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;

public class RutinaDetalleController : MonoBehaviour
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
    }

    void CargarRutina()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("No hay usuario logeado");
            return;
        }

        string uid = auth.CurrentUser.UserId;

        db.Collection("users")
        .Document(uid)
        .Collection("rutinas")
        .Document(RutinaSeleccionada.rutinaId)
        .GetSnapshotAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error cargando rutina");
                return;
            }

            if (task.Result.Exists)
            {
                string nombre = task.Result.GetValue<string>("nombre");

                nombreRutinaTexto.text = nombre;
            }
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
                string nombre = doc.GetValue<string>("nombre");
                int peso = doc.GetValue<int>("peso");
                int reps = doc.GetValue<int>("reps");
                int series = doc.GetValue<int>("series");

                CrearEjercicio(nombre, peso, reps, series);
            }
        });
    }

    void CrearEjercicio(string nombre, int peso, int reps, int series)
{
    GameObject nuevo = Instantiate(ejercicioPrefab, contenedorEjercicios);

    EjercicioUI ejercicio = nuevo.GetComponent<EjercicioUI>();

    ejercicio.Configurar(nombre, peso, reps, series);
}

    public async void GuardarRutina()
{
    string uid = auth.CurrentUser.UserId;

    var ejercicios = contenedorEjercicios.GetComponentsInChildren<EjercicioUI>();

    foreach (var ejercicio in ejercicios)
    {
        Dictionary<string, object> datos = new()
        {
            { "nombre", ejercicio.ObtenerNombre() },
            { "peso", ejercicio.ObtenerPeso() },
            { "reps", ejercicio.ObtenerReps() },
            { "series", ejercicio.ObtenerSeries() }
        };

        await db.Collection("users")
        .Document(uid)
        .Collection("rutinas")
        .Document(RutinaSeleccionada.rutinaId)
        .Collection("ejercicios")
        .AddAsync(datos);
    }

    Debug.Log("Entrenamiento guardado");
}
}