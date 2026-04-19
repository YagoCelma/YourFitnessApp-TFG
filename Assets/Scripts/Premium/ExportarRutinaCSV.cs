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

        Debug.Log("Iniciando exportación con formato personalizado...");
        string uid = auth.CurrentUser.UserId;
        StringBuilder csvContent = new StringBuilder();
        
        csvContent.AppendLine("Fecha;Rutina;Ejercicio;Serie;Peso (kg);Reps");

        try
        {
            QuerySnapshot entrenamientosSnapshot = await db.Collection("users").Document(uid)
                .Collection("entrenamientos").GetSnapshotAsync();

            foreach (DocumentSnapshot entrenamientoDoc in entrenamientosSnapshot.Documents)
            {
                string fecha = entrenamientoDoc.Id;  // La fecha es el ID del documento

                QuerySnapshot ejerciciosSnapshot = await db.Collection("users").Document(uid)
                    .Collection("entrenamientos").Document(fecha)
                    .Collection("ejercicios").GetSnapshotAsync();

                foreach (DocumentSnapshot ejerDoc in ejerciciosSnapshot.Documents)
                {
                    string nombreEjer = ejerDoc.Id; 

                    try
                    {
                        List<object> seriesList = ejerDoc.GetValue<List<object>>("series");

                        if (seriesList != null && seriesList.Count > 0)
                        {
                            int numeroSerie = 1;
                            foreach (object serie in seriesList)
                            {
                                var serieDic = serie as Dictionary<string, object>;
                                if (serieDic != null)
                                {
                                    int kg = 0;
                                    int reps = 0;

                                    if (serieDic.ContainsKey("kg"))
                                        int.TryParse(serieDic["kg"].ToString(), out kg);
                                    if (serieDic.ContainsKey("reps"))
                                        int.TryParse(serieDic["reps"].ToString(), out reps);

                                    // Añadir fila al CSV
                                    csvContent.AppendLine($"{fecha};{nombreEjer};{nombreEjer};{numeroSerie};{reps};{kg}");
                                    numeroSerie++;
                                }
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning($"Error procesando {nombreEjer}: {e.Message}");
                    }
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
        Debug.Log($"<color=gold>¡Hecho! Archivo guardado en: {ruta}</color>");

        #if UNITY_EDITOR || UNITY_STANDALONE_WIN
            System.Diagnostics.Process.Start("explorer.exe", "/select," + ruta.Replace("/", "\\"));
        #endif
    }
}