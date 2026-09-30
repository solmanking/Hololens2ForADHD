using System.Collections;
using System.Collections.Generic;

using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

using UnityEngine.EventSystems;

public class EyeTrackingScript : MonoBehaviour
{

    private bool isGazing = false;
    private float gazeDuration = 0.0f;
    private float requiredGazeDuration = 2.0f; // 設定需要注視的時間
    public GameObject TargetRoom;

    public GameObject workBar;
    public WorkBarManager WorkBarManager;


    public bool isNewMissionTolooking;
    public float UsingTime;



    

    // Start is called before the first frame update
    void Start()
    {
        WorkBarManager = workBar.GetComponent<WorkBarManager>();



        Debug.Log("開始動作");
    }

   
    void Update()
    {
       
    }


    public void GazeMission()
    {
          if (CoreServices.InputSystem.GazeProvider.GazeTarget)
          {
              // 如果正在注視物體，增加注視時間
              gazeDuration += Time.deltaTime;

               

            if (gazeDuration >= requiredGazeDuration && !isGazing  && isNewMissionTolooking == true)//嘗試加入
              {
                isGazing = true;
                TargetShow();

              }
          }
       

    }


    public void TargetLost()
    {
        gazeDuration = 0.0f;
        isGazing = false;
        Debug.Log("IsGazing=" + isGazing);
    }



    public void Targetstart()
    {

        Debug.Log("成功");
    }

    public void TargetShow()
    {
        if (isGazing == true)
        {

            WorkBarManager.AudioManager.Play(2,"thinking",false);

            TargetRoom.GetComponent<TargetRoom>().TargetShow();
            //把眼睛追蹤到的時間紀錄
            TargetRoom.GetComponent<TargetRoom>().lookatTime = WorkBarManager.time;

            gazeDuration = 0;

            isNewMissionTolooking = false;
            isGazing = false;

            Debug.Log("isGazing="+isGazing );


            
        }
    
    
    }


}


