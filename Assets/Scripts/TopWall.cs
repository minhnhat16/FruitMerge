using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopWall : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("MergeCircle"))
        {
            IngameController.instance.isGameOver = true;    
        }
    }
}
