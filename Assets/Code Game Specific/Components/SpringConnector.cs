using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpringConnector : MonoBehaviour 
{
    [System.Serializable]
    public class ConnectBySpring
    {
        public enum SpringType
        {
            Spring,
            Hinge,
            Slider,
            OFF
        }
        public SpringType Type;
        public Rigidbody2D rigid1, rigid2;
        public Vector2 Anchor, ConnectedAnchor;
        public float Distance;
        public bool CollideConnected;

        public Joint2D CreateSpring()
        {
            Joint2D jointret;
            switch (Type)
            {
                case SpringType.Hinge:
                    HingeJoint2D hinge = rigid1.gameObject.AddComponent<HingeJoint2D>();
                    hinge.connectedBody = rigid2;
                    hinge.anchor = Anchor;
                    hinge.connectedAnchor = ConnectedAnchor;
                    hinge.collideConnected = CollideConnected;
                    jointret = (Joint2D)hinge;
                    break;
                case SpringType.Spring:
                    SpringJoint2D joint = rigid1.gameObject.AddComponent<SpringJoint2D>();
                    joint.connectedBody = rigid2;
                    joint.anchor = Anchor;
                    joint.connectedAnchor = ConnectedAnchor;
                    joint.distance = Distance;
                    joint.collideConnected = CollideConnected;
                    jointret = (Joint2D)joint;
                    break;
                case SpringType.Slider:
                    SliderJoint2D slider = rigid1.gameObject.AddComponent<SliderJoint2D>();
                    slider.connectedBody = rigid2;
                    slider.anchor = Anchor;
                    slider.connectedAnchor = ConnectedAnchor;
                    slider.collideConnected = CollideConnected;
                    jointret = (Joint2D)slider;
                    break;
                default:
                    jointret = null;
                    break;
            }

            return jointret;
        }
    }

    public List<ConnectBySpring> SpringConnections;
    protected List<Joint2D> Joints = new List<Joint2D>();

    public float Distance = 1;
    protected float oldDist = 1;

	// Use this for initialization
	void Start () 
    {
        Init();
	}

    protected void Init()
    {
        foreach (ConnectBySpring spring in SpringConnections)
        {
            Joints.Add(spring.CreateSpring());
        }
    }

    public void OnDrawGizmosSelected()
    {
        
        for (int i = 0; i < SpringConnections.Count; i++)
        {
            Gizmos.color = Color.green;
            ConnectBySpring c = SpringConnections[i];
            Vector3 pos1 = c.rigid1.transform.position + new Vector3(c.Anchor.x, c.Anchor.y, 10);
            Vector3 pos2 = c.rigid2.transform.position + new Vector3(c.ConnectedAnchor.x, c.ConnectedAnchor.y, 10);
            Gizmos.DrawLine(pos1, pos2);
            Gizmos.DrawWireSphere(pos1, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos2, 0.1f);
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (oldDist != Distance)
        {
            SpringJoint2D[] springs = GetComponents<SpringJoint2D>();

            foreach (SpringJoint2D spring in springs)
            {
                if (spring.distance == oldDist)
                    spring.distance = Distance;
            }
            oldDist = Distance;
        }
	}
}
