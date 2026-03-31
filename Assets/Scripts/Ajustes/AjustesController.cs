using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AjustesController : MonoBehaviour
{
    public TMP_Text textoCorreo;
    public TMP_Text textoUsuario;
    public void CerrarSesion()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        
        Debug.Log("Sesión cerrada");
        SceneManager.LoadScene(0);
    }
}