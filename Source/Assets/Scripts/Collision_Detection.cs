using System.Collections.Generic;
using UnityEngine;

public class LineDrawingWithCircleInteraction : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float lineWidth = 0.1f;
    public LayerMask circleLayer;

    private List<Vector3> linePoints = new List<Vector3>();
    private bool isDrawing = false;

    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -3f;
    public float maxY = 3f;

    void Start()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButton(0))
        {
            ContinueDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
        }
    }

    void StartDrawing()
    {
        isDrawing = true;
        linePoints.Clear();
        lineRenderer.positionCount = 0;

        Vector3 mousePosition = GetClampedMousePosition();
        linePoints.Add(mousePosition);
    }

    void ContinueDrawing()
    {
        if (isDrawing)
        {
            Vector3 mousePosition = GetClampedMousePosition();
            linePoints.Add(mousePosition);

            UpdateLineRenderer();

            // Detect and remove intersected circles
            DetectAndRemoveIntersectedCircles();
        }
    }

    void StopDrawing()
    {
        isDrawing = false;
        lineRenderer.positionCount = 0;
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

    Vector3 GetClampedMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        float clampedX = Mathf.Clamp(mousePosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(mousePosition.y, minY, maxY);

        return new Vector3(clampedX, clampedY, 0);
    }

    void DetectAndRemoveIntersectedCircles()
    {
        foreach (Vector3 point in linePoints)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(point, circleLayer);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Circle"))
                {
                    Destroy(collider.gameObject);
                }
            }
        }
    }
}
