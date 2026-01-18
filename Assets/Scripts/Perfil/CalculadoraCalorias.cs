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
    //public PopupAdvertencia popup;
    public TextMeshProUGUI caloriasTexto;
    public TextMeshProUGUI caloriasBasalesTexto;
    


    public void calcularTodo()
    {
        double caloriasBasales = calcularCaloriasBasalesSexo();

        double caloriasActividad = calcularCaloriasActividad(caloriasBasales);

        double caloriasFinales = calcularCaloriasFinales(caloriasActividad);

        //Redondeamos las calorias para que no tengan decimales
        int caloriasBasalesEnteras = Mathf.RoundToInt((float) caloriasBasales);
        int caloriasFinalesEnteras = Mathf.RoundToInt((float) caloriasFinales);

        mostrarCalorias(caloriasBasalesEnteras, caloriasFinalesEnteras);

    }
    public double calcularCaloriasBasalesSexo()
    {
        //Parseamos los valores
        int alturaNumber = int.Parse(altura.text);
        double pesoNumber = double.Parse(peso.text);
        int edadNumber = int.Parse(edad.text);

        double tmb;

        Toggle sexoSeleccionado = sexoGrupo.GetFirstActiveToggle();

        if(sexoSeleccionado.name == "Toggle_Hombre")
        {
            tmb = (10 * pesoNumber) + (6.25 * alturaNumber) - (5 * edadNumber) + 5;

            return tmb;

        }else if(sexoSeleccionado.name == "Toggle_Mujer")
        {
            tmb = (10 * pesoNumber) + (6.25 * alturaNumber) - (5 * edadNumber) - 161;

            return tmb;
        }
        else
        {
            //popup.Mostrar("Error", "No se ha seleccionado ningun nivel de actividad");
            return 0;
        }


    }

    public double calcularCaloriasActividad(double tmb)
    {
        double tdee;
        Toggle actividadSeleccionada = actividadGrupo.GetFirstActiveToggle();
        
        if(actividadSeleccionada.name == "Toggle_Bajo")
        {
            return tdee = tmb * 1.375;
        }else if(actividadSeleccionada.name == "Toggle_Medio")
        {
            return tdee = tmb * 1.55;
        }else if (actividadSeleccionada.name == "Toggle_Alto")
        {
            return tdee = tmb * 1.725;
        }
        else
        {
            //popup.Mostrar("Error", "No se ha seleccionado ningun nivel de actividad");
            return 0;
        }
        
    }

    public double calcularCaloriasFinales(double calorias)
    {
        double caloriasFinales;
        Toggle objetivoSeleccionado = objetivoGrupo.GetFirstActiveToggle();

        if(objetivoSeleccionado.name == "Toggle_Definicion")
        {
            return caloriasFinales = calorias * 0.85;
        }else if (objetivoSeleccionado.name == "Toggle_Mantenimiento")
        {
            return calorias;
        }else if (objetivoSeleccionado.name == "Toggle_Volumen")
        {
            return caloriasFinales = calorias * 1.10;
        }
        else
        {
            //popup.Mostrar("Error", "No se ha seleccionado ningun objetivo");
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
}
