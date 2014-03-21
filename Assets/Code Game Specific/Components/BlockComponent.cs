using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(BoxCollider2D))]
public class BlockComponent : MonoBehaviour 
{
    protected Transform trans;

	// Use this for initialization
	void Start () 
    {
        Init();
	}

    protected virtual void Init()
    {
        trans = transform;
        rigidbody2D.isKinematic = true; 
        Debug.Log("blockComponent");
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
