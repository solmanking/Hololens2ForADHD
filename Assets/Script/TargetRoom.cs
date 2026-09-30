using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TargetRoom : MonoBehaviour
{

    public GameObject CurrentFood;
    public GameObject workBar;
    public List<GameObject> FoodList;
    public WorkBarManager WorkBarManager;

    public float time;


    public Text MissionShow;
    public TextMeshPro Missionshow;

    public bool CompeletethisMission;//當前任務是否可完成
    public int MissionNum;//要做幾次任務
    public bool IsRecordNow = false;
    public event System.EventHandler<RecordMission> RecordMission;

    public EyeTrackingScript EyeTrackingScript;
    public float lookatTime;

    


    // Start is called before the first frame update
    void Start()
    {
        MissionNum = 3;
        RecordMission += (sender, e) => { };
        CompeletethisMission = true;
        FoodList = workBar.GetComponent<WorkBarManager>().Food;
        WorkBarManager = workBar.GetComponent<WorkBarManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (CompeletethisMission == true && MissionNum !=0) 
        {
            RandomTarget();
            
        }
        else if(CompeletethisMission=true && MissionNum ==0 && !IsRecordNow)
        {
            
            WorkBarManager.isRecording = true;
            //記錄點
            CompeletethisMission = false;
            IsRecordNow = true;
        }
        //TargetShow();
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject != null && CurrentFood.gameObject != null && RecordMission != null)
        {

          

            if (collision.gameObject.name.Trim() == CurrentFood.gameObject.name.Trim() + "(Clone)")
            {

               

                RecordMission(this, new RecordMission(this.CurrentFood.gameObject.name, collision.gameObject.name, WorkBarManager.time, true, lookatTime,0));

                CorrectEffect();//播放"正確"音效
                

                Destroy(collision.gameObject);

                WorkBarManager.FoodNumTotal += 1;

                MissionNum -= 1;
                if (MissionNum != 0)
                {
                    CompeletethisMission = true;
                }
                else
                {
                    CompeletethisMission = false;
                    EyeTrackingScript.GetComponent<EyeTrackingScript>().isNewMissionTolooking = true;///1/5嘗試加入控管眼睛偵測
                }



            }

            else if(collision.gameObject.name.Trim() != CurrentFood.gameObject.name.Trim() + "(Clone)")//放置錯誤物件
            {

                RecordMission(this, new RecordMission(this.CurrentFood.gameObject.name, collision.gameObject.name, WorkBarManager.time, false , lookatTime,0));

                WrongEffect();

                Destroy(collision.gameObject);

                WorkBarManager.FoodNumTotal += 1;

            }


            

        }
        
    }

    public void RandomTarget()
    {
        CompeletethisMission = false;
        int NowMission = (int)Random.RandomRange(0,FoodList.Count);
        this.CurrentFood = this.FoodList[NowMission];

        EyeTrackingScript.GetComponent<EyeTrackingScript>().isNewMissionTolooking = true;///1/5嘗試加入控管眼睛偵測

    }

    public void TargetShow()
    {
        if (MissionNum != 0)
            Missionshow.text = "I Want " + CurrentFood.gameObject.name;


        else if (MissionNum == 0) {
            Missionshow.text = "Successful";

            WorkBarManager.AudioManager.Play(3, "claps", false);
        }
           
            
        
    }


    public void FullenRecord(string FullTargetname,float Fulldistance)
    {
        RecordMission(this, new RecordMission(CurrentFood.gameObject.name, FullTargetname, WorkBarManager.time, false,lookatTime, Fulldistance));
    
    }


    public void CorrectEffect()
    {
        WorkBarManager.AudioManager.Play(0,"Correct",false);
        //Debug.Log("播放音效");
    }

    public void WrongEffect()
    {

        WorkBarManager.AudioManager.Play(1, "Wrong", false);
    }
    
}
