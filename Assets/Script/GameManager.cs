using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class GameManager : MonoBehaviour
{


   
    public List<GameObject> MissionPool;

    public int CurrentMissionNum;
   
    public GameObject CurrentTarget;//當前任務目標為何
    public bool CurrentMissionIsDone;

    public bool CollatorAble;

    public Text MissionText;/// 呈現任務目標
    public float time;//計時


    
    public string fileName;
    public List<Missionrecord> MissionRecordCollator;//儲存每次動作與結果
    
    string filePath;
    
    public event System.EventHandler<Missionrecord> missionrecord;


    [SerializeField]
     OnColliderEnter OnColiderEnter;



    // Start is called before the first frame update
    void Start()
    {



        MissionPrefInitialization();//初始化記錄檔
        CollatorAble = true;

        CurrentMissionIsDone = true;
        time = 0;
        OnColiderEnter.MissionControl += MissionCheck;
        
        
        //WorkBarManager.MissionRecord +=           //12/30 從這加上要讀
    }

    // Update is called once per frame
    void Update()
    {
        CurrentMissionNum = MissionPool.Count;
        StartMission();
        if (CurrentMissionNum != 0 && CurrentMissionIsDone == true)
        {

            RandomMission(CurrentMissionNum);

        }
        else if(CurrentMissionNum == 0 && CollatorAble==true)
        {

            MissionText.text = "任務完成";
            MissionRecordAdd(MissionRecordCollator);
            CollatorAble = false;
        
        }


    }

    public void RandomMission(int currentMissionNum)//抽出當前任務
    {
        CurrentMissionIsDone = false;
        int NowMission =(int)Random.RandomRange(0, currentMissionNum);
        
        ReadMission(NowMission);

    }

    public void ReadMission(int MissionNum)
    {
        this.CurrentTarget = this.MissionPool[MissionNum];
        MissionText.text = "當前任務目標:尋找" + this.CurrentTarget+"。";//顯示任務
        
       

    }

    public void StartMission()//遊戲開始
    {
        this.time +=Time.deltaTime; //計時開始
      
               
    }
    public void MissionCheck(object Sender, MissionControl m)//接收事件
    {
 

        if (m.Target.name.Trim() == CurrentTarget.name.Trim())//如果接受到的目標為任務所需
        {


            MissionRecordCollator.Add(new Missionrecord(CurrentTarget, m.Target, time, true));//紀錄訊息


            Destroy(m.Target.gameObject);
     
            MissionPool.Remove(this.CurrentTarget);//從物件清單中刪除當前Target

       
            CurrentMissionIsDone = true;
        

        }
        else
        {
            
            MissionRecordCollator.Add(new Missionrecord(CurrentTarget, m.Target, time, false));
        }

 


    }

    
    public void MissionPrefInitialization()//初始化csv檔
    {
        
        this.filePath = Application.streamingAssetsPath + "/" + fileName + ".csv";
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }
        StreamWriter sw = new StreamWriter(this.filePath);
        sw.WriteLine("GetTarget,CurrentTarget,Time,MissionCheck");
        sw.Flush();
        sw.Close();
        
    }

    


    public void MissionRecordAdd(List<Missionrecord> MissionRecordCollator)//紀錄每次放置物件的結果
    {
        using (StreamWriter sw = new StreamWriter(this.filePath, true))
        {
            if (MissionRecordCollator != null && MissionRecordCollator.Count != 0)
            {

                foreach (Missionrecord m in MissionRecordCollator)
                {
                  
                    sw.WriteLine($"{m.NowGetObject},{ m.CurrentTarget}, {m.UsingTime},{ m.IsTargetCorrect}"); //12 / 19 跑出的資料怪怪的
                }
            }



        }

   

    }



}

public class MissionControl: System.EventArgs
{
    public GameObject Target;//當前目標
    public MissionControl(GameObject Target)
    {
        this.Target = Target;
    }
    
}





    public class Missionrecord : MonoBehaviour
    {
        
        public string CurrentTarget;
        public string NowGetObject;

        public float UsingTime;
        public bool IsTargetCorrect;
        public Missionrecord(GameObject CurrentTarget, GameObject NowGetObject, float UsingTime, bool IsTargetCorrect)
        {
            this.CurrentTarget = CurrentTarget.name;
            this.NowGetObject = NowGetObject.name;
            this.UsingTime = UsingTime;
            this.IsTargetCorrect = IsTargetCorrect;


        }

    }


public class ThisMission : System.EventArgs  //用於紀錄
{

    public VauleType Value_Type;
    public int FoodNumChange;
    public enum VauleType
    {
        record,
        Compute
    }

    public ThisMission(VauleType Value_Type, int FoodNumChange)
    {

        this.Value_Type = Value_Type;
        this.FoodNumChange = FoodNumChange;

    }




}


