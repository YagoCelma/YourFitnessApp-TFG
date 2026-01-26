using TMPro;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System.Collections;

public class Registro : MonoBehaviour
{
    public TMP_InputField nombreInput;
    public TMP_InputField apellidosInput;
    public TMP_InputField nombreUsuarioInput;
    public TMP_InputField correoInput;
    public TMP_InputField contrasenhaInput;
    public TMP_InputField confirmarContrasenhaInput;
    public PopupAdvertencia popup;


    public void RegistrarUsuario()
    {
        string nombre = nombreInput.text;
        string apellidos = apellidosInput.text;
        string nombreUsuario = nombreUsuarioInput.text;
        string correo = correoInput.text;
        string contrasenha = contrasenhaInput.text;
        string confirmarContrasenha = confirmarContrasenhaInput.text;




        if (contrasenha != confirmarContrasenha)
        {
            Debug.LogError("Las contraseñas no coinciden");
            popup.Mostrar("Error", "Las contraseñas no coinciden");
            return;
        }

        if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenha) || string.IsNullOrEmpty(nombre)
                || string.IsNullOrEmpty(apellidos) || string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(confirmarContrasenha))
        {
            Debug.LogError("Ningun campo puede estar vacio");
            popup.Mostrar("Error", "Ningun campo puede estar vacio");
            return;
        }

        if (contrasenha.Length < 8)
        {
            Debug.LogError("Error la contraseña debe contener un minimo de 8 caracteres");
            popup.Mostrar("Error", "La contraseña debe tener minimo 8 caracteres");
            return;
        }

        bool mayusculas = false;
        bool minusculas = false;
        bool numero = false;

        foreach (char c in contrasenha)
        {
            if (char.IsUpper(c)) mayusculas = true;
            if (char.IsLower(c)) minusculas = true;
            if (char.IsNumber(c)) numero = true;
        }


        if (!mayusculas || !minusculas)
        {
            Debug.Log("La contraseña debe contener mayusculas y minusculas");
            popup.Mostrar("Error", "La contraseña debe contener mayusculas y minusculas");
            return;

        }

        if (!numero)
        {
            Debug.Log("La conraseña debe tener valores numericos");
            popup.Mostrar("Error", "La conraseña debe tener valores numericos");
            return;
        }

        FirebaseManager.Auth
            .CreateUserWithEmailAndPasswordAsync(correo, contrasenha)
            .ContinueWithOnMainThread(task =>
            {
                if (!task.IsCompletedSuccessfully)
                {

                    Debug.LogError(task.Exception);
                    return;
                }

                string uid = FirebaseManager.Auth.CurrentUser.UserId;

                FirebaseManager.DB
                    .Collection("usuarios")
                    .Document(uid)
                    .SetAsync(new
                    {
                        nombre = nombre,
                        apellidos = apellidos,
                        nombreUsuario = nombreUsuario,
                        correo = correo,
                        creado = Timestamp.GetCurrentTimestamp()
                    });

                Debug.Log("Usuario registrado correctamente");
            });
    }

    public void VolverAtras()
    {
        SceneManager.LoadScene(0);
    }
}
