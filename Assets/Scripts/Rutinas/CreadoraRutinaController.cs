using Firebase.Auth;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class CreadoraRutinaController : MonoBehaviour
{
    public TMP_InputField nombreRutinaInput;

    FirebaseFirestore db;
    FirebaseAuth auth;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        if (!string.IsNullOrEmpty(RutinaSeleccionada.rutinaId))
        {
            CargarRutina();
        }
    }

    void CargarRutina()
    {
        string uid = auth.CurrentUser.UserId;

        db.Collection("users")
          .Document(uid)
          .Collection("rutinas")
          .Document(RutinaSeleccionada.rutinaId)
          .GetSnapshotAsync()
          .ContinueWith(task =>
          {
              if (!task.Result.Exists) return;

              var data = task.Result.ToDictionary();
              nombreRutinaInput.text = data["nombre"].ToString();
          });
    }

    public void GuardarRutina()
    {
        string uid = auth.CurrentUser.UserId;
        string nombre = nombreRutinaInput.text;

        if (string.IsNullOrEmpty(nombre))
        {
            Debug.LogWarning("Nombre vacío");
            return;
        }

        Dictionary<string, object> datos = new()
        {
            { "nombre", nombre }
        };

        var rutinasRef = db.Collection("users")
                           .Document(uid)
                           .Collection("rutinas");


        if (string.IsNullOrEmpty(RutinaSeleccionada.rutinaId))
        {
            rutinasRef.AddAsync(datos).ContinueWith(task =>
            {
                RutinaSeleccionada.rutinaId = task.Result.Id;

                // Aquí ya existe la rutina
                Debug.Log("Rutina creada: " + RutinaSeleccionada.rutinaId);
            });
        }

        else
        {
            rutinasRef.Document(RutinaSeleccionada.rutinaId)
                      .SetAsync(datos);

            Debug.Log("Rutina actualizada");
        }
    }
}