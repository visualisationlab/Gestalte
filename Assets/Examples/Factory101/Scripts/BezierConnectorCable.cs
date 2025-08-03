using UnityEngine;

public class BezierConnectorCable : MonoBehaviour
{
    public Transform from;
    public Transform to;
    public float length;
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        Vector3 start = from.position;
        Vector3 end = to.position;

        // Midpoint aligned on X
        Vector3 mid1 = new Vector3(end.x, start.y, start.z - 0.001f);
        // You could also do mid1 on Y, or customize depending on layout

        Vector3[] points = new[] { start, mid1, end };

        line.positionCount = points.Length;
        line.SetPositions(points);

        // Length calculation
        length = Vector3.Distance(start, mid1) + Vector3.Distance(mid1, end);
    }
}