using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIGradient : BaseMeshEffect
{
    public Color startColor = Color.white;
    public Color endColor = Color.black;
    [Range(0f, 360f)]
    public float angle = 90f;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || vh.currentVertCount == 0)
            return;

        UIVertex vertex = new UIVertex();
        Rect rect = graphic.rectTransform.rect;

        // Dirección del gradiente basada en el ángulo
        float rad = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

        // Determinar los valores mínimo y máximo
        float min = float.MaxValue;
        float max = float.MinValue;

        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);
            float projection = Vector2.Dot(vertex.position, direction);
            min = Mathf.Min(min, projection);
            max = Mathf.Max(max, projection);
        }

        float range = max - min;

        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);
            float projection = Vector2.Dot(vertex.position, direction);
            float t = (projection - min) / range;
            vertex.color = Color.Lerp(startColor, endColor, t);
            vh.SetUIVertex(vertex, i);
        }
    }
}