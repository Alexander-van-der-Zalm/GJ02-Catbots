using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wheel : MonoBehaviour 
{
    public List<GameObject> Mid;
    public List<GameObject> End;
    public bool jump;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OnCollisionStay2D(Collision2D other)
    {
        //Debug.Log(other.transform.tag);
       
        //if(jump && other.transform.tag == "Floor")
        //    rigidbody2D.AddForce(new Vector2(0,600));
    }
}
