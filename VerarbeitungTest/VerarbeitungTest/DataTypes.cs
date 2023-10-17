static class DataType
{
    public static struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float x,float y,float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float x { get; }
        public float y { get; }
        public float z { get; }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            Vector3 v = new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
            return v;
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            Vector3 v = new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
            return v;
        }
        public static Vector3 operator *(Vector3 v1, float s)
        {
            Vector3 v = new Vector3(v1.x * s, v1.y * s, v1.z * s);
            return v;
        }
        public static Vector3 operator /(Vector3 v1, float s)
        {
            Vector3 v = new Vector3(v1.x / s, v1.y / s, v1.z / s);
            return v;
        }

        public override string ToString() => $"({x} ,{y} ,{z})";
    }
}