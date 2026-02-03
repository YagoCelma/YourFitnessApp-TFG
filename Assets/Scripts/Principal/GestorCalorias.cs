using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Linq;


public class GestorCalorias : MonoBehaviour
{
    public TMP_Text textoCalorias;

    FirebaseFirestore db;
    FirebaseAuth auth;

    async void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        await CargarUltimasCalorias();
    }

    public async Task CargarUltimasCalorias()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogWarning("No hay ningún usuario logeado");
            return;
        }

        string uid = auth.CurrentUser.UserId;

        QuerySnapshot snapshot = await db
            .Collection("users")
            .Document(uid)
            .Collection("pesoHistorial")
            .OrderByDescending("timestamp")
            .Limit(1)
            .GetSnapshotAsync();

        if (snapshot.Count == 0)
        {
            Debug.LogWarning("No hay registros de calorías");
            return;
        }

        DocumentSnapshot doc = snapshot.Documents.FirstOrDefault();
        if (doc == null)
        {
            Debug.LogWarning("No hay documento (FirstOrDefault devuelve null)");
            return;
        }


        if (!doc.Exists || !doc.ContainsField("calorias totales"))
        {
            Debug.LogWarning("El documento no tiene calorias_totales");
            return;
        }

        long calorias = doc.GetValue<long>("calorias totales");

        textoCalorias.text = calorias + " kcal";
    }
}
