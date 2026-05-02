using UnityEngine;
using System.Text;
using System.IO;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ExportarPremium : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseFirestore db;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
    }

    public async void BotonDescargarTodoCSV()
    {
        if (auth.CurrentUser == null) return;

        Debug.Log("Iniciando exportación con estructura de Arrays...");
        string uid = auth.CurrentUser.UserId;
        StringBuilder csvContent = new StringBuilder();
        
        csvContent.AppendLine("Fecha;Rutina;Ejercicio;Series Totales;Detalle (Peso x Reps)");

        try
        {
            //ruta de la query
            QuerySnapshot entrenamientosSnapshot = await db.Collection("users").Document(uid)
                .Collection("entrenamientos").GetSnapshotAsync();

            foreach (DocumentSnapshot entDoc in entrenamientosSnapshot.Documents)
            {
                string fecha = entDoc.Id;
                
                string nombreRutina = entDoc.ContainsField("nombre") ? entDoc.GetValue<string>("nombre") : "Entrenamiento";

                QuerySnapshot ejerciciosSnapshot = await entDoc.Reference.Collection("ejercicios").GetSnapshotAsync();

                foreach (DocumentSnapshot ejerDoc in ejerciciosSnapshot.Documents)
                {
                    string nombreEjer = ejerDoc.Id;

                    int totalSeries = 0;
                    string detallePesoReps = "0x0";

                    if (ejerDoc.ContainsField("series"))
                    {
                        List<object> listaSeries = ejerDoc.GetValue<List<object>>("series");
                        totalSeries = listaSeries.Count;

                        List<string> setsCompletos = new List<string>();

                        foreach (object item in listaSeries)
                        {
                            Dictionary<string, object> setDatos = item as Dictionary<string, object>;
                            if (setDatos != null)
                            {
                                long kg = setDatos.ContainsKey("kg") ? (long)setDatos["kg"] : 0;
                                long reps = setDatos.ContainsKey("reps") ? (long)setDatos["reps"] : 0;

                                setsCompletos.Add($"{kg}x{reps}");
                            }
                        }

                        if (setsCompletos.Count > 0)
                        {
                            detallePesoReps = string.Join(" / ", setsCompletos);
                        }
                    }

                    csvContent.AppendLine($"{fecha};{nombreRutina};{nombreEjer};{totalSeries};{detallePesoReps}");
                }
            }

            FinalizarGuardado(csvContent.ToString());
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error en exportación: " + e.Message);
        }
    }

    private void FinalizarGuardado(string contenidoCompleto)
    {
        string nombreArchivo = "Reporte_Entrenamientos_Premium.csv";
        string ruta = Path.Combine(Application.persistentDataPath, nombreArchivo);

        File.WriteAllText(ruta, contenidoCompleto, Encoding.UTF8);
        Debug.Log($"<color=green>¡Éxito! CSV guardado en: {ruta}</color>");

        #if UNITY_EDITOR || UNITY_STANDALONE_WIN
            System.Diagnostics.Process.Start("explorer.exe", "/select," + ruta.Replace("/", "\\"));
        #endif
    }
}