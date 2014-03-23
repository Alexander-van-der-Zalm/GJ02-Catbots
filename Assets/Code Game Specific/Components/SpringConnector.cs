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

    [System.Serializable]
    public class Joint2DHelper
    {
        [SerializeField]
        private Joint2D joint;
        public Joint2D Joint { get { return joint; } set { joint = value; type = value.GetType(); } }

        [SerializeField]
        private System.Type type;
        public System.Type Type { get { return type; } }
    }

    public List<ConnectBySpring> SpringConnections = new List<ConnectBySpring>();
    protected List<Joint2DHelper> Joints = new List<Joint2DHelper>();

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
            Joints.Add(new Joint2DHelper() { Joint = spring.CreateSpring() });
        }
    }

    public void OnDrawGizmosSelected()
    {
        if (!enabled)
            return;

        if(!DrawJointGizmos())
        {
            DrawSpringConnections();
        }
    }

    protected void DrawSpringConnections()
    {
        for (int i = 0; i < SpringConnections.Count; i++)
        {
            Gizmos.color = Color.green;
            ConnectBySpring c = SpringConnections[i];
            Vector3 pos1 = c.rigid1.transform.position;// +;
            Vector3 pos2 = c.rigid2.transform.position;// +;

            Vector3 off1 = new Vector3(c.Anchor.x, c.Anchor.y, 10);
            Vector3 off2 = new Vector3(c.ConnectedAnchor.x, c.ConnectedAnchor.y, 10);

            pos1 += c.rigid1.transform.TransformDirection(off1);
            pos2 += c.rigid2.transform.TransformDirection(off2);

            Gizmos.DrawLine(pos1, pos2);
            Gizmos.DrawWireSphere(pos1, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos2, 0.1f);
        }
    }

    protected bool DrawJointGizmos()
    {
        if (Joints.Count <= 0)
            return false;

        for (int i = 0; i < Joints.Count; i++)
        {
            Joint2DHelper j = Joints[i];

            Gizmos.color = Color.green;
            Joint2D c = j.Joint;
            Vector3 pos1 = c.gameObject.rigidbody2D.transform.position;// +new Vector3(c.Anchor.x, c.Anchor.y, 10);
            Vector3 pos2 = c.connectedBody.transform.position;// +new Vector3(c.ConnectedAnchor.x, c.ConnectedAnchor.y, 10);

            Vector3 off1 = Vector3.zero;
            Vector3 off2 = Vector3.zero;

            switch (j.Type.ToString())
            {
                case "UnityEngine.HingeJoint2D":
                    HingeJoint2D hinge = c as HingeJoint2D;
                    if (hinge != null)
                    {
                        off1 = new Vector3(hinge.anchor.x, hinge.anchor.y, 10);
                        off2 = new Vector3(hinge.connectedAnchor.x, hinge.connectedAnchor.y, 10);
                    }
                    break;
                case "UnityEngine.SpringJoint2D":
                    SpringJoint2D joint = c as SpringJoint2D;
                    off1 = new Vector3(joint.anchor.x, joint.anchor.y, 10);
                    off2 = new Vector3(joint.connectedAnchor.x, joint.connectedAnchor.y, 10);
                    break;
            }

            pos1 += c.gameObject.rigidbody2D.transform.TransformDirection(off1);
            pos2 += c.connectedBody.transform.TransformDirection(off2);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos1, pos2);
            Gizmos.DrawWireSphere(pos1, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos2, 0.1f);
        }
        return true;
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
