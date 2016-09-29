using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Collections.Generic;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;

namespace COPUnity
{
    public class AStarController : MonoView 
    {
        public CharacterController controller;
        public Seeker seeker;

        // injections
        [Inject]
        public IPathfinder pathfinder { get; set; }

        [Inject(WeaponId.HockeyStick)]
        public IWeapon weapon { get; set; }

        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher eventBus { get; set; }

        // internal path from pathfinder
        List<Vector3> path;
        int currentWaypoint = 0;

        // settings
        float movespeed = 12f;
        float rotationSpeed = 10f;
        float gravity = -10f;
        float maxWaypointDist = 0.2f;

        protected override void OnAwake()
        {
            Debug.Log("Weapon " + weapon.Description);

            controller = this.GetComponent<CharacterController>();
            seeker = this.GetComponent<Seeker>();

            eventBus.Dispatch(CameraEvent.SET_TARGET, this.transform);
        }

        void Update()
        {
            // input
            if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                RaycastHit hit;
    
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.NameToLayer("Ground")))
                {
                    // without dependency injection
                    // var nearestWalkablePos = AstarPath.active.GetNearest(hit.point).clampedPosition;
 
                    // with dependency injection
                    var nearestWalkablePos = pathfinder.GetNearest(hit.point);

                    seeker.StartPath(this.transform.position, nearestWalkablePos, onPathCalculated);
                }
            }

            // movement
            var movedir = calculateMovedir();
            var flags = controller.Move (movedir * movespeed * Time.deltaTime);

            // rotation
            var rotation = Quaternion.Euler (0, calculateRotation(movedir), 0);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        Vector3 calculateMovedir()
        {
            Vector3 movedir = Vector3.zero;

            if(path == null)
            {
                return movedir;
            }

            if(isAtEndOfPath())
            {
                path = null;
                return movedir;
            }

            movedir = (path[currentWaypoint] - this.transform.position).normalized;

            var dist = path[currentWaypoint] - this.transform.position;// + movedir;
            dist.y = 0;

            if(dist.magnitude < maxWaypointDist)
            {
                currentWaypoint++;
            }

            if (!controller.isGrounded) 
            {
                movedir.y -= 10f * Time.deltaTime;
            }

            return movedir;
        }

        float calculateRotation(Vector3 movedir)
        {
            float yRotation = this.transform.rotation.eulerAngles.y;

            if(path == null)
            {
                return yRotation;
            }

            if(isAtEndOfPath())
            {
                return yRotation;
            }

            if(movedir == Vector3.zero)
            {
                return yRotation;
            }

            Quaternion targetRotation = Quaternion.LookRotation(movedir);

            targetRotation.x = 0;
            targetRotation.z = 0;

            yRotation = targetRotation.eulerAngles.y;
            return yRotation;
        }

        void onPathCalculated(Path p)
        {
            if (!p.error)
            {
                path = p.vectorPath;
                currentWaypoint = 1;
            }
            else Debug.LogError("Pathfinder returned with an error:\n" + p.error);
        }

        bool isAtEndOfPath()
        {
            if (path == null) return true;
            else return (currentWaypoint >= path.Count);
        }
    }
}