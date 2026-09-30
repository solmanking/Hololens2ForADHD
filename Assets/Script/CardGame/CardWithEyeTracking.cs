using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

using System.IO;


public class CardWithEyeTracking : MonoBehaviour
{

    private bool isGazing = false;
    public int gazeCount = 0; // 观察次数
    public float maxGazeDuration = 0f; // 最长持续观察时间


    public float GazeStartTime = 0;//觀察開始前注視的時間
    public float GazeKeepTime = 0;//觀察持續的時間
    private float gazeStartNeed = 0.5f; // 观察开始所需时间

    public EyeTrackingTarget EyeTrackingThisCard;

    
    // Start is called before the first frame update
    void Start()
    {
        EyeTrackingThisCard = this.GetComponent<EyeTrackingTarget>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void test()
    {
        Debug.Log("成功注視");
    }

    public void GazeCardTimeAndCount()
    {
        if (CoreServices.InputSystem.GazeProvider.GazeTarget)
        {
            GazeStartTime += Time.deltaTime;

           
            if (GazeStartTime >= gazeStartNeed && !isGazing)//嘗試加入   && isNewMissionTolooking == true
            {

                Debug.Log("開始觀察");

                isGazing = true;           
                LookAtTarget();
            }



        }

    }

    public void TargetLost()
    {
        GazeStartTime = 0.0f;
        GazeKeepTime = 0.0f;

        isGazing = false;
        Debug.Log("以重製時間 IsGazing=" + isGazing);
    }



    public void LookAtTarget()
    {
        gazeCount++;
        GazeKeepTime +=Time.deltaTime;

       

        if (maxGazeDuration < GazeStartTime)
        {
            maxGazeDuration = GazeStartTime + gazeStartNeed;
        }
        

    
    }



}
