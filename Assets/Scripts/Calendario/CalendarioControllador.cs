using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Firestore;

public class CalendarioControlador : MonoBehaviour
{
    public Transform gridDias;
    public GameObject diaPrefab;
    public TMP_Text textoMes;
    public TMP_Text numeroDiasEntrenados;

    DateTime fechaActual;
    HashSet<int> diasEntrenados = new HashSet<int>();

    async void Start()
    {
        fechaActual = DateTime.Now;
        await CargarEntrenamientos();
        GenerarCalendario();
    }

    async Task CargarEntrenamientos()
    {
        diasEntrenados.Clear();

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        if (auth.CurrentUser == null)
        {
            Debug.Log("Usuario no logueado");
            return;
        }

        string uid = auth.CurrentUser.UserId;

        QuerySnapshot snapshot = await db.Collection("users")
            .Document(uid)
            .Collection("entrenamientos")
            .GetSnapshotAsync();

        int mes = fechaActual.Month;
        int ano = fechaActual.Year;

        foreach (var doc in snapshot.Documents)
        {
            DateTime fecha = DateTime.Parse(doc.Id);

            if (fecha.Month == mes && fecha.Year == ano)
            {
                diasEntrenados.Add(fecha.Day);
            }
        }

        ActualizarContador();
    }

    void GenerarCalendario()
    {
        foreach (Transform child in gridDias)
            Destroy(child.gameObject);

        int ano = fechaActual.Year;
        int mes = fechaActual.Month;

        textoMes.text = fechaActual.ToString("MMMM yyyy");

        DateTime primerDia = new DateTime(ano, mes, 1);
        int diasMes = DateTime.DaysInMonth(ano, mes);

        int offset = ((int)primerDia.DayOfWeek - 1 + 7) % 7;

        for (int i = 0; i < offset; i++)
            Instantiate(diaPrefab, gridDias);

        for (int dia = 1; dia <= diasMes; dia++)
        {
            GameObject nuevoDia = Instantiate(diaPrefab, gridDias);
            CalendarioDia cd = nuevoDia.GetComponent<CalendarioDia>();

            bool entrenado = diasEntrenados.Contains(dia);

            bool esHoy =
                dia == DateTime.Now.Day &&
                mes == DateTime.Now.Month &&
                ano == DateTime.Now.Year;

            cd.Configurar(dia, entrenado, esHoy);
        }
    }

    void ActualizarContador()
    {
        numeroDiasEntrenados.text = diasEntrenados.Count.ToString();
    }

    public async void MesSiguiente()
    {
        fechaActual = fechaActual.AddMonths(1);
        await CargarEntrenamientos();
        GenerarCalendario();
    }

    public async void MesAnterior()
    {
        fechaActual = fechaActual.AddMonths(-1);
        await CargarEntrenamientos();
        GenerarCalendario();
    }
}