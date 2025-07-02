using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
     public enum PivotAxis
    {
        // Rotate about all axes.
        Free,
        // Rotate about an individual axis.
        X,
        Y, 
        Z
    }
    
        /// <summary>
        /// The axis about which the object will rotate.
        /// </summary>
        [Tooltip("Specifies the axis about which the object will rotate (Free rotates about both X and Y).")]
        public PivotAxis pivotAxis = PivotAxis.Free;

        /// <summary>
        /// Overrides the cached value of the GameObject's default rotation.
        /// </summary>
        public Quaternion DefaultRotation { get; private set; }

        private void Awake()
        {
            // Cache the GameObject's default rotation.
            DefaultRotation = gameObject.transform.rotation;
        }

        /// <summary>
        /// Keeps the object facing the camera.
        /// </summary>
        private void Update()
        {
            // Get a Vector that points from the Camera to the target.
            Vector3 forward;
            Vector3 up;

            // Adjust for the pivot axis. We need a forward and an up for use with Quaternion.LookRotation
            switch (pivotAxis)
            {
                // If we're fixing one axis, then we're projecting the camera's forward vector onto
                // the plane defined by the fixed axis and using that as the new forward.
                case PivotAxis.X:
                    Vector3 right = transform.right; // Fixed right
                    forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, right).normalized;
                    up = Vector3.Cross(forward, right); // Compute the up vector
                    break;

                case PivotAxis.Y:
                    up = transform.up; // Fixed up
                    forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, up).normalized;
                    break;

                case PivotAxis.Z:
                    forward = Camera.main.transform.forward;
                    up = Camera.main.transform.up;
                    break;

                // If the axes are free then we're simply aligning the forward and up vectors
                // of the object with those of the camera. 
                case PivotAxis.Free:
                default:
                    forward = Camera.main.transform.forward;
                    up = Camera.main.transform.up;
                    break;
            }


            // Calculate and apply the rotation required to reorient the object
            transform.rotation = Quaternion.LookRotation(forward, up);
        }

    // Update is called once per frame
    //void Update()
    //    transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
   // }
}
