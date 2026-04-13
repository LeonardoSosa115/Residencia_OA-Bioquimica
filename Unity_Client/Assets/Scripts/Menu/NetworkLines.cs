using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
public class NetworkLines : MaskableGraphic
{
    public List<RectTransform> nodes = new List<RectTransform>();
    public float connectionDistance = 250f;
    public Color lineColor = new Color(1, 1, 1, 0.3f);

    public void SetNodes(List<RectTransform> n)
    {
        nodes = n;
    }

    void Update()
    {
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (nodes == null || nodes.Count == 0) return;

        int index = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                if (nodes[i] == null || nodes[j] == null) continue;

                Vector2 posA = nodes[i].anchoredPosition;
                Vector2 posB = nodes[j].anchoredPosition;
                float dist = Vector2.Distance(posA, posB);

                if (dist < connectionDistance)
                {
                    float alpha = 1f - (dist / connectionDistance);
                    Color c = new Color(
                        lineColor.r, lineColor.g,
                        lineColor.b, lineColor.a * alpha
                    );

                    Vector2 dir = (posB - posA).normalized;
                    Vector2 perp = new Vector2(-dir.y, dir.x) * 1.5f;

                    UIVertex v0 = UIVertex.simpleVert;
                    v0.color = c;

                    v0.position = posA + perp; vh.AddVert(v0);
                    v0.position = posA - perp; vh.AddVert(v0);
                    v0.position = posB + perp; vh.AddVert(v0);
                    v0.position = posB - perp; vh.AddVert(v0);

                    vh.AddTriangle(index, index + 1, index + 2);
                    vh.AddTriangle(index + 1, index + 3, index + 2);
                    index += 4;
                }
            }
        }
    }
}