using UnityEngine;
using System.Collections;

public class CustomSpring : SpringConnector 
{
    public Vector2 Anchor, ConnectedAnchor;
    public Rigidbody2D Rigid1, Rigid2;
    public float SpringWidth;

    public Color GizmoColor;

	// Use this for initialization
	void Start () 
    {
        CreateSpringConnections();

        Init();

        SetDistance(Distance);
	}

    private void CreateSpringConnections()
    {
        Vector2 offset = new Vector2();

        Vector2 dir = Anchor - ConnectedAnchor;
        if (Mathf.Abs(dir.x) > 0)
            offset = new Vector2(0, SpringWidth);
        else
            offset = new Vector2(SpringWidth, 0);
        
        SpringConnections = new System.Collections.Generic.List<ConnectBySpring>();

        for (int i = 0; i < 4; i++)
            SpringConnections.Add(new ConnectBySpring() { ConnectedAnchor = ConnectedAnchor, Anchor = Anchor, rigid1 = Rigid1, rigid2 = Rigid2, Distance = Distance, Type = ConnectBySpring.SpringType.Spring, CollideConnected = true });

        SpringConnections[0].Anchor += offset;
        SpringConnections[1].Anchor -= offset;
        SpringConnections[2].Anchor += offset;
        SpringConnections[3].Anchor -= offset;

        SpringConnections[0].ConnectedAnchor += offset;
        SpringConnections[1].ConnectedAnchor += offset;
        SpringConnections[2].ConnectedAnchor -= offset;
        SpringConnections[3].ConnectedAnchor -= offset;
    }

    private void SetDistance(float distance)
    {
        Distance = distance;
        float crossDist = Mathf.Sqrt(4 * SpringWidth * SpringWidth + Distance * Distance);

        for (int i = 0; i < 4; i++)
        {
            SpringJoint2D j = (SpringJoint2D)Joints[i];
            float dist = distance;
            if (i == 1 || i == 2)
                dist = crossDist;

            j.distance = dist;
            SpringConnections[i].Distance = dist;
        }
    }

    public void OnDrawGizmosSelected()
    {
        //GizmoColor;
        CreateSpringConnections();
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
            SetDistance(Distance);
            oldDist = Distance;
        }
	}
}
