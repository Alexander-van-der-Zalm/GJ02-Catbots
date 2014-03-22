using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Player")
            return;

        float force1 = (rigidbody2D.velocity * rigidbody2D.mass).magnitude;
        float force2 = (other.rigidbody.velocity * rigidbody2D.mass).magnitude;
        Debug.Log(gameObject.tag + " " + force1 + " " + other.gameObject.tag +  " " + force2);
    }
}
