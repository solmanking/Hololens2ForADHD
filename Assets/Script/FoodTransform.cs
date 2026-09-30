using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTransform : MonoBehaviour
{

    public bool IsOnTable;
    public GameObject Exit;


    public WorkBarManager WorkBarManager;

    //public System.EventHandler<ThisMission> FoodNumChange;
    [SerializeField]
    public TargetRoom T;
    [SerializeField]
    public GameObject TargetRoom;


    public event System.EventHandler<RecordMission> RecordMission;//1/16加入 扔物件失敗與 物件吧檯彼此距離



    // Start is called before the first frame update
    void Start()
    {

        TargetRoom = GameObject.Find("kitchen_sink");
        //TargetRoom = GetComponentInChildren<TargetRoom>().gameObject;
        T = TargetRoom.GetComponent<TargetRoom>();


        RecordMission += (sender, e) => { };


        IsOnTable = false;
        WorkBarManager = GameObject.Find("WorkBar").GetComponent<WorkBarManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (IsOnTable == true && this.transform.position.y > -5.0f)
            foodTrans();
        else if (this.transform.position.y < -0.8f)//1/16調整落下消滅距離
        {
            // WorkBarManager.FoodNumTotal += 1;
            //Destroy(this.gameObject);
            FullenDestroy();
        }
            
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.tag == "table")
        {
            IsOnTable = true;
            //Debug.Log("碰到地板");
        }
        
        if (collision.gameObject.name == "exit")
        {
            IsOnTable = false;
            WorkBarManager.FoodNumTotal += 1;
            Destroy(this.gameObject);
            //Debug.Log("摧毀");

        }

    }

     private void OnCollisionExit(Collision collision)
     {
         if (collision.gameObject.tag == "table")
             IsOnTable = false;
     }


    public void foodTrans()
    {
       
        this.gameObject.transform.Translate(0.3f*Time.deltaTime, 0, 0);

    }

    public void FullenDestroy()
    {
        float FullDistance = 0;

        FullDistance = Vector3.Distance(this.gameObject.transform.position,TargetRoom.gameObject.transform.position);

        T.FullenRecord(this.gameObject.name,FullDistance);

        //RecordMission(this, new RecordMission(T.CurrentFood.gameObject.name,this.gameObject.name, WorkBarManager.time, false,T.lookatTime,FulleDistance));
        WorkBarManager.FoodNumTotal += 1;
        Destroy(this.gameObject);
    }

}
