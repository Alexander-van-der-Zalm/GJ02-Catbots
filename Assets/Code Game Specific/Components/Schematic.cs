using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Schematic : MonoBehaviour 
{
    //int[,] MainLayer,MovingLayer,LooseLayer;
    int[,,] Layers;


    //public Sprite
	
    //public enum GridType
    //{
    //    Empty,Block,Spring,Rotator,Wheel
    //}
        
        
    // Use this for initialization
	void Start () 
    {
        Init();
        Seed();
        BuildSchematic();
	}

    private void Init()
    {
        //MainLayer = new int[100, 100];
        //MovingLayer = new int[100, 100];
        //LooseLayer = new int[100, 100];
        Layers = new int[3, 30, 30];
    }

    public void BuildSchematic()
    {
        int layersAmount = Layers.GetLength(0);
        int width = Layers.GetLength(1);
        int height = Layers.GetLength(2);

        for (int layer = 0; layer < layersAmount; layer++)
        {
            for(int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    GameObject go = GetGameObject(Layers[layer, x, y]);
                }

        }
    }

    private GameObject GetGameObject(int block)
    {
        return null;
    }

    private void Seed()
    {

    }

	// Update is called once per frame
	void Update () 
    {
	
	}

    public void Instantiate()
    {

    }
}
