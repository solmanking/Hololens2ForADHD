using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WorkBarManager : MonoBehaviour
{

    public List<GameObject> Food;
    public GameObject Entrance;//入口
    public GameObject Exit;//出口
    public int FoodNumTotal;
    public float time;


    public List<TargetInfoPref> RecordCollator;
    public event System.EventHandler<RecordMission> RecordMission;
    [SerializeField]
    TargetRoom TargetRoom;


    string FilePath;
    public string fileName;
    public bool isRecording;

    public AudioManager AudioManager;


    public Animator AnimaInGirl;


    // Start is called before the first frame update
    void Start()
    {
        isRecording = false;



        AnimaInGirl = GetComponent<Animator>();

       

        //CSVPrefInitialization();//初始化
        this.FilePath = Application.streamingAssetsPath + "/" + fileName + ".csv";


        FoodNumTotal = Food.Count;//用於保證生成物件的數量      
        InvokeRepeating("FoodInstantiate",0,2.0f);

        TargetRoom.RecordMission += MissionCompelete;///

       
    }

    // Update is called once per frame
    void Update()
    {

        StartMission();

        if (isRecording == true)
        {
            RecordAdd(RecordCollator);
            isRecording = false;
            Application.Quit();//退出遊戲
        }

        if (Input.GetKey(KeyCode.P))
        {
            CSVPrefInitialization();
            Debug.Log("資料初始化完成");
        }
    }

    
    private void FoodInstantiate()
    {
        if (FoodNumTotal > 0)
        {
            int number = (int)Random.RandomRange(0, Food.Count);
            GameObject NowFood = Food[number].gameObject;

            Vector3 Spawnvector = Entrance.transform.position+ Entrance.transform.up*0.4f;
            Quaternion SpawnQuaternion = NowFood.transform.rotation;

            Instantiate(NowFood, Spawnvector, SpawnQuaternion);

            //Debug.Log("生成成功");
            FoodNumTotal -= 1;
            
        }

    }

    public void MissionCompelete(object sender, RecordMission R)//確認任務資訊
    {
        RecordCollator.Add(new TargetInfoPref(R.CurrentTarget,R.CatchObject,R.UsingTime,R.isTargetCorrect,R.LookatTime,R.FullDistance));
        Debug.Log("儲存成功");
    }



    public void CSVPrefInitialization()//初始化csv檔
    {

        this.FilePath = Application.streamingAssetsPath + "/" + fileName + ".csv";
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }
        StreamWriter sw = new StreamWriter(this.FilePath);
        sw.WriteLine("CurrentTarget,LookingTime,CatchTarget,UsingTime,IsCorrect,FullDistance");
        sw.Flush();
        sw.Close();

    }


     public void RecordAdd(List<TargetInfoPref> TargetInfoPrefCollator)//紀錄每次放置物件的結果
    {
        using (StreamWriter sw = new StreamWriter(this.FilePath, true))
        {
            if (TargetInfoPrefCollator != null && TargetInfoPrefCollator.Count != 0)
            {

                foreach (TargetInfoPref m in TargetInfoPrefCollator)
                {
                  
                    sw.WriteLine($"{m.CurrentTarget},{m.LookatTime},{ m.CatchObject}, {m.UsingTime},{ m.isTargetCorrect},{m.FullDistance}"); 
                }
            }

        }

    }

    public void StartMission()//遊戲開始
    {
        this.time += Time.deltaTime; //計時開始
    }







}


public class TargetInfoPref : MonoBehaviour
{

    

    public string CurrentTarget;
    public string CatchObject;
    public float UsingTime;
    public bool isTargetCorrect;


    public float LookatTime;
    public float FullDistance;
    public TargetInfoPref(string CurrentTarget, string CatchObject, float UsingTime, bool isTargetCorrect , float LookatTime, float FullDistance)
    {
        this.CurrentTarget = CurrentTarget;
        this.CatchObject = CatchObject;
        this.UsingTime = UsingTime;
        this.isTargetCorrect = isTargetCorrect;

        this.LookatTime = LookatTime;
        this.FullDistance = FullDistance;
    }

}


public class RecordMission : System.EventArgs
{
    public string CurrentTarget;
    public string CatchObject;
    public float UsingTime;
    public bool isTargetCorrect;

    public float LookatTime;
    public float FullDistance;

    public RecordMission(string CurrentTarget,string CatchObject, float UsingTime,bool isTargetCorrect, float LookatTime, float FullDistance)
    {
        this.CurrentTarget = CurrentTarget;
        this.CatchObject = CatchObject;
        this.UsingTime = UsingTime;
        this.isTargetCorrect = isTargetCorrect;

        this.LookatTime = LookatTime;
        this.FullDistance = FullDistance;

    }

}





