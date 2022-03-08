using UnityEngine;

namespace RPG.Control
{
    public class PathController : MonoBehaviour
    {
        [SerializeField] float radiusOfGizmo = 1f;

        private void OnDrawGizmos() 
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.color = Color.gray;
                Gizmos.DrawSphere(GetWaypoint(i), radiusOfGizmo);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            if(i + 1 == transform.childCount) return 0;
            else return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }      
    }
}
