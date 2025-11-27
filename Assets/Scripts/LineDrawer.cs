using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public Material lineMaterial;

    private List<LineRenderer> lines = new List<LineRenderer>();
    private int currentIndex = 0;

    // Drawing lines with Line Renderer
    public void DrawLine(Vector3 startPos, Vector3 endPos, Color color, float lineWidth = 1f)
    {
        LineRenderer lr;
        if(lines.Count <= currentIndex)
        {
            GameObject lineObj = new GameObject("Line_" + currentIndex);
            lr = lineObj.AddComponent<LineRenderer>();

            lr.material = lineMaterial;
            
            lr.positionCount = 2;
            lr.useWorldSpace = true;
            lineObj.transform.parent = this.transform;

            lines.Add(lr);
        }
        else
        {
            lr = lines[currentIndex];
            lr.enabled = true;
        }
        
        lr.startWidth = lr.endWidth = lineWidth;
        lr.startColor = lr.endColor = color;
        lr.positionCount = 2;
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);

        currentIndex++;
    }

    // Clear the lines that are not being used
    public void ClearLines()
    {
        for (int i = 0; i < currentIndex; i++)
        {
            if (i < lines.Count)
                lines[i].enabled = false;
        }

        if (currentIndex < lines.Count)
        {
            for (int i = currentIndex; i < lines.Count; i++)
            {
                if (lines[i] != null)
                    Destroy(lines[i].gameObject);
            }

            lines.RemoveRange(currentIndex, lines.Count - currentIndex);
        }

        currentIndex = 0;

    }

    public void DestroyAllLines()
    {
        foreach (var lr in lines)
        {
            if (lr != null)
                Destroy(lr.gameObject);
        }

        lines.Clear();
        currentIndex = 0;
    }
}
