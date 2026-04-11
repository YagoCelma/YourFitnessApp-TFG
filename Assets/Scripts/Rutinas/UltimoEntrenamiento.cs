using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class UltimoEntrenamiento : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI textoUltimoEntrenamiento; // Arrastra aquí el texto de dentro del panel
    public Button botonVerDetalles;
    public GameObject panelDetalles;

    private string nombreRutinaActual;
    private List<string> nombresEjerciciosRutina = new List<string>();
    private bool cargandoDatosDeRutina = true;

    void Start()
    {
        if (panelDetalles != null) panelDetalles.SetActive(false);
        if (botonVerDetalles != null)
            botonVerDetalles.onClick.AddListener(CargarUltimoEntrenamiento);

        ObtenerNombreRutinaActual();
    }

    void ObtenerNombreRutinaActual()
    {
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        if (string.IsNullOrEmpty(RutinaSeleccionada.rutinaId)) return;

        db.Collection("users").Document(uid).Collection("rutinas").Document(RutinaSeleccionada.rutinaId)
            .GetSnapshotAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) return;
                nombreRutinaActual = task.Result.GetValue<string>("nombre");
                CargarListaDeEjercicios(uid, db);
            });
    }

    void CargarListaDeEjercicios(string uid, FirebaseFirestore db)
    {
        db.Collection("users").Document(uid).Collection("rutinas").Document(RutinaSeleccionada.rutinaId).Collection("ejercicios")
            .GetSnapshotAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) return;
                nombresEjerciciosRutina.Clear();
                foreach (var doc in task.Result.Documents)
                {
                    nombresEjerciciosRutina.Add(doc.GetValue<string>("nombre"));
                }
                cargandoDatosDeRutina = false;
            });
    }

    public void CargarUltimoEntrenamiento()
    {
        if (cargandoDatosDeRutina) return;

        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        db.Collection("users").Document(uid).Collection("entrenamientos")
            .GetSnapshotAsync().ContinueWithOnMainThread(async task => {
                if (task.IsFaulted) return;

                var documentos = task.Result.Documents.OrderByDescending(d => d.Id).ToList();
                DocumentSnapshot entrenamientoEncontrado = null;

                foreach (var docEntrenamiento in documentos)
                {
                    var ejerciciosSnapshot = await docEntrenamiento.Reference.Collection("ejercicios").GetSnapshotAsync();
                    bool coincidencia = ejerciciosSnapshot.Documents.Any(d => nombresEjerciciosRutina.Contains(d.Id));

                    if (coincidencia)
                    {
                        entrenamientoEncontrado = docEntrenamiento;
                        break;
                    }
                }

                if (entrenamientoEncontrado == null)
                {
                    textoUltimoEntrenamiento.text = $"<align=center><size=140%><b>{nombreRutinaActual}</b></size>\n\n<size=70%>No hay historial registrado todavía.</size></align>";
                    if (panelDetalles != null) panelDetalles.SetActive(true);
                }
                else
                {
                    MostrarDetalles(uid, db, entrenamientoEncontrado.Id);
                }
            });
    }

    void MostrarDetalles(string uid, FirebaseFirestore db, string fechaId)
    {
        db.Collection("users").Document(uid).Collection("entrenamientos").Document(fechaId).Collection("ejercicios")
            .GetSnapshotAsync().ContinueWithOnMainThread(task => {
                
                // Formato de cabecera centrado
                string texto = $"<align=center><size=180%><b>{nombreRutinaActual}</b></size>\n";
                texto += $"<color=#888888><size=90%>Sesión: {fechaId}</size></color></align>\n\n";

                foreach (var doc in task.Result.Documents)
                {
                    if (!nombresEjerciciosRutina.Contains(doc.Id)) continue;

                    texto += $"<size=130%><color=orange><b>• {doc.Id.ToUpper()}</b></color></size>\n";
                    
                    if (doc.ContainsField("series"))
                    {
                        var series = doc.GetValue<List<object>>("series");
                        List<string> formatoSeries = new List<string>();
                        foreach (var s in series)
                        {
                            var d = s as Dictionary<string, object>;
                            formatoSeries.Add($"<size=130%>{d["reps"]}kg x {d["kg"]}</size>");
                        }
                        texto += $"<indent=5%>{string.Join(" | ", formatoSeries)}</indent>\n\n";
                    }
                }

                textoUltimoEntrenamiento.text = texto;
                if (panelDetalles != null) panelDetalles.SetActive(true);
            });
    }

    public void CerrarPanel()
{
    if (panelDetalles != null)
    {
        panelDetalles.SetActive(false);
    }
}
}