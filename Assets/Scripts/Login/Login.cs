using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Extensions;

public class Login : MonoBehaviour
{

    public TMP_InputField correoInput;
    public TMP_InputField contrasenhaInput;
    public PopupAdvertencia popup;

    /*void Start()
    {
        if(FirebaseManager.Auth.CurrentUser != null)
        {
            Debug.Log("Sesión ya Iniciada");
            //Poner aqui para pasar ya al menu principal sin el login
        }
    }*/
    public void LoginUsuario()
    {

        string correo = correoInput.text;
        string contrasenha = contrasenhaInput.text;


        if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenha))
        {

            Debug.Log("Error: Correo o contraseña vacios");
            popup.Mostrar("Error", "Correo o contraseña vacios");
            return;
        }

        FirebaseManager.Auth
            .SignInWithEmailAndPasswordAsync(correo, contrasenha)
            .ContinueWithOnMainThread(task =>
            {
                if (!task.IsCompletedSuccessfully)
                {
                    Debug.Log("Correo o contraseña incorrectos");
                    popup.Mostrar("Error", "Correo o contraseña incorrectos");
                    return;
                }

                Debug.Log("Login correcto");

                //Aqui despues llevarlo a la pagina principal
                SceneManager.LoadScene(2);


            });

    }

    public void RegistrarUsuario()
    {

        SceneManager.LoadScene(1);
    }


}
