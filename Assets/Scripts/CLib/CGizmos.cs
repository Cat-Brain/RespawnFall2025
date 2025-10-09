using UnityEngine;

public static class CGizmos
{
    #region Constants
    const float Sqrt2 = 1.41421356f;
    const float InvSqrt2 = 0.70710678f;
    #endregion
    #region 3D
    // Does not connect at all to center
    public static void DrawEmptyAxisArc(Vector3 center, Vector3 direction, Vector3 axis, float radius, float angle, float precisionPerDegree)
    {
        int precision = Mathf.CeilToInt(angle * precisionPerDegree);
        Vector3[] points = new Vector3[precision];
        direction *= radius;
        for (int i = 0; i < precision; i++)
            points[i] = center + Quaternion.AngleAxis((i / (precision - 1f) * 2 - 1) * angle, axis) * direction;

        Gizmos.DrawLineStrip(points, false);
    }

    // Does not connect at all to center
    public static void DrawEmptyAxisArc(Vector3 center, Vector3 direction, Vector3 axis, float radius, float angle)
    {
        DrawEmptyAxisArc(center, direction, axis, radius, angle, arcPrecisionPerDegree);
    }

    #region Wire Squares
    public static void DrawXYWireSquare(Vector3 center, Vector2 halfDim)
    {
        Vector3[] points = new Vector3[4];
        Vector3 dir = halfDim;
        points[0] = center + dir;
        dir.x *= -1;
        points[1] = center + dir;
        dir.y *= -1;
        points[2] = center + dir;
        dir.x *= -1;
        points[3] = center + dir;

        Gizmos.DrawLineStrip(points, true);
    }

    public static void DrawXZWireSquare(Vector3 center, Vector2 halfDim)
    {
        Vector3[] points = new Vector3[4];
        Vector3 dir = CMath.Vector3X0Y(halfDim);
        points[0] = center + dir;
        dir.x *= -1;
        points[1] = center + dir;
        dir.z *= -1;
        points[2] = center + dir;
        dir.x *= -1;
        points[3] = center + dir;

        Gizmos.DrawLineStrip(points, true);
    }

    public static void DrawYZWireSquare(Vector3 center, Vector2 halfDim)
    {
        Vector3[] points = new Vector3[4];
        Vector3 dir = CMath.Vector30XY(halfDim);
        points[0] = center + dir;
        dir.y *= -1;
        points[1] = center + dir;
        dir.z *= -1;
        points[2] = center + dir;
        dir.y *= -1;
        points[3] = center + dir;

        Gizmos.DrawLineStrip(points, true);
    }
    #endregion

    public static void DrawWireExpandedCube(Vector3 center, Vector3 halfDim, float radius)
    {
        DrawYZWireSquare(center + new Vector3(halfDim.x + radius, 0, 0), CMath.Vector3ToYZ(halfDim));
        DrawYZWireSquare(center - new Vector3(halfDim.x + radius, 0, 0), CMath.Vector3ToYZ(halfDim));

        DrawEmptyAxisArc(center + halfDim, new Vector3(0, InvSqrt2, InvSqrt2), Vector3.right, radius, 45);
        DrawEmptyAxisArc(center + CMath.V3NPP(halfDim), new Vector3(0, InvSqrt2, InvSqrt2), Vector3.right, radius, 45);

        DrawEmptyAxisArc(center + CMath.V3PNP(halfDim), new Vector3(0, -InvSqrt2, InvSqrt2), Vector3.right, radius, 45);
        DrawEmptyAxisArc(center + CMath.V3NNP(halfDim), new Vector3(0, -InvSqrt2, InvSqrt2), Vector3.right, radius, 45);

        DrawEmptyAxisArc(center + CMath.V3PNN(halfDim), new Vector3(0, -InvSqrt2, -InvSqrt2), Vector3.right, radius, 45);
        DrawEmptyAxisArc(center + -halfDim, new Vector3(0, -InvSqrt2, -InvSqrt2), Vector3.right, radius, 45);

        DrawEmptyAxisArc(center + CMath.V3PPN(halfDim), new Vector3(0, InvSqrt2, -InvSqrt2), Vector3.right, radius, 45);
        DrawEmptyAxisArc(center + CMath.V3NPN(halfDim), new Vector3(0, InvSqrt2, -InvSqrt2), Vector3.right, radius, 45);


        DrawXZWireSquare(center + new Vector3(0, halfDim.y + radius, 0), CMath.Vector3ToXZ(halfDim));
        DrawXZWireSquare(center - new Vector3(0, halfDim.y + radius, 0), CMath.Vector3ToXZ(halfDim));

        DrawEmptyAxisArc(center + halfDim, new Vector3(InvSqrt2, 0, InvSqrt2), Vector3.up, radius, 45);
        DrawEmptyAxisArc(center + CMath.V3PNP(halfDim), new Vector3(InvSqrt2, 0, InvSqrt2), Vector3.up, radius, 45);

        DrawEmptyAxisArc(center + CMath.V3NPP(halfDim), new Vector3(-InvSqrt2, 0, InvSqrt2), Vector3.up, radius, 45);
        DrawEmptyAxisArc(center + CMath.V3NNP(halfDim), new Vector3(-InvSqrt2, 0, InvSqrt2), Vector3.up, radius, 45);

        DrawEmptyAxisArc(center + CMath.V3NPN(halfDim), new Vector3(-InvSqrt2, 0, -InvSqrt2), Vector3.up, radius, 45);
        DrawEmptyAxisArc(center + -halfDim, new Vector3(-InvSqrt2, 0, -InvSqrt2), Vector3.up, radius, 45);

        DrawEmptyAxisArc(center + CMath.V3PPN(halfDim), new Vector3(InvSqrt2, 0, -InvSqrt2), Vector3.up, radius, 45);
        DrawEmptyAxisArc(center + CMath.V3PNN(halfDim), new Vector3(InvSqrt2, 0, -InvSqrt2), Vector3.up, radius, 45);


        DrawXYWireSquare(center + new Vector3(0, 0, halfDim.z + radius), halfDim);
        DrawXYWireSquare(center - new Vector3(0, 0, halfDim.z + radius), halfDim);

        DrawEmptyAxisArc(center + halfDim, new Vector3(InvSqrt2, InvSqrt2, 0), Vector3.forward, radius, 45);
        DrawEmptyAxisArc(center + CMath.V3PPN(halfDim), new Vector3(InvSqrt2, InvSqrt2), Vector3.forward, radius, 45);

        DrawEmptyAxisArc(center + CMath.V3NPP(halfDim), new Vector3(-InvSqrt2, InvSqrt2, 0), Vector3.forward, radius, 45);
        DrawEmptyAxisArc(center + CMath.V3NPN(halfDim), new Vector3(-InvSqrt2, InvSqrt2, 0), Vector3.forward, radius, 45);

        DrawEmptyAxisArc(center + CMath.V3NNP(halfDim), new Vector3(-InvSqrt2, -InvSqrt2, 0), Vector3.forward, radius, 45);
        DrawEmptyAxisArc(center + -halfDim, new Vector3(-InvSqrt2, -InvSqrt2, 0), Vector3.forward, radius, 45);

        DrawEmptyAxisArc(center + CMath.V3PNP(halfDim), new Vector3(InvSqrt2, -InvSqrt2, 0), Vector3.forward, radius, 45);
        DrawEmptyAxisArc(center + CMath.V3PNN(halfDim), new Vector3(InvSqrt2, -InvSqrt2, 0), Vector3.forward, radius, 45);
    }
    #endregion
    #region 2D
    public const int circlePrecision = 72;
    public const float arcPrecisionPerDegree = 0.2f;
    public static void DrawCircle(Vector2 center, float radius)
    {
        Vector3[] points = new Vector3[circlePrecision];
        for (int i = 0; i < circlePrecision; i++)
            points[i] = center + radius * CMath.Polar(radius, Mathf.PI * 2 * i / circlePrecision);

        Gizmos.DrawLineStrip(points, true);
    }

    public static void DrawArc(Vector2 center, Vector2 direction, float radius, float angle, float precisionPerDegree)
    {
        int precision = Mathf.CeilToInt(angle * precisionPerDegree);
        Vector3[] points = new Vector3[precision + 1];
        points[0] = center;
        direction *= radius;
        for (int i = 0; i < precision; i++)
            points[i + 1] = center + CMath.Rotate(direction, (i / (precision - 1f) * 2 - 1) * angle * Mathf.Deg2Rad);

        Gizmos.DrawLineStrip(points, true);
    }

    public static void DrawArc(Vector2 center, Vector2 direction, float radius, float angle)
    {
        DrawArc(center, direction, radius, angle, arcPrecisionPerDegree);
    }

    // Does not connect at all to center
    public static void DrawEmptyArc(Vector2 center, Vector2 direction, float radius, float angle, float precisionPerDegree)
    {
        int precision = Mathf.CeilToInt(angle * precisionPerDegree);
        Vector3[] points = new Vector3[precision];
        direction *= radius;
        for (int i = 0; i < precision; i++)
            points[i] = center + CMath.Rotate(direction, (i / (precision - 1f) * 2 - 1) * angle * Mathf.Deg2Rad);

        Gizmos.DrawLineStrip(points, false);
    }

    // Does not connect at all to center
    public static void DrawEmptyArc(Vector2 center, Vector2 direction, float radius, float angle)
    {
        DrawEmptyArc(center, direction, radius, angle, arcPrecisionPerDegree);
    }

    public static void DrawRect(Rect rect)
    {
        Vector2 a = rect.min, b = rect.min;
        a.x = rect.max.x;
        b.y = rect.max.y;
        Vector3[] points = { rect.min, a, rect.max, b };
        Gizmos.DrawLineStrip(points, true);
    }
    #endregion
}
