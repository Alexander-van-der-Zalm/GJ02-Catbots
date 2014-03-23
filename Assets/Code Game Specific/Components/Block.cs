using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour 
{
    public float HP;
    public int Cost;

    public GameObject BlockParticle;

    public int PlayerID;

    public float MinForce = 100;
    public float MaxForce = 200;
    public float MinDamage = 1.0f;
    public float MaxDamage = 2.0f;

    [HideInInspector]
    public List<Joint2D> ConnectedJoints = new List<Joint2D>();

    private Vector2 OldVelocity;
    private Rigidbody2D rigid;
	// Use this for initialization
	void Start () 
    {
        rigid = rigidbody2D;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        OldVelocity = rigid.velocity;
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") || !gameObject.CompareTag("Player"))
            return;

        Block otherBLock = other.gameObject.GetComponent<Block>();
        
        if (otherBLock == null || otherBLock.PlayerID == PlayerID)
            return;

        float force1 = OldVelocity.magnitude * rigidbody2D.mass;
        float force2 = otherBLock.OldVelocity.magnitude * rigidbody2D.mass;

        if (force2 > MinForce)
        {
            float damage;
            if (force2 > MaxForce)
                damage = MaxDamage;
            else
            {
                float deltaMinMax = MaxForce - MinForce;
                float delta = force2 - MinForce;
                damage = MinDamage + delta / deltaMinMax * (MaxDamage - MinDamage);
            }
            Debug.Log("BAAAM DAMAGE " + damage);
            
            DoDamage(damage, rigid.velocity);
        }

        //Debug.Log(other.relativeVelocity + "   " + gameObject.tag + PlayerID + " " + force1 + " " + rigidbody2D.velocity + " old " + OldVelocity + " " + other.gameObject.tag + otherBLock.PlayerID + " " + force2 + " " + other.gameObject.rigidbody2D.velocity + " old " + otherBLock.OldVelocity);
    }

    private void DoDamage(float damage, Vector2 velocity)
    {
        if (HP - damage <= 0)
        {
            //GameObject go = GameObject.Instantiate(BlockParticle, rigid.transform.position, rigid.transform.rotation) as GameObject;
            //go.rigidbody2D.velocity = velocity * 2;
            //go.rigidbody2D.AddForce(velocity * 10);

            //gameObject.SetActive(false);
            
            //Joint2D[] joints = GetComponents<Joint2D>();
            SpringConnector[] SpringConnectors = GetComponents<SpringConnector>();

            foreach (Joint2D joint in ConnectedJoints)
                joint.enabled = false;

            foreach (SpringConnector spring in SpringConnectors)
                spring.enabled = false;
        }
        else
        {
            HP-=damage;
        }
    }


}
