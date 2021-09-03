public struct Vector2
{
    public float x;
    public float y;


    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }


    public static Vector2 operator +(Vector2 vectorLeft, Vector2 vectorRight)
        => new Vector2(vectorLeft.x + vectorRight.x, vectorLeft.y + vectorRight.y);


    public static Vector2 Zero => new Vector2(0, 0);
}