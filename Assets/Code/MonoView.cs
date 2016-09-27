using UnityEngine;
using System.Collections;
using strange.extensions.mediation.api;
using strange.extensions.context.impl;
using strange.extensions.mediation.impl;
using strange.extensions.context.api;

namespace COPUnity
{
    public class MonoView : MonoBehaviour, IView 
    {
        #region Implementation of IView
        public bool requiresContext { get; set; }
        public bool autoRegisterWithContext { get; set; }
        public bool registeredWithContext { get; set; }
        #endregion

        protected virtual void OnAwake() {}

        void Awake()
        {
            autoRegisterWithContext = true;
            requiresContext = true;
            registeredWithContext = false;
            registerToContext(true);
            OnAwake();
        }
        
        /// A MonoBehaviour OnDestroy handler
        /// The View will inform the Context that it is about to be
        /// destroyed.
        protected virtual void OnDestroy ()
        {
            registerToContext(false);
        }
        
        void registerToContext(bool toAdd)
        {
            if (Context.firstContext != null)
            {
                if(toAdd)
                {
                    Context.firstContext.AddView (this);
                    registeredWithContext = true;
                }
                else
                {
                    Context.firstContext.RemoveView(this);                  
                }
                return;
            }
        }
    }
}