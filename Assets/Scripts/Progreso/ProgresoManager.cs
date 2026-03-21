using UnityEngine;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Auth;
using System.Linq;

public class ProgresoManager : MonoBehaviour
{
    public GraficaManual miGrafica; // Referencia al script de arriba
    public TMPro.TMP_Text txtMax, txtMin, txtAvg;

    void OnEnable()
    {
        CargarDatosPeso();
    }

    public void CargarDatosPeso()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        db.Collection("users").Document(uid).Collection("pesoHistorial")
          .OrderBy("timestamp")
          .GetSnapshotAsync().ContinueWith(task => {
              
              if (task.IsFaulted) return;

              List<float> pesos = new List<float>();

              foreach (DocumentSnapshot doc in task.Result.Documents)
              {
                  float p = float.Parse(doc.GetValue<object>("peso").ToString());
                  pesos.Add(p);
              }

              // Usamos el Dispatcher para volver al hilo principal
              UnityMainThreadDispatcher.Instance().Enqueue(() => {
                  ActualizarEstadisticas(pesos);
                  miGrafica.Dibujar(pesos);
              });
          });
    }

    void ActualizarEstadisticas(List<float> pesos)
    {
        if (pesos.Count == 0) return;
        txtMax.text = pesos.Max().ToString("F1") + " kg";
        txtMin.text = pesos.Min().ToString("F1") + " kg";
        txtAvg.text = pesos.Average().ToString("F1") + " kg";
    }
}