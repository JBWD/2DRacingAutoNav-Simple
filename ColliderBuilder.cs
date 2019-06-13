using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LineRenderer renderer = GetComponent<LineRenderer>();

        PolygonCollider2D collider2D = gameObject.AddComponent<PolygonCollider2D>();

        Vector3[] points = new Vector3[renderer.positionCount];
        renderer.GetPositions(points);

        Vector2[] convPoints = new Vector2[points.Length];

        for (int i = 0; i < points.Length; i++)
        {

            convPoints[i] = points[i];
        }

        List<Vector2> finalPoints = new List<Vector2>();

        for(int i = 0; i < 2; i++)
        {
            foreach (Vector2 point in convPoints)
            {
                finalPoints.Add(point - Vector2.down * .3f * i);
            }

        }

        collider2D.points = finalPoints.ToArray();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
