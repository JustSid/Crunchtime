using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierThing
{
    static private float CalculatePoint(float n1, float n2, float ratio)
    {
        float diff = n2 - n1;
        return n1 + (diff * ratio);
    }

    static public Vector3[] CalculateBezier(Vector3 start, Vector2 end, float z, int steps = 20)
    {
        Vector3[] points = new Vector3[steps + 1];

        Vector3 midPoint = new Vector3(1, 1, 0);

        float stepSize = 1.0f / steps;
        int index = 0;

        for(float i = 0.0f; i < 1.0f; i += stepSize)
        {
            float xa = CalculatePoint(start.x, midPoint.x, i);
            float xb = CalculatePoint(midPoint.x, end.x, i);

            float ya = CalculatePoint(start.y, midPoint.y, i);
            float yb = CalculatePoint(midPoint.y, end.y, i);

            float x = CalculatePoint(xa, xb, i);
            float y = CalculatePoint(ya, yb, i);

            points[index ++] = new Vector3(x, y, z);
        }

        points[steps] = new Vector3(end.x, end.y, z);

        return points;
    }

    static public Vector3[] CalculateAutoAttachmentPoints(Vector3 start, Vector3 end)
    {
        if (Mathf.Abs(start.x - end.x) < 0.01f || Mathf.Abs(start.y - end.y) < 0.01f)
        {
            Vector3[] positions = new Vector3[2];
            positions[0] = start;
            positions[1] = end;

            return positions;
        }
        else
        {
            float half_y = (end.y - start.y) / 2.0f;
            float half_x = (end.x - start.x) / 2.0f;

            if (Math.Abs(half_x) > Math.Abs(half_y))
            {
                if (half_y < 0.0)
                {
                    Swap(ref start, ref end);
                    half_y = (end.y - start.y) / 2.0f;
                }

                Vector3[] positions = new Vector3[5];
                positions[0] = start;
                positions[1] = new Vector3(start.x, start.y + half_y);
                positions[2] = new Vector3(start.x, start.y + half_y);
                positions[3] = new Vector3(end.x, start.y + half_y);
                positions[4] = end;

                return positions;
            }
            else
            {
                if(half_x < 0.0)
                {
                    Swap(ref start, ref end);
                    half_x = (end.x - start.x) / 2.0f;
                }

                Vector3[] positions = new Vector3[4];
                positions[0] = start;
                positions[1] = new Vector3(start.x + half_x, start.y);
                positions[2] = new Vector3(start.x + half_x, end.y);
                positions[3] = end;

                return positions;
            }
        }
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
}
