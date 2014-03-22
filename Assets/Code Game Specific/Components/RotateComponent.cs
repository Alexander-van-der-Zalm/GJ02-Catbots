using UnityEngine;
using System.Collections;

public class RotateComponent : MonoBehaviour 
{
    HingeJoint2D hinge;
    public float MotorSpeed;

	void Start () 
    {
        hinge = GetComponent<HingeJoint2D>();
        JointMotor2D motor = hinge.motor;//.motorSpeed = MotorSpeed;
        motor.motorSpeed = MotorSpeed;
        hinge.motor = motor;
	}

    protected void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            hinge.useMotor = true;
            //hinge.constantForce
            Debug.Log("A");
        }
        else
            hinge.useMotor = false;
    }

    public void Rotate(float deltaAngle)
    {

    }
}
