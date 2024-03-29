﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Schematic : MonoBehaviour 
{
    // Layer 0 - Main
    // Layer 1 - Moving
    // Layer 2 - Loose
    int[,,] BluePrint;
    GridType[,,] BlockType;
    int[,,] Group;
    GameObject[, ,] BotObjects;

    public int maxHeight,maxWidth;
    public float gridSize = 1.0f;
    public int PlayerNr;
    
    public GameObject Wheel;
    public GameObject WheelParent;
    public GameObject Block;
    public GameObject Rotator;
    public GameObject Spring;

    //public Sprite

    public enum GridType
    {
        Empty, Block, Spring, Rotator, Wheel
    }
        
        
    // Use this for initialization
	void Start () 
    {
        Init();
        Seed();
        BuildSchematic();
        //DebugHelper.LogArray<int>(Group);
        //DebugHelper.LogArray<GridType>(BlockType);
	}

    private void Init()
    {
        BluePrint = new int[3, maxWidth, maxHeight];
        BotObjects = new GameObject[3, maxWidth, maxHeight];
        Group = new int[3, maxWidth, maxHeight];
        BlockType = new GridType[3, maxWidth, maxHeight];
    }

    public void BuildSchematic()
    {
        // Group & Type loop
        InitSchematic();

        // SetGameObject && Connect Loop
        InitObjects();
    }

    private void InitObjects()
    {
        #region Create the objects
        
        int layersAmount = BluePrint.GetLength(0);
        
        for (int layer = 0; layer < layersAmount; layer++)
            for (int y = 0; y < maxHeight; y++)
                for (int x = 0; x < maxWidth; x++)
                {
                    GameObject prefab = GetGameObject(BlockType[layer, x, y]);
                    if (prefab == null)
                        continue;
                    
                    // Save link to object
                    BotObjects[layer, x, y] = InstantiateObject(prefab, layer, x, y);
                }

        #endregion

        #region Connect the objects

        for (int layer = 0; layer < layersAmount; layer++)
            for (int y = 0; y < maxHeight; y++)
                for (int x = 0; x < maxWidth; x++)
                {
                   GridType type = BlockType[layer, x, y];
                    if (type == GridType.Empty)
                        continue;

                    // Get all the relevant objects
                    GameObject left = x > 0 ? BotObjects[layer,x-1,y] : null;
                    GameObject bot = y > 0 ? BotObjects[layer, x, y-1] : null;
                    GameObject cur = BotObjects[layer, x, y];

                    // Connect if able
                    if (left != null)
                        Connect(cur, left, true, type, BlockType[layer, x - 1, y], layer, x, y);
                    if (bot != null)
                        Connect(cur, bot, false, type, BlockType[layer, x, y - 1], layer, x, y);
                }


        #endregion

        #region Connect objects to controller



        for (int layer = 0; layer < layersAmount; layer++)
            for (int y = 0; y < maxHeight; y++)
                for (int x = 0; x < maxWidth; x++)
                {
                    ConnectToController(gameObject, layer, x, y);
                }

        #endregion
    }

    private void ConnectToController(GameObject parent, int layer, int x, int y)
    {
        GridType type = BlockType[layer, x, y];
        GameObject go = BotObjects[layer, x, y];
        
        switch (type)
        {
            case GridType.Wheel:
                WheelPhysics wheel = go.GetComponent<WheelPhysics>();
                ComponentController controller = go.transform.parent.GetComponent<ComponentController>();
                controller.Wheels.Add(wheel);
                break;
        }
    }

    private void Connect(GameObject cur, GameObject other, bool isLeft, GridType type, GridType otherType, int layer, int x, int y)
    {
        bool connect = true;
        bool springConnection = false;
        bool isDurp = false; // because of x flip wrong pivot point

        #region Wheels

        if (type == GridType.Wheel)
        {
            // Set to right gameobject
            Wheel wb = cur.GetComponent<Wheel>();
                        
            if (isLeft && otherType == GridType.Wheel)
            {
                Wheel otherWb = other.GetComponent<Wheel>();
                
                if (otherWb.Section == global::Wheel.BlockSection.Right)
                {
                    otherWb = otherWb.SetPiece(global::Wheel.BlockSection.Mid);
                    other = otherWb.gameObject;;
                    BotObjects[layer, x - 1, y] = other;
                    ConnectSpring(other, BotObjects[layer, x - 2, y], isLeft);
                }

                wb = wb.SetPiece(global::Wheel.BlockSection.Right);

                cur = wb.gameObject;
                BotObjects[layer, x, y] = cur;
                isDurp = true;
                connect = true;
            }

       
            // Group it up
        }
        #endregion


        if (connect)
        {
            if (springConnection)
            {

            }
            else
            {
                ConnectSpring(cur, other, isLeft,isDurp);
            }
        }
    }

    private void ConnectSpring(GameObject cur, GameObject other, bool isLeft, bool isDurp = false)
    {
        SpringConnector con = cur.AddComponent<SpringConnector>();
        SpringConnector.ConnectBySpring spring = new SpringConnector.ConnectBySpring();

        spring.rigid1 = cur.rigidbody2D;
        spring.rigid2 = other.rigidbody2D;
        spring.Type = SpringConnector.ConnectBySpring.SpringType.Hinge;

        Vector2 anchor = new Vector2(0, 0.5f);
        Vector2 otherAnchor = new Vector2(1, 0.5f);
        if (isDurp)
            anchor.x = 1;
        if (!isLeft)
        {
            anchor = new Vector2(0.5f, 0);
            otherAnchor = new Vector2(0.5f, 1.0f);
        }

        spring.Anchor = anchor;
        spring.ConnectedAnchor = otherAnchor;
        spring.CollideConnected = true;

        con.SpringConnections.Add(spring);
    }

    private void InitSchematic()
    {
        // Group & Type loop 

        for (int layer = 0; layer < 2; layer++)
        {
            int nextNr = 1;
            int nextWheelNR = 101;

            for (int y = 0; y < maxHeight; y++)
                for (int x = 0; x < maxWidth; x++)
                {
                    // Get blueprint nr for type
                    int curType = BluePrint[layer, x, y];

                    // Save the type
                    GridType type = GetBlockType(curType);
                    BlockType[layer, x, y] = type;

                    if (curType == 0)
                        continue;

                    // Get the left/top group value if not out of bounds
                    int left = x > 0 ? Group[layer, x - 1, y] : int.MaxValue;
                    int bot = y > 0 ? Group[layer, x, y - 1] : int.MaxValue;

                    left = left > 0 ? left : int.MaxValue;
                    bot = bot > 0 ? left : int.MaxValue;

                    int newNextNr = type == GridType.Wheel ? nextWheelNR : nextNr;

                    int curNr = Mathf.Min(newNextNr, left, bot);
                    
                    if (curNr == nextNr)
                        nextNr++;
                    else if (curNr == nextWheelNR)
                        nextWheelNR++;
                    
                    Group[layer, x, y] = curNr;
                }
        }
    }

    private GridType GetBlockType(int block)
    {
        if (block == 0)
            return GridType.Empty;

        int blockNR = block / 100;

        GridType type = GridType.Empty;

        switch (blockNR)
        {
            case 0:
                type = GridType.Block;
                break;
            case 1:
                type = GridType.Wheel;
                break;
            case 2:
                type = GridType.Rotator;
                break;
            case 3:
                type = GridType.Spring;
                break;
            default:
                type = GridType.Empty;
                break;
        }

        return type;
        //switch (blockNR)
        //{
        //    case 0:
        //        return GridType.Block;
        //    case 1:
        //        return GridType.Wheel;
        //    case 2:
        //        return GridType.Rotator;
        //    case 3:
        //        return GridType.Spring;
        //    default:
        //        return GridType.Empty;
        //}
    }

    private GameObject InstantiateObject(GameObject prefab, int layer, int x, int y)
    {
        GameObject go = GameObject.Instantiate(prefab, transform.position + new Vector3(x, y) * gridSize, Quaternion.identity) as GameObject;
        go.transform.parent = transform;

        GridType type = BlockType[layer, x, y];
        go.name =  type + " " + x + " " + y;
        go.tag = "Player";

        Block block = go.GetComponent<Block>();
        block.PlayerID = PlayerNr;

        // Set layer
        switch (layer)
        {
            case 0:
                go.layer = LayerMask.NameToLayer("Player" + PlayerNr + "MainBody");
                break;
            case 1:
                go.layer = LayerMask.NameToLayer("Player" + PlayerNr + "MovingBody");
                break;
            default:
                go.layer = LayerMask.NameToLayer("Player" + PlayerNr + "Loose");
                break;
        }

        return go;
    }

    private GameObject GetGameObject(GridType block)
    {
        switch (block)
        {
            case GridType.Block:
                return Block;
            case GridType.Wheel:
                return Wheel;
            case GridType.Rotator:
                return Rotator;
            case GridType.Spring:
                return Spring;
            default:
                return null;
        }
    }

    private void Seed()
    {
        //// Wheels
        BluePrint[0, 0, 0] = 100;
        BluePrint[0, 1, 0] = 100;
        BluePrint[0, 2, 0] = 100;
        BluePrint[0, 4, 0] = 100;
        BluePrint[0, 5, 0] = 100;
        BluePrint[0, 6, 0] = 100;

        // Body
        BluePrint[0, 0, 1] = 1;
        BluePrint[0, 1, 1] = 1;
        BluePrint[0, 2, 1] = 1;
        BluePrint[0, 3, 1] = 1;
        BluePrint[0, 4, 1] = 1;
        BluePrint[0, 5, 1] = 1;
        BluePrint[0, 6, 1] = 1;

        BluePrint[0, 0, 2] = 1;
        BluePrint[0, 1, 2] = 1;
        BluePrint[0, 2, 2] = 1;
        BluePrint[0, 3, 2] = 1;
        BluePrint[0, 4, 2] = 1;
        BluePrint[0, 5, 2] = 1;
        BluePrint[0, 6, 2] = 1;
    }

	// Update is called once per frame
	void Update () 
    {
	
	}

    public void Instantiate()
    {

    }
}
