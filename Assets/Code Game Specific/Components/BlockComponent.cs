using UnityEngine;
using System.Collections;

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
        int bla = 99 / 100;
        Debug.Log("blockComponent" + bla);
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
