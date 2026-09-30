using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class OnColliderEnter : MonoBehaviour
{
    //public GameObject CurrentTarget;
    public GameObject GameManager;

    public event System.EventHandler<MissionControl> MissionControl;

 

    private void Start()
    {
        MissionControl += (sender, e) => { };
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null && MissionControl!=null)
        {
            Debug.Log("這裡" + collision.gameObject);
            MissionControl(this,new MissionControl(collision.gameObject));
        }
        else
        {

            Debug.Log("這裡沒拿到物件");
        }
        
    }
}



