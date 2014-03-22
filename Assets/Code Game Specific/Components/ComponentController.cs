using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComponentController : MonoBehaviour 
{
    public enum CatBotControlScheme
    {
        Action1,Action2,Action3
    }
    
    public int ControlSchemeIndex = 1;
    public List<WheelPhysics> Wheels = new List<WheelPhysics>();
    //TODO THE REST

    private ControlScheme controls;


	void Start () 
    {
        controls = ControlScheme.CreateScheme<CatBotControlScheme>();
        controls.controllerID = ControlSchemeIndex;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        float hor = controls.Horizontal.Value();
        
        for (int i = 0; i < Wheels.Count; i++)
            Wheels[i].Move(hor);
	}
}
