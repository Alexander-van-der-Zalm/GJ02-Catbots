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
            Hinge
        }
        public Rigidbody2D rigid1, rigid2;
        public float distance;

        public void CreateSpring()
        {

        }
    }

    public List<ConnectBySpring> SpringConnections;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
