using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    public async void GuardarEntrenamiento()
    {
        string uid = auth.CurrentUser.UserId;
        string fecha = System.DateTime.Now.ToString("yyyy-MM-dd");

        DocumentReference docFecha = db.Collection("users").Document(uid).Collection("entrenamientos").Document(fecha);

        Dictionary<string, object> datosEntrenamiento = new Dictionary<string, object>
        {
            { "completado", true },
            { "nombreRutina", nombreRutinaTexto.text }
        };

        await docFecha.SetAsync(datosEntrenamiento, SetOptions.MergeAll);

        var ejercicios = contenedorEjercicios.GetComponentsInChildren<EjercicioItem>();

        foreach (var ejercicio in ejercicios)
        {
            string nombre = ejercicio.ObtenerNombre();
            Debug.Log($"Procesando ejercicio: '{nombre}'");

            if (string.IsNullOrWhiteSpace(nombre))
            {
                Debug.LogWarning("Se ha saltado un ejercicio porque no tenía nombre escrito.");
                continue;
            }

            var series = ejercicio.ObtenerSeries();
            var listaSeries = new List<object>();

            foreach (var serie in series)
            {
                listaSeries.Add(new Dictionary<string, object>
            {
                { "kg", serie["kg"] },
                { "reps", serie["reps"] }
            });
            }

            await docFecha.Collection("ejercicios").Document(nombre).SetAsync(new Dictionary<string, object>
        {
            { "series", listaSeries }
        });
        }

        Debug.Log("Entrenamiento guardado con éxito. ¡Adiós documentos fantasma!");
        SceneManager.LoadScene(2);
    }
}