using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Firebase.Auth;
using Firebase.Firestore;
using System.Collections.Generic;


public class ManagerRutina : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject panelEjercicioPrefab;

    [Header("UI")]
    public Transform content;

    public void AgregarEjercicio()
    {
        GameObject nuevoEjercicio = Instantiate(panelEjercicioPrefab, content, false);

        nuevoEjercicio.transform.SetAsLastSibling();

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)content);
    }

    public void EliminarEjercicio()
    {
        Destroy(gameObject);
    }

    public void BotonAtras()
    {
        SceneManager.LoadScene(7);
    }

    public async void RegistrarEntrenamiento()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        

        if (auth.CurrentUser == null)
        {
            Debug.Log("Usuario no logueado");
            return;
        }

        string uid = auth.CurrentUser.UserId;

        string fecha = DateTime.Now.ToString("yyyy-MM-dd");

        Dictionary<string, object> datos = new Dictionary<string, object>();

        await db.Collection("users")
            .Document(uid)
            .Collection("entrenamientos")
            .Document(fecha)
            .SetAsync(datos);

        Debug.Log("Entrenamiento guardado " + fecha);
        
        SceneManager.LoadScene(7);
    }

 
}
