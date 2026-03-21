using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GraficaManual : MonoBehaviour
{
    [Header("Configuración Visual")]
    public RectTransform contenedor; // Arrastra el mismo objeto que tiene este script
    public GameObject puntoPrefab;   // El prefab del circulito que haremos ahora
    public Color colorLinea = new Color(1f, 0.4f, 0f, 1f); // Naranja Gym
    public float grosorLinea = 4f;

    public void Dibujar(List<float> pesos)
    {
        // 1. Limpiar puntos anteriores
        foreach (Transform child in contenedor) {
            if (child.name != "Fondo") Destroy(child.gameObject);
        }

        if (pesos == null || pesos.Count < 2) return;

        // 2. Cálculos de escala
        float anchoTotal = contenedor.rect.width;
        float altoTotal = contenedor.rect.height;
        
        // Ajustamos el rango para que no toque los bordes (margen de 5kg)
        float minPeso = pesos.Min() - 5;
        float maxPeso = pesos.Max() + 5;
        float rangoPeso = maxPeso - minPeso;

        Vector2 ultimoPuntoPos = Vector2.zero;

        // 3. Dibujar
        for (int i = 0; i < pesos.Count; i++)
        {
            // Calculamos posición X (repartida en el ancho) e Y (proporcional al peso)
            float xPos = (i / (float)(pesos.Count - 1)) * anchoTotal;
            float yPos = ((pesos[i] - minPeso) / rangoPeso) * altoTotal;
            Vector2 posicionActual = new Vector2(xPos, yPos);

            // Crear conexión (línea)
            if (i > 0)
            {
                CrearLinea(ultimoPuntoPos, posicionActual);
            }

            // Crear el punto encima
            CrearPunto(posicionActual);

            ultimoPuntoPos = posicionActual;
        }
    }

    void CrearPunto(Vector2 pos)
    {
        GameObject dot = Instantiate(puntoPrefab, contenedor);
        RectTransform rt = dot.GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.anchorMin = rt.anchorMax = Vector2.zero; // Importante para posicionamiento manual
    }

    void CrearLinea(Vector2 start, Vector2 end)
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