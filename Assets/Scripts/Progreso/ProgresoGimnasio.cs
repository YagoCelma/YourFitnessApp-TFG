using UnityEngine;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Auth;
using XCharts.Runtime;
using System.Linq;

public class ProgresoGimnasio : MonoBehaviour
{
    public LineChart miGrafica;
    public TMPro.TMP_Text txtMax, txtMin, txtAvg;

    void OnEnable()
    {
        CargarDatos();
    }

    void CargarDatos()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        db.Collection("users").Document(uid).Collection("pesoHistorial")
          .OrderBy("timestamp")
          .GetSnapshotAsync().ContinueWith(task => {
              
              if (task.IsFaulted) {
                  Debug.LogError("Error en Firebase");
                  return;
              }

              List<float> pesos = new List<float>();
              List<System.DateTime> fechas = new List<System.DateTime>();

              foreach (DocumentSnapshot doc in task.Result.Documents)
              {
                  float p = float.Parse(doc.GetValue<object>("peso").ToString());
                  Timestamp ts = doc.GetValue<Timestamp>("timestamp");
                  
                  pesos.Add(p);
                  fechas.Add(ts.ToDateTime());
              }

              UnityMainThreadDispatcher.Instance().Enqueue(() => {
                  Dibujar(pesos, fechas);
              });
          });
    }

    void Dibujar(List<float> pesos, List<System.DateTime> fechas)
    {
        if (pesos.Count == 0) return;

        txtMax.text = pesos.Max().ToString("F1") + " kg";
        txtMin.text = pesos.Min().ToString("F1") + " kg";
        txtAvg.text = pesos.Average().ToString("F1") + " kg";

        miGrafica.RemoveData(); 
    for (int i = 0; i < pesos.Count; i++)
    {
        long ms = new System.DateTimeOffset(fechas[i]).ToUnixTimeMilliseconds();
        
        miGrafica.AddData(0, pesos[i], ms); 
    }
    }
}