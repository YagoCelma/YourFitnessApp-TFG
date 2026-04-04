using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AjustesController : MonoBehaviour
{
    public TMP_Text nombreUsuarioText;
    public TMP_Text correoText;

    void Start()
    {
        CargarDatosUsuario();
    }

    void CargarDatosUsuario()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        if (auth.CurrentUser == null)
        {
            Debug.Log("Usuario no logueado");
            return;
        }

        string uid = auth.CurrentUser.UserId;

        db.Collection("users")
            .Document(uid)
            .GetSnapshotAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (!task.IsCompletedSuccessfully)
                {
                    Debug.LogError("Error al cargar datos: " + task.Exception);
                    return;
                }

                DocumentSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    string nombre = snapshot.GetValue<string>("nombre");
                    string apellidos = snapshot.GetValue<string>("apellidos");
                    string correo = snapshot.GetValue<string>("correo");
                    string nombreUsuario = snapshot.GetValue<string>("nombreUsuario");

                    nombreUsuarioText.text = nombreUsuario;
                    correoText.text = correo;

                    Debug.Log($"Usuario: {nombreUsuario}, Email: {correo}");
                }
                else
                {
                    Debug.Log("El documento del usuario no existe");
                }
            });
    }

    public void CerrarSesion()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        
        Debug.Log("Sesión cerrada");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
    }
}