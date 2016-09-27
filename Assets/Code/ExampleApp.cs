using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace COPUnity
{
    public class ExampleApp : ContextView 
    {
        void Start()
        {
            DontDestroyOnLoad(this);
            context = new COPUnityExample(this, true);
            context.Start(); // fires the ContextKeys.START event
        }
    }

    public class COPUnityExample : MVCSContext
    {
        public COPUnityExample(MonoBehaviour application, bool autoMap) : base(application, autoMap) {}

        protected override void mapBindings()
        {
            // commands
            commandBinder.Bind(ContextEvent.START).To<ExampleStartCommand>();
        }
    }

    public class ExampleStartCommand : Command
    {
        public override void Execute()
        {
            Debug.Log("COPUnity Example App Starting...");
        }
    }
}
