using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement.Custom2D
{
    [TaskDescription("Check to see if the any objects are within sight of the agent.")]
    [TaskCategory("Movement/Custom2D")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CanSeeObject : Conditional
    {
        [Tooltip("The object that we are searching for")]
        public SharedGameObject targetObject;
        [Tooltip("The LayerMask of the objects to look for when performing the line of sight check")]
        public LayerMask objectLayerMask;
        [Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat fieldOfViewAngle = 90;
        [Tooltip("The distance that the agent can see")]
        public SharedFloat viewDistance = 1000;
        [Tooltip("Should a debug look ray be drawn to the scene view?")]
        public SharedBool drawDebugRay;
        [Tooltip("The object that is within sight")]
        public SharedGameObject returnedObject;

        private GameObject[] agentColliderGameObjects;
        private int[] originalColliderLayer;


        // Returns success if an object was found otherwise failure
        public override TaskStatus OnUpdate()
        {
            if (targetObject.Value != null) { // If the target is not null then determine if that object is within sight
                returnedObject.Value = PhysicsUtils.HasLineOfSight(transform, targetObject.Value.transform, viewDistance.Value,
                    fieldOfViewAngle.Value, objectLayerMask);
            }

            if (returnedObject.Value != null) {
                // Return success if an object was found
                return TaskStatus.Success;
            }
            // An object is not within sight so return failure
            return TaskStatus.Failure;
        }

        // Reset the public variables
        public override void OnReset()
        {
            fieldOfViewAngle = 90;
            viewDistance = 1000;
            drawDebugRay = false;
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
            MovementUtility.DrawLineOfSight(Owner.transform, Vector3.zero, fieldOfViewAngle.Value, 0, viewDistance.Value, true);
            
            if (drawDebugRay.Value && returnedObject.Value != null)
            {
                Debug.DrawLine(transform.position, returnedObject.Value.transform.position, Color.yellow);
            }
        }
    }
}