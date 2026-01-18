using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseAuth Auth;
    public static FirebaseFirestore DB;

    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    Auth = FirebaseAuth.DefaultInstance;
                    DB = FirebaseFirestore.DefaultInstance;
                    Debug.Log("Firebase inicializado correctamente");
                }
                else
                {
                    Debug.LogError("Firebase no pudo inicializarse");
                }
            });
    }
}
