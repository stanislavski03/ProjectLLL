        using UnityEngine;

        public class LineDrawer : MonoBehaviour
        {
            public float rayLength = 3f;

            void OnDrawGizmos()
            {
                Gizmos.color = Color.red;
                Vector3 direction = transform.TransformDirection(Vector3.forward) * rayLength;
                Gizmos.DrawRay(transform.position, direction);
            }
        }