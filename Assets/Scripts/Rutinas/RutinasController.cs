using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class RutinasController : MonoBehaviour
{
    public GameObject rutinaPrefab;
    public Transform contenedorRutinas;

    FirebaseFirestore db;
    FirebaseAuth auth;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        CargarRutinas();
    }

    // ESTE método es el importante ahora
    void CargarRutinas()
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("No hay usuario logueado");
            return;
        }

        string uid = auth.CurrentUser.UserId;

        db.Collection("users")
          .Document(uid)
          .Collection("rutinas")
          .GetSnapshotAsync()
          .ContinueWith(task =>
          {
              if (!task.IsCompletedSuccessfully) return;

              foreach (var doc in task.Result.Documents)
              {
                  string rutinaId = doc.Id;
                  string nombre = doc.GetValue<string>("nombre");

                  CrearRutinaUI(rutinaId, nombre);
              }
          });
    }

    void CrearRutinaUI(string rutinaId, string nombre)
    {
        GameObject item = Instantiate(rutinaPrefab, contenedorRutinas);

        RutinaItem ui = item.GetComponent<RutinaItem>();
        ui.Iniciar(rutinaId, nombre);
    }
}
