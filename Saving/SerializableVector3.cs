using UnityEngine;

namespace Musialkov.Saving
{
    [System.Serializable]
    public class SerializableVector3
    {
        float x, y, z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }

        public override string ToString()
        {
            return x.ToString() + " " + y.ToString() + " " + z.ToString();
        }
    }
}