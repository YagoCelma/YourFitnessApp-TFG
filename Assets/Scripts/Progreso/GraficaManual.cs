using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System;

public class GraficaManual : MonoBehaviour
{
    [Header("Configuración Visual")]
    public RectTransform contenedor;
    public GameObject puntoPrefab;
    public Color colorLinea = new Color(1f, 0.4f, 0f, 1f);
    public Color colorEjes = new Color(0.5f, 0.5f, 0.5f, 0.3f);
    public float grosorLinea = 4f;
    public float grosorEje = 2f;

    [Header("Etiquetas")]
    public TextMeshProUGUI prefabEtiqueta;

    private float minPeso;
    private float maxPeso;
    private List<float> pesos;
    private List<DateTime> fechas;
    private List<GameObject> puntos = new List<GameObject>();
    private TextMeshProUGUI etiquetaActual;

    public void Dibujar(List<float> pesosList, List<DateTime> fechasRegistro = null)
    {
        foreach (Transform child in contenedor)
        {
            if (child.name != "Fondo" && child.name != "EjeX" && child.name != "EjeY" && child.name != "Etiqueta")
                Destroy(child.gameObject);
        }

        puntos.Clear();

        if (pesosList == null || pesosList.Count < 2) return;

        float anchoTotalContenedor = contenedor.rect.width;
        float altoTotalContenedor = contenedor.rect.height;

        float paddingIzquierda = 50f;
        float paddingDerecha = 50f;
        float paddingArriba = 40f;
        float paddingAbajo = 40f;

        float anchoTotal = anchoTotalContenedor - paddingIzquierda - paddingDerecha;
        float altoTotal = altoTotalContenedor - paddingArriba - paddingAbajo;

        float offsetX = paddingIzquierda;
        float offsetY = paddingAbajo;

        pesos = pesosList;
        minPeso = pesos.Min() - 5;
        maxPeso = pesos.Max() + 5;
        float rangoPeso = maxPeso - minPeso;

        fechas = fechasRegistro ?? new List<DateTime>();

        DibujarEjes(anchoTotal, altoTotal, offsetX, offsetY);

        Vector2 ultimoPuntoPos = Vector2.zero;

        for (int i = 0; i < pesos.Count; i++)
        {
            float xPos = offsetX + (i / (float)(pesos.Count - 1)) * anchoTotal;
            float yPos = offsetY + ((pesos[i] - minPeso) / rangoPeso) * altoTotal;
            Vector2 posicionActual = new Vector2(xPos, yPos);

            if (i > 0)
            {
                CrearLinea(ultimoPuntoPos, posicionActual);
            }

            GameObject punto = CrearPunto(posicionActual, i);
            puntos.Add(punto);

            ultimoPuntoPos = posicionActual;
        }
    }

    private GameObject CrearPunto(Vector2 pos, int indice)
    {
        GameObject dot = Instantiate(puntoPrefab, contenedor);
        RectTransform rt = dot.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.anchorMin = rt.anchorMax = Vector2.zero;

        Button btn = dot.GetComponent<Button>();
        if (btn == null) btn = dot.AddComponent<Button>();

        int indiceLocal = indice;
        btn.onClick.AddListener(() => MostrarEtiqueta(pos, indiceLocal));

        return dot;
    }

    private void MostrarEtiqueta(Vector2 pos, int indice)
    {
        if (etiquetaActual != null)
            Destroy(etiquetaActual.gameObject);

        TextMeshProUGUI etiqueta = Instantiate(prefabEtiqueta, contenedor);
        etiqueta.name = "Etiqueta";

        string texto = pesos[indice].ToString("F1") + " kg";
        if (indice < fechas.Count && fechas[indice] != default)
        {
            texto += "\n" + fechas[indice].ToString("dd/MM");
        }

        etiqueta.text = texto;
        etiqueta.fontSize = 30;
        etiqueta.alignment = TextAlignmentOptions.Bottom;
        etiqueta.color = new Color(255f, 255f, 255f, 255f);

        RectTransform rt = etiqueta.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
        rt.pivot = new Vector2(0.5f, 0f);

        rt.anchoredPosition = new Vector2(pos.x, pos.y + 25);
        rt.sizeDelta = new Vector2(100, 40);

        etiquetaActual = etiqueta;
    }

    private void DibujarEjes(float anchoTotal, float altoTotal, float offsetX, float offsetY)
    {
        GameObject ejeYGO = new GameObject("EjeY", typeof(Image));
        ejeYGO.transform.SetParent(contenedor, false);
        Image ejeYImg = ejeYGO.GetComponent<Image>();
        ejeYImg.color = colorEjes;
        ejeYImg.raycastTarget = false;

        RectTransform ejeYRT = ejeYGO.GetComponent<RectTransform>();
        ejeYRT.anchorMin = ejeYRT.anchorMax = Vector2.zero;
        ejeYRT.sizeDelta = new Vector2(grosorEje, altoTotal);
        ejeYRT.anchoredPosition = new Vector2(offsetX, offsetY + altoTotal * 0.5f);

        // Eje X (horizontal)
        GameObject ejeXGO = new GameObject("EjeX", typeof(Image));
        ejeXGO.transform.SetParent(contenedor, false);
        Image ejeXImg = ejeXGO.GetComponent<Image>();
        ejeXImg.color = colorEjes;
        ejeXImg.raycastTarget = false;

        RectTransform ejeXRT = ejeXGO.GetComponent<RectTransform>();
        ejeXRT.anchorMin = ejeXRT.anchorMax = Vector2.zero;
        ejeXRT.sizeDelta = new Vector2(anchoTotal, grosorEje);
        ejeXRT.anchoredPosition = new Vector2(offsetX + anchoTotal * 0.5f, offsetY);
    }

    private void CrearLinea(Vector2 start, Vector2 end)
    {
        GameObject lineaGO = new GameObject("SegmentoLinea", typeof(Image));
        lineaGO.transform.SetParent(contenedor, false);
        Image img = lineaGO.GetComponent<Image>();
        img.color = colorLinea;
        img.raycastTarget = false;

        RectTransform rt = lineaGO.GetComponent<RectTransform>();
        Vector2 dir = (end - start).normalized;
        float distancia = Vector2.Distance(start, end);

        rt.anchorMin = rt.anchorMax = Vector2.zero;
        rt.sizeDelta = new Vector2(distancia, grosorLinea);
        rt.anchoredPosition = start + dir * distancia * 0.5f;
        rt.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
}