using UnityEngine;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Auth;
using System.Linq;
using System;

public class ProgresoManager : MonoBehaviour
{
    public GraficaManual miGrafica;
    public TMPro.TMP_Text txtMax, txtMin, txtAvg;

    private void OnEnable()
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
              List<DateTime> fechas = new List<DateTime>();  // ✅ NUEVO

              foreach (DocumentSnapshot doc in task.Result.Documents)
              {
                  float p = float.Parse(doc.GetValue<object>("peso").ToString());
                  pesos.Add(p);

                  // ✅ Extraer la fecha
                  Timestamp timestamp = doc.GetValue<Timestamp>("timestamp");
                  fechas.Add(timestamp.ToDateTime());
              }

              UnityMainThreadDispatcher.Instance().Enqueue(() => {
                  ActualizarEstadisticas(pesos);
                  miGrafica.Dibujar(pesos, fechas);  // ✅ Pasar fechas
              });
          });
    }

    private void ActualizarEstadisticas(List<float> pesos)
    {
        if (pesos.Count == 0) return;
        txtMax.text = pesos.Max().ToString("F1") + " kg";
        txtMin.text = pesos.Min().ToString("F1") + " kg";
        txtAvg.text = pesos.Average().ToString("F1") + " kg";
    }
}