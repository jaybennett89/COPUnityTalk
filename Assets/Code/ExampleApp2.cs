using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace COPUnity
{
    public class ExampleApp2 : ContextView 
    {
        void Start()
        {
            DontDestroyOnLoad(this);
            context = new COPUnityExample2(this, true);
            context.Start(); // fires the ContextKeys.START event
        }
    }

    public class COPUnityExample2 : MVCSContext
    {
        public COPUnityExample2(MonoBehaviour application, bool autoMap) : base(application, autoMap) {}

        protected override void mapBindings()
        {
            // commands
            commandBinder.Bind(ContextEvent.START).To<Example2StartCommand>();

            // example: binding a singleton service component
            injectionBinder.Bind<IPathfinder>().To<Pathfinder>().ToSingleton();

            // example: binding to a value / object
            var exampleCamera = GameObject.FindObjectOfType<ExampleCamera>();
            injectionBinder.Bind<ICamera>().ToValue(exampleCamera);

            // example: binding concrete implementations over an enum
            WeaponBindings.Bind(injectionBinder);
        }
    }

    public class Example2StartCommand : Command
    {
        public override void Execute()
        {
            Debug.Log("COPUnity Example App 2 Starting...");

            var respawn = GameObject.FindGameObjectWithTag("Respawn");
 
            var character = MonoBehaviour.Instantiate(Resources.Load("Character", typeof(GameObject)),
                respawn.transform.position, respawn.transform.rotation) as GameObject;          
        }
    }
}
