using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : MaskableGraphic
{
    public List<Vector2> points = new List<Vector2>();
    public float lineWidth = 5f;

    public void SetPoints(List<Vector2> newPoints)
    {
        points = newPoints;
        SetVerticesDirty(); 
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (points == null || points.Count < 2) return;

        for (int i = 0; i < points.Count - 1; i++)
        {
            DrawSegment(vh, points[i], points[i + 1], i);
        }
    }

    private void DrawSegment(VertexHelper vh, Vector2 start, Vector2 end, int index)
    {
        Vector2 dir = (end - start).normalized;
        Vector2 perp = new Vector2(-dir.y, dir.x) * (lineWidth / 2f);

        int vertIndex = index * 4;

        vh.AddVert(start - perp, color, Vector2.zero);
        vh.AddVert(start + perp, color, Vector2.zero);
        vh.AddVert(end + perp, color, Vector2.zero);
        vh.AddVert(end - perp, color, Vector2.zero);

        vh.AddTriangle(vertIndex, vertIndex + 1, vertIndex + 2);
        vh.AddTriangle(vertIndex + 2, vertIndex + 3, vertIndex);
    }
}