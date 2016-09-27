using UnityEngine;
using System.Collections;

public class ExampleCamera : MonoBehaviour, ICamera 
{
    public Transform pivot;
    public Camera camera;
    public Transform target;
    public float interp = 1.0f;
 
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
   
    void Awake()
    {
        camera = this.GetComponent<Camera>();
    }

    void Update()
    {
        if(target)
        {
            pivot.transform.position = Vector3.Lerp(pivot.transform.position, target.transform.position, interp * Time.deltaTime);
        }
    }
}

public interface ICamera
{
    void SetTarget(Transform target);
}
