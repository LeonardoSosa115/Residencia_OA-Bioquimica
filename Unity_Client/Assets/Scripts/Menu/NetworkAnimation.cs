using UnityEngine;
using System.Collections.Generic;

public class NetworkAnimation : MonoBehaviour
{
    [Header("Configuración de nodos")]
    public int nodeCount = 40;
    public float areaWidth = 1920f;
    public float areaHeight = 1080f;
    public float nodeSpeed = 30f;
    public float connectionDistance = 250f;

    [Header("Visuals")]
    public GameObject nodePrefab;      // asignarás un prefab simple
    public Color nodeColor = Color.white;
    public Color lineColor = new Color(1, 1, 1, 0.3f);

    private List<RectTransform> nodes = new List<RectTransform>();
    private List<Vector2> velocities = new List<Vector2>();
    private List<UnityEngine.UI.Image> nodeImages = new List<UnityEngine.UI.Image>();

    // Para dibujar líneas en UI usaremos un componente GL custom
    private NetworkLines lineRenderer;

    void Start()
    {
        Transform linesObj = transform.Find("LinesRenderer");
        if (linesObj == null)
        {
            Debug.LogError("No se encontró el hijo 'LinesRenderer'");
            return;
        }

        lineRenderer = linesObj.GetComponent<NetworkLines>();
        if (lineRenderer == null)
        {
            Debug.LogError("LinesRenderer no tiene el componente NetworkLines");
            return;
        }

        lineRenderer.lineColor = lineColor;
        lineRenderer.connectionDistance = connectionDistance;

        // Usar dimensiones reales del RectTransform
        RectTransform parentRT = GetComponent<RectTransform>();
        float w = parentRT.rect.width;
        float h = parentRT.rect.height;

        Debug.Log("Área real: " + w + " x " + h);

        for (int i = 0; i < nodeCount; i++)
        {
            GameObject nodeObj = Instantiate(nodePrefab, transform);
            RectTransform rt = nodeObj.GetComponent<RectTransform>();

            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);

            rt.anchoredPosition = new Vector2(
                Random.Range(w * 0.4f, w),
                Random.Range(0f, h)
            );

            float size = Random.Range(6f, 14f);
            rt.sizeDelta = new Vector2(size, size);

            Vector2 vel = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized * nodeSpeed;

            nodes.Add(rt);
            velocities.Add(vel);

            var img = nodeObj.GetComponent<UnityEngine.UI.Image>();
            if (img != null) img.color = nodeColor;
        }

        lineRenderer.SetNodes(nodes);
    }

    void Update()
    {
        RectTransform parentRT = GetComponent<RectTransform>();
        float w = parentRT.rect.width;
        float h = parentRT.rect.height;

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector2 pos = nodes[i].anchoredPosition + velocities[i] * Time.deltaTime;

            if (pos.x < w * 0.4f || pos.x > w)
                velocities[i] = new Vector2(-velocities[i].x, velocities[i].y);
            if (pos.y < 0 || pos.y > h)
                velocities[i] = new Vector2(velocities[i].x, -velocities[i].y);

            pos.x = Mathf.Clamp(pos.x, w * 0.4f, w);
            pos.y = Mathf.Clamp(pos.y, 0, h);

            nodes[i].anchoredPosition = pos;
        }
    }
}