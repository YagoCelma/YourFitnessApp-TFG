using Firebase.Auth;
using Firebase.Firestore;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GestorPrincipal : MonoBehaviour
{
    [Header("Texto agua")]
    public TMP_Text cantidadAgua;

    FirebaseAuth auth;
    FirebaseFirestore db;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        CalcularCantidadAgua();
    }

    private async void CalcularCantidadAgua()
{
    if (auth.CurrentUser == null) return;

    string uid = auth.CurrentUser.UserId;

    try
    {
        QuerySnapshot snapshot = await db.Collection("users").Document(uid)
                                         .Collection("pesoHistorial")
                                         .OrderByDescending("timestamp") 
                                         .Limit(1)
                                         .GetSnapshotAsync();

        if (snapshot.Count > 0)
        {
            DocumentSnapshot docReciente = null;

            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                docReciente = doc;
                break; 
            }

            if (docReciente != null && docReciente.ContainsField("peso"))
            {
                float pesoActual = System.Convert.ToSingle(docReciente.GetValue<object>("peso"));

                float metaMililitros = pesoActual * 35f;
                float metaLitros = metaMililitros / 1000f;

                cantidadAgua.text = $"{metaLitros.ToString("F1")} L";
            }
        }
        else
        {
            cantidadAgua.text = "3L";
            Debug.LogWarning("El usuario no tiene el peso registrado. Usando valor por defecto.");
        }
    }
    catch (System.Exception e)
    {
        Debug.LogError("Error al calcular el agua: " + e.Message);
    }
}
    public void Ejercicio()
    {
        SceneManager.LoadScene(11);
    }

    public void Rutinas()
    {
        SceneManager.LoadScene(7);
    }

    public void Progreso()
    {
        SceneManager.LoadScene(5);
    }

    public void Alarmas()
    {
        SceneManager.LoadScene(6);
    }

    public void Perfil()
    {
        SceneManager.LoadScene(8);
    }

    public void Calendario()
    {
        SceneManager.LoadScene(10);
    }

    public void Ajustes()
    {
        SceneManager.LoadScene(12);
    }
}
