using UnityEngine;
using TMPro;

public class FilaSerie : MonoBehaviour
{
    [Header("Inputs")]
    public TMP_InputField inputSerie;
    public TMP_InputField inputKG;
    public TMP_InputField inputReps;

    public void SetNumeroSerie(int numero)
    {
        inputSerie.text = numero.ToString();
    }


}
