using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WheelPhysics : MonoBehaviour 
{
    public float Speed;

    //private Rigidbody2D rigid;
    private Rigidbody2D[] rigids;
    private int samplesPerBlock = 2;
    private int blocks = 0;
    private float yMin;
    private float xMin, xMax;

	// Use this for initialization
	void Start () 
    {
        //rigid = rigidbody2D;
        rigids = GetComponentsInChildren<Rigidbody2D>();
	}

    public void Move(float forward)
    {
        // Check if grounded
        
        Vector2 localRight = transform.TransformDirection(Vector3.right);
        Vector2 force = localRight * forward * Speed;
        //Debug.Log(rigid.velocity);
        foreach(Rigidbody2D rigid in rigids)
            rigid.AddForce(force);
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
