using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WheelBlock : MonoBehaviour 
{
    public List<GameObject> Mid;
    public List<GameObject> End;
    public bool IsMid;

    public void SetPiece(bool isMid = false, bool isLeft = true, int index = 0)
    {
        IsMid = isMid;
        GameObject go;
        go = isMid ? Mid[index] : End[index];
        
        if (!IsMid && !isLeft)
        {
            Vector3 localScale = go.transform.localScale;
            localScale.x *= -1;
            go.transform.localScale = localScale;
        }

        GameObject.Instantiate(go, transform.position, Quaternion.identity);
        
        DestroyImmediate(this);
        

        //return go;
    }
}
