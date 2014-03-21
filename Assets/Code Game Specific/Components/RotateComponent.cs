using UnityEngine;
using System.Collections;

public class RotateComponent : MonoBehaviour 
{
    Transform rotate;

	void Start () 
    {
        Init();
	}

    protected void Init()
    {
        //base.Init();
        rotate = transform.Find("ROT");
    }

    // Update is called once per frame
    void Update()
    {
        Rotate(90.0f*Time.deltaTime);
    }

    public void Rotate(float deltaAngle)
    {
        rotate.Rotate(new Vector3(0, 0, 1), deltaAngle);
    }
}
