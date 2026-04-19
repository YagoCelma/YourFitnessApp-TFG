using System;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculadoraCalorias : MonoBehaviour
{
    public TMP_InputField altura;
    public TMP_InputField peso;
    public TMP_InputField edad;
    public ToggleGroup sexoGrupo;
    public ToggleGroup actividadGrupo;
    public ToggleGroup objetivoGrupo;
    public TextMeshProUGUI mensajeError;
    public TextMeshProUGUI caloriasTexto;
    public TextMeshProUGUI caloriasBasalesTexto;
    

    FirebaseAuth auth;
    FirebaseFirestore db;
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
    }

    public void calcularTodo()
    {
        if (altura == null || peso == null || edad == null || sexoGrupo == null || actividadGrupo == null || objetivoGrupo == null)
        {
            mensajeError.text = "Error: No puede haber ningun campo vacio";
            return;
        }
        
        double caloriasBasales = calcularCaloriasBasalesSexo();

        double caloriasActividad = calcularCaloriasActividad(caloriasBasales);

        double caloriasFinales = calcularCaloriasFinales(caloriasActividad);

        //Redondeamos las calorias para que no tengan decimales
        int caloriasBasalesEnteras = Mathf.RoundToInt((float) caloriasBasales);
        int caloriasFinalesEnteras = Mathf.RoundToInt((float) caloriasFinales);

        mostrarCalorias(caloriasBasalesEnteras, caloriasFinalesEnteras);

        double pesoDouble = Double.Parse(peso.text);

        guardarPeso(pesoDouble, caloriasFinalesEnteras);

    }
    public double calcularCaloriasBasalesSexo()
    {
        //Parseamos los valores
        int alturaNumber = int.Parse(altura.text);
        double pesoNumber = double.Parse(peso.text);
        int edadNumber = int.Parse(edad.text);

        double tmb;

        Toggle sexoSeleccionado = sexoGrupo.GetFirstActiveToggle();

        if(sexoSeleccionado.name == "Hombre")
        {
            tmb = (10 * pesoNumber) + (6.25 * alturaNumber) - (5 * edadNumber) + 5;

            return tmb;

        }else if(sexoSeleccionado.name == "Mujer")
        {
            tmb = (10 * pesoNumber) + (6.25 * alturaNumber) - (5 * edadNumber) - 161;

            return tmb;
        }
        else
        {
            mensajeError.text = "Error: No se ha seleccionado el sexo";
            return 0;
        }


    }

    public double calcularCaloriasActividad(double tmb)
    {
        double tdee;
        Toggle actividadSeleccionada = actividadGrupo.GetFirstActiveToggle();
        
        if(actividadSeleccionada.name == "Bajo")
        {
            return tdee = tmb * 1.375;
        }else if(actividadSeleccionada.name == "Medio")
        {
            return tdee = tmb * 1.55;
        }else if (actividadSeleccionada.name == "Alto")
        {
            return tdee = tmb * 1.725;
        }
        else
        {
            mensajeError.text = "Error: No se ha seleccionado ningun nivel de actividad";
            return 0;
        }
        
    }

    public double calcularCaloriasFinales(double calorias)
    {
        double caloriasFinales;
        Toggle objetivoSeleccionado = objetivoGrupo.GetFirstActiveToggle();

        if(objetivoSeleccionado.name == "Definicion")
        {
            return caloriasFinales = calorias * 0.85;
        }else if (objetivoSeleccionado.name == "Mantenimiento")
        {
            return calorias;
        }else if (objetivoSeleccionado.name == "Volumen")
        {
            return caloriasFinales = calorias * 1.10;
        }
        else
        {
            mensajeError.text ="Error: No se ha seleccionado ningun objetivo";
            return 0;
        }
    }

    public void mostrarCalorias(double calorias, double caloriasBasales)
    {
        string caloriasText = calorias.ToString();
        caloriasTexto.text = caloriasText;

        string caloriasBasalesTextoD = caloriasBasales.ToString();
        caloriasBasalesTexto.text = caloriasBasalesTextoD;
    }

    public void guardarPeso(double peso, int calorias)
    {

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        if (auth.CurrentUser == null)
        {
            Debug.LogError("No hay ningun usuario iniciado");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        Debug.Log("UID REAL = " + uid);

        Dictionary<string, object> datosPeso = new Dictionary<string, object>
        {
            { "calorias totales", calorias},
            {"peso", peso},
            { "timestamp", Timestamp.GetCurrentTimestamp() }
        };

        db.Collection("users")
          .Document(uid)
          .Collection("pesoHistorial")
          .AddAsync(datosPeso)
          .ContinueWith(task =>
          {
              if (task.IsFaulted)
              {
                  Debug.LogError("Error al guardar peso: " + task.Exception);
                  mensajeError.text = "Error: No se ha podido guardar el peso";
              }
              else
              {
                  Debug.Log("Peso guardado correctamente");
              }
          });


        Dictionary<string, object> perfil = new()
        {
            {"altura", int.Parse(altura.text)},
            { "edad", int.Parse(edad.text)},
            {"sexo", sexoGrupo.GetFirstActiveToggle().name},
            {"actividad", actividadGrupo.GetFirstActiveToggle().name},
            {"objetivo", objetivoGrupo.GetFirstActiveToggle().name}
        };

        db.Collection("users")
        .Document(uid)
        .Collection("perfil")
        .Document("datos")
        .SetAsync(perfil)
        .ContinueWith(task=>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error al guardar los datos de perfil " + task.Exception);
            }
            else
            {
                Debug.Log("Datos de perfil guardados correctamente");
            }
        });

    }
}