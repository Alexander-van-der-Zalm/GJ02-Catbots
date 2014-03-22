using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Schematic : MonoBehaviour 
{
    //int[,] MainLayer,MovingLayer,LooseLayer;
    int[,,] Layers;
    //GameObject[,,] 
    public GameObject Wheel;
    public GameObject Block;
    public GameObject Rotator;
    public GameObject Spring;

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
                    if (go == null)
                        continue;
                    InstantiateObject(go, layer);
                }

        }
    }

    private void InstantiateObject(GameObject go, int layer)
    {
        switch (layer)
        {
            case 0:

                break;
            case 1:

                break;
            default:

                break;
        }
    }

    private GameObject GetGameObject(int block)
    {
        GameObject go;
        if (block == 0)
            return null;

        int blockNR = block / 100;

        switch (blockNR)
        {
            case 0:
                return Block;
            case 1:
                return Wheel;
            case 2:
                return Rotator;
            case 3:
                return Spring;
        }

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
