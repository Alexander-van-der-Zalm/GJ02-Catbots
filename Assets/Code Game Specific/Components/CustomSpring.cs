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
        // Offset for the two spring sets
        Vector2 offset = new Vector2();

        Vector2 anchorDelta = Anchor - ConnectedAnchor;
        Vector3 dir = new Vector3(anchorDelta.x,anchorDelta.y) + (Rigid1.transform.position - Rigid2.transform.position);

        // Check if horizontal or vertical
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
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
            SpringJoint2D j = (SpringJoint2D)Joints[i].Joint;
            float dist = distance;
            if (i == 1 || i == 2)
                dist = crossDist;

            j.distance = dist;
            SpringConnections[i].Distance = dist;
        }
    }

    public void OnDrawGizmosSelected()
    {
        if (!enabled)
            return;
        if (!DrawJointGizmos())
        {
            CreateSpringConnections();
            DrawSpringConnections();
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
