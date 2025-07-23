using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_collider_attack : MonoBehaviour
{
    public Collider_attack[] collider_Attack;
    public float dame;
   
    public void onCollider_Attack()
    {
        foreach (var collider in collider_Attack)
        {
            collider.dame = dame;
            collider.gameObject.SetActive(true);
        }    
    }
    public void offCollider_Attack()
    {
        foreach (var collider in collider_Attack)
        {
            collider.dame = 0f;
            collider.gameObject.SetActive(false);
        }
    }


}
