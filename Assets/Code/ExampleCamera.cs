using UnityEngine;
using System.Collections;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;
using System;

namespace COPUnity
{
    public enum CameraEvent
    {
        SET_TARGET,
    }
    
    public class ExampleCamera : MonoView 
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher eventBus { get; set; }

        public Transform pivot;
        public Camera camera;
        public Transform target;
        public float interp = 1.0f;
     
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
       
        protected override void OnAwake()
        {
            camera = this.GetComponent<Camera>();

            eventBus.AddListener(CameraEvent.SET_TARGET, OnSetTarget);
        }
    
        void Update()
        {
            if(target)
            {
                pivot.transform.position = Vector3.Lerp(pivot.transform.position, target.transform.position, interp * Time.deltaTime);
            }
        }

        void OnSetTarget(IEvent evt)
        {
            var t = (Transform)evt.data;
            if(t == null)
            {
                // can consider both throwing an exception as a strong assertion or simply returning
                throw new NullReferenceException();
            }

            Debug.Log("Camera Set Target: " + t.gameObject.name);
            this.target = t;
        }
    }
}
