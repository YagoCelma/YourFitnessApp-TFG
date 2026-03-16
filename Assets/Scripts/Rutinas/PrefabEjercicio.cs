using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class PrefabEjercicio : MonoBehaviour
{
    string ejercicioId;

    public void SetEjercicioId(string id)
    {
        ejercicioId = id;
    }

    public void BorrarEjercicio()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        string uid = auth.CurrentUser.UserId;

        if(string.IsNullOrEmpty(ejercicioId))
        {
            Destroy(transform.parent.gameObject);
            return;
        }

        db.Collection("users")
          .Document(uid)
          .Collection("rutinas")
          .Document(RutinaSeleccionada.rutinaId)
          .Collection("ejercicios")
          .Document(ejercicioId)
          .DeleteAsync();

        Destroy(transform.parent.gameObject);
    }

}