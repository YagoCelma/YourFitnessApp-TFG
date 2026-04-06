using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreadorRutina : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField nombreRutinaInput;
    public Transform contenedorEjercicios;
    public GameObject ejercicioPrefab;

    FirebaseAuth auth;
    FirebaseFirestore db;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

        if (!string.IsNullOrEmpty(RutinaSeleccionada.rutinaId))
        {
            CargarNombreRutina();
            CargarEjercicios();
        }
    }

    public async void GuardarRutina()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("No hay usuario logueado");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        string nombreRutina = nombreRutinaInput.text;

        if (string.IsNullOrEmpty(nombreRutina))
        {
            Debug.LogWarning("Nombre de rutina vacío");
            return;
        }

        DocumentReference rutinaRef;

        if (string.IsNullOrEmpty(RutinaSeleccionada.rutinaId))
        {
            rutinaRef = db
                .Collection("users")
                .Document(uid)
                .Collection("rutinas")
                .Document();

            RutinaSeleccionada.rutinaId = rutinaRef.Id;
        }
        else
        {
            rutinaRef = db
                .Collection("users")
                .Document(uid)
                .Collection("rutinas")
                .Document(RutinaSeleccionada.rutinaId);
        }

        Dictionary<string, object> datosRutina = new()
        {
            { "nombre", nombreRutina },
            { "creadoDia", Timestamp.GetCurrentTimestamp() }
        };

        await rutinaRef.SetAsync(datosRutina);

        QuerySnapshot ejerciciosViejos = await rutinaRef.Collection("ejercicios").GetSnapshotAsync();

        foreach (DocumentSnapshot doc in ejerciciosViejos.Documents)
        {
            await doc.Reference.DeleteAsync();
        }

        foreach (Transform hijo in contenedorEjercicios)
        {
            TMP_InputField input = hijo.GetComponentInChildren<TMP_InputField>();

            if (input != null && !string.IsNullOrEmpty(input.text))
            {
                Dictionary<string, object> ejercicio = new()
                {
                    { "nombre", input.text }
                };

                await rutinaRef
                    .Collection("ejercicios")
                    .AddAsync(ejercicio);
            }
        }

        Debug.Log("Rutina guardada o actualizada correctamente");

        SceneManager.LoadScene(7);
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
            if (task.IsFaulted)
            {
                Debug.LogError("Error cargando ejercicios");
                return;
            }

            foreach (var doc in task.Result.Documents)
            {
                string ejercicioId = doc.Id;
                string nombreEjercicio = doc.GetValue<string>("nombre");

                CrearEjercicioUI(ejercicioId, nombreEjercicio);
            }
        });
    }

    void CrearEjercicioUI(string ejercicioId, string nombre)
    {
        GameObject nuevo = Instantiate(ejercicioPrefab, contenedorEjercicios);

        TMP_InputField input = nuevo.GetComponentInChildren<TMP_InputField>();

        if (input != null)
            input.text = nombre;

        PrefabEjercicio prefab = nuevo.GetComponent<PrefabEjercicio>();

        if (prefab != null)
            prefab.SetEjercicioId(ejercicioId);
    }

    void CargarNombreRutina()
    {
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
                  Debug.LogError("Error cargando nombre rutina");
                  return;
              }

              if (task.Result.Exists)
              {
                  string nombre = task.Result.GetValue<string>("nombre");

                  nombreRutinaInput.text = nombre;

                  nombreRutinaInput.ForceLabelUpdate();
              }
          });
    }
}