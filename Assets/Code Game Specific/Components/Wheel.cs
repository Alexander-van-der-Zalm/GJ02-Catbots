using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour 
{
    public enum BlockSection
    {
        Left, Mid, Right
    }
    
    public WheelBlock WheelBlock;
    public GameObject Parent;
    public BlockSection Section;


    public Wheel SetPiece(BlockSection section, int index = 0)
    {
        GameObject prefab;
        prefab = section == BlockSection.Mid ? WheelBlock.Mid[index] : WheelBlock.End[index];

        //Debug.Log(section.ToString() + " " + prefab.GetComponent<Wheel>().Section);

        Vector3 offset = Vector3.zero;
        if (section == BlockSection.Right)
            offset.x++;
        else if (section == BlockSection.Mid)
            offset.x --;

        GameObject go = GameObject.Instantiate(prefab, transform.position + offset, Quaternion.identity) as GameObject;
        
        if (section == BlockSection.Right)
        {
            Vector3 localScale = go.transform.localScale;
            localScale.x *= -1;
            go.transform.localScale = localScale;
        }

        Wheel wheel = go.GetComponent<Wheel>();
        wheel.Section = section;
        wheel.Parent = Parent;
        go.transform.parent = transform.parent;

        DestroyImmediate(gameObject);

        return wheel;
    }
}
