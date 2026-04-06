using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Firestore;

public class RutinaItem : MonoBehaviour
{
    public TMP_Text nombreRutina;

    private string rutinaId;

    public void Iniciar(string id, string nombre)
    {
        rutinaId = id;

        if (nombreRutina != null)
            nombreRutina.text = nombre;
    }

    public void OnClickEjercicio()
    {
        RutinaSeleccionada.rutinaId = rutinaId;
        SceneManager.LoadScene(3);
    }
    public void OnClick()
    {
        RutinaSeleccionada.rutinaId = rutinaId;
        SceneManager.LoadScene(9);
    }

    public async void BorrarRutina()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        if (auth.CurrentUser == null)
        {
            Debug.Log("No hay usuario logueado");
            return;
        }

        string uid = auth.CurrentUser.UserId;

        await db.Collection("users")
            .Document(uid)
            .Collection("rutinas")
            .Document(rutinaId)
            .DeleteAsync();

        Destroy(gameObject);

        Debug.Log("Rutina borrada");
    }
    
}
