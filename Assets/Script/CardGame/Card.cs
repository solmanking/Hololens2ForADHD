using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update


    Quaternion ThisCardQuaternion;
    Quaternion Q = Quaternion.Euler(0, 0, 90);
    public bool ChoseThis;

    public CardGameManager CGM;

    public event System.EventHandler<RecordMissionInCardGame> RecordMissionInCardGame;
    void Start()
    {
        CGM = GameObject.Find("GameManager").GetComponent<CardGameManager>();


        ChoseThis = false;
        //RecordMissionInCardGame +=()
    
            
    }

    // Update is called once per frame
    void Update()
    {
        //if(ChoseThis == false)
        //DetectedRotation();
        if (CGM.RaceCanStart == true && ChoseThis == false)
        {
            Debug.Log("正在運行");
            InvokeRepeating("DetectedRotation", 0f, 1.0f);

        }
    }


    public void DetectedRotation()
    {
        ThisCardQuaternion = this.transform.localRotation;
        if (ThisCardQuaternion.z <= Q.z && ChoseThis==false)
        {
            

            ChoseThis = true;

            Debug.Log("開始比較");
            CGM.ChoesnResult(this.gameObject);
        }
    }
}
