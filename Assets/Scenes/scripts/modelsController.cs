using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelsController : MonoBehaviour
{

    public float speedHigh = 1.0f;
    public float speedSlow = 0.1f;
    
    public float maxLen = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 s = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        var rot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        var dir = rot * new Vector3(-s.x, 0, -s.y);
        float tri = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        float inc = speedSlow;
        if (tri > 0.5){
            inc = speedHigh;
        }
        var pos = transform.localPosition + dir * inc * Time.deltaTime;

        if (pos.magnitude > maxLen)
        {
            pos = pos.normalized * maxLen;
        } 

        transform.localPosition = pos;
        
    }
}
