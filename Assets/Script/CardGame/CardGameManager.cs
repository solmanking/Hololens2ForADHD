using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

using System.IO;

public class CardGameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public int QuestNum;//關卡次數


    public List<GameObject> CardList;//卡片組

    public int CardNum;//每局卡片數量

    public GameObject CardDeck;//牌組物件上方

    public GameObject TargetCard;//目標卡片



    public List<GameObject> PlaceList;
    public GameObject Table;
    public GameObject CardPutPlace;//卡片放置處
    public Vector3 CardPutPlaceBasePos; //= new Vector3(-0.26f,-0.3f,0.589f);



    public float time;//時間紀錄
    
    
    public bool HasShuffle;
    public List<Vector3> initialPositions;



    public List<GameObject> CardChose;
    public List<GameObject> TargetList;
    public bool RaceCanStart;//遊戲開始
    public GameObject TargetShowPlace;



    public List<RecordMissionInCardGame> RMC;
    public event System.EventHandler<RecordMissionInCardGame> RecordMissionInCardGame;
    string FilePath;
    public string fileName = "CardGame";


    //public Dictionary<GameObject,Vector3> CardPosPref;//用於儲存洗牌過後每張卡片當前的位置
    public List<GameObject> CardName;
    public List<Vector3> CardPos;

    public EyeTrackingTarget EyeTrackingforPointer;



    public EyeTrackingTarget TargetCardEyeTracking;
    public IMixedRealityGazeProvider gazeProvider;

    public GameObject TargetPosPointer;//用於把虛擬按鈕放到目標卡上直到卡片洗牌完成前可以研究目標注視狀況ㄋ
    public BoxCollider PointerColider;
    public bool PointerCanTrace;




    public CardWithEyeTracking CWT;


    void Start()
    {

        PointerColider = TargetPosPointer.GetComponent<BoxCollider>();
        PointerColider.enabled = false;
        PointerCanTrace = false;



        gazeProvider = CoreServices.InputSystem.GazeProvider;
        CardName = new List<GameObject>();
        CardPos = new List<Vector3>();

        //CSVPrefInitialization();
        this.FilePath = Application.streamingAssetsPath + "/" + fileName + ".csv";


        this.time = 0;

        RaceCanStart = false;

        initialPositions = new List<Vector3>();

        CardPutPlaceBasePos = Table.transform.position+new Vector3(-0.3f,0.13f,-0.4f);

        CardInstantiate(CardNum, CardList);

        CardPutPlaceSpawn(CardNum,CardPutPlace,CardPutPlaceBasePos);

        HasShuffle = false;
    }

    // Update is called once per frame
    void Update()
    {
        ReadyToStartShuffle();


        if (RaceCanStart == true)
        {
            StartTime();
           
        }


        if (Input.GetKey(KeyCode.P))
        {
            CSVPrefInitialization();
            Debug.Log("資料初始化完成");
        }

        if (PointerCanTrace == true)
        {
            PointerSet();
        }
        else
            PointerReset();


    }





    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public void CSVPrefInitialization()//初始化csv檔
    {

        this.FilePath = Application.streamingAssetsPath + "/" + fileName + ".csv";
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }
        StreamWriter sw = new StreamWriter(this.FilePath);
        sw.WriteLine("TargetCard,ChoseCard,IsCorrect,UsingTime,DistanceLevel,GazeCount,GazeMaxTime");
        sw.Flush();
        sw.Close();

    }

    public void CardInfoGet(object sender, RecordMissionInCardGame RMC)//接收卡片資訊
    {
        
        //this.RMC.Add(new RecordMissionInCardGame());
        Debug.Log("儲存成功");
    }

  
    public void StartTime()
    {
        this.time += Time.deltaTime;
    
    
    }

    public void CardInstantiate(int CardNum, List<GameObject> CardList)
    {

            List<GameObject> CurrentCardLsit = CardList;

            

            ////用於儲存生成的卡片與目標
            List<GameObject> CardChose = new List<GameObject>();
            List<GameObject> TargetChose = new List<GameObject>();
            ////


            int CardListNum = CurrentCardLsit.Count;
            for (int i = 1; i <= CardNum; i++)
            {
                int number = (int)Random.RandomRange(0, CardListNum);
                GameObject CurrentCard= CurrentCardLsit[number].gameObject;//當前準備要生成的是number抽到的物件
 
                Vector3 CardSpawnVector = CardDeck.transform.position+CardDeck.transform.up*0.04f;

                Quaternion CardSpawnQuaternion = Quaternion.Euler(0,0,180);

                CardSizeFix(CurrentCard, CardDeck,CardSpawnVector,CardSpawnQuaternion);         
                CardChose.Add(CurrentCard);

                CurrentCardLsit.Remove(CurrentCard);


                /////////
                //把所有抽到的卡片相同的Target放進TargetLsit中用於決定目標後的呈現
                GameObject TargetPre = TargetList[number].gameObject;

                TargetChose.Add(TargetPre);//把同樣的卡片目標放入List
                TargetList.Remove(TargetPre);
                /////////

            }

            this.CardChose = CardChose;//外部儲存所有抽到卡片的資訊
            Target(CardChose,TargetChose);

    }


    public void Target(List<GameObject> CardList,List<GameObject>TargetList)
    {
        int number = (int)Random.RandomRange(0, CardList.Count);

         TargetCard = TargetList[number].gameObject;//選擇本場遊戲的目標

        
        TargetCard.transform.localScale = new Vector3(0.03f,0.03f,0.03f);//調整大小
        
        Vector3 TargetShowPlaceVector = TargetShowPlace.transform.position + transform.up*0.2f;
        Quaternion TargetShowPlaceQuaternion = Quaternion.EulerAngles(-90, 0, 0);

        Instantiate(TargetList[number],TargetShowPlaceVector,TargetShowPlaceQuaternion);//生成跟目標卡片相同但沒有腳本的物件
        TargetCard = CardList[number].gameObject;//




        TargetCardEyeTracking = GameObject.Find(TargetCard.gameObject.name.Trim() + "(Clone)").GetComponent<EyeTrackingTarget>();
       
        


    }


 

    public void CardSizeFix(GameObject CurrentCard, GameObject Deck,Vector3 CardSpawnVector, Quaternion CardSpawnQuaternion)
    {

        
        Vector3 DeckSize = Deck.transform.localScale;

        float scaleX = DeckSize.x;
        float scaleY = DeckSize.y;
        float scaleZ = DeckSize.z;

        GameObject c=Instantiate(CurrentCard, CardSpawnVector, CardSpawnQuaternion);//生成卡片

        c.transform.SetParent(Table.transform);

        c.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        c.transform.SetParent(null);

    }

    public void CardPutPlaceSpawn(int CardNum, GameObject CardPutPalce , Vector3 BaseCardPutPlacePos)
    {

        PlaceList = new List<GameObject>();

        int boardNum = CardNum;
        Vector3 lastCardPutPlacePos=BaseCardPutPlacePos;//初始化生成第一塊磚的位置
        Quaternion CardPutPalceSpawnQuaternion = Quaternion.Euler(0, 0, 0);

        for (int i = 0; i < CardNum; i++)
        {
          GameObject n = Instantiate(CardPutPalce,lastCardPutPlacePos,CardPutPalceSpawnQuaternion);//
            PlaceList.Add(n);
            lastCardPutPlacePos = lastCardPutPlacePos +new Vector3(0.2f,0,0); 
         
        }

        

        Debug.Log(PlaceList.Count);
      
    }


    public void ReadyToStartShuffle()//確定所有磚塊都放置了卡片
    {
        int hasCardNum = 0;
        for (int i= 0; i < PlaceList.Count;i++)
            {
                bool g = PlaceList[i].GetComponent<CardSetPosition>().checkHasCard();
            if (g == true)
                hasCardNum++;
            else
                hasCardNum = 0;
            }
        if (hasCardNum == PlaceList.Count && HasShuffle==false)
        {

            HasShuffle = true;

            List<GameObject> placeListCopy = new List<GameObject>(PlaceList);
            StartCoroutine(ShuffleAnimation(placeListCopy));
            
            

        }
    
    }



    public void RecordAdd(GameObject ChoseCard, GameObject TargetCard,bool isCorrect,float time, CardWithEyeTracking CWT)//紀錄結果
    {



        float GazeCount = CWT.gazeCount;

        float GazeMaxTime = CWT.maxGazeDuration;

        int Cindex = CardName.IndexOf(ChoseCard);//得到選卡的index
        Vector3 CcardPos = CardPos[Cindex];

        Debug.Log("C=" + CcardPos );



        //得到targetCard在List中的位置
       
        for (int i = 0; i < CardName.Count; i++)
        {
            if (TargetCard.gameObject.name.Trim() + "(Clone)" == CardName[i].gameObject.name.Trim())
            {
                TargetCard = CardName[i].gameObject;
            } 
        }

        int Tindex = CardName.IndexOf(TargetCard);//得到目標卡的index
        Vector3 TcardPos = CardPos[Tindex];
        Debug.Log("T="+TcardPos);




        Vector3 DistanceVector = CcardPos - TcardPos;
        Debug.Log("兩卡差距=" + DistanceVector);


        Vector3 LevelDis = new Vector3(0.2f,0,0);//每個卡片的間隔
        
        
        
        float DistanceLevel =Mathf.Round(DistanceVector.x / LevelDis.x);//選擇卡片間距離的差距以等級作為標示

        
        string DisLevelToString;

        if (DistanceLevel > 0)
        {
            DisLevelToString = "On Right" + Mathf.Abs(DistanceLevel) + " Pawn";
        }
        else if (DistanceLevel == 0)
        {
            DisLevelToString = "Correct";

        }
        else
        {

            DisLevelToString = "On Left" + Mathf.Abs(DistanceLevel) + " Pawn";

        }

        /*
        switch(DistanceLevel)
        {
            case -2:
                DisLevelToString = "在左邊2格";
                break;
            case -1:
                DisLevelToString = "在左邊1格";
                break;
            case 0:
                DisLevelToString = "正確";
                break;
            case 1:
                DisLevelToString = "在右邊1格";
                break;
            case 2:
                DisLevelToString = "在右邊2格";
                break;

            default:
                break;
        }*/
          




        using (StreamWriter sw = new StreamWriter(this.FilePath, true))
        {

            sw.WriteLine($"{ChoseCard},{TargetCard},{ isCorrect}, {time},{DisLevelToString},{GazeCount},{GazeMaxTime}");//,{m.FullDistance} DistanceLevel
        }


    }




    public void ChoesnResult(GameObject ChoseCard)//卡片選取後得出結果
    {
        if (TargetCard.gameObject.name.Trim() + "(Clone)" == ChoseCard.gameObject.name.Trim())
        {
            Debug.Log("選擇正確");


            RecordAdd(ChoseCard, TargetCard, true, this.time, CWT);

            RaceCanStart = false;

            
        }
        else
        {

            Debug.Log("選擇錯誤");
            Debug.Log("目標是:"+TargetCard.gameObject);
            Debug.Log("選到的是:"+ChoseCard.gameObject);

            RecordAdd(ChoseCard, TargetCard, false, this.time, CWT);

            RaceCanStart = false;
        }
    
        
    
    }





    public void CardPosPref(GameObject C,Vector3 V)//
    {
        CardName.Add(C);
        CardPos.Add(V);
    
    }




    public void PointerSet()
    {

        PointerColider.enabled = true;

        GameObject T = GameObject.Find(TargetCard.gameObject.name.Trim() + "(Clone)");
        //TargetPosPointer.transform.SetParent(T.transform);
        TargetPosPointer.transform.position = T.transform.position + transform.up * 0.02f;//* -0.01f;

    }

    public void PointerReset()
    {
        
        PointerColider.enabled = false;
    }



    IEnumerator ShuffleAnimation(List<GameObject> bricks)
    {

        //TargetCardEyeTracking.enabled = true;//這裡打開目標卡片的eyetrackingTarget

        PointerCanTrace = true;


        foreach (GameObject brick in bricks)
        {
            Transform card = brick.transform.GetChild(0); // 假设卡片是子物体的第一个子对象
            Quaternion flippedRotation = Quaternion.Euler(0, 0, 180); // 设置翻转后的旋转角度
            card.localRotation = flippedRotation; // 应用翻转旋转
        }



        int shuffleCount = 6; // 洗牌次数
        Vector3[] initialPositions = new Vector3[PlaceList.Count];// 设置 initialPosition 数组大小为 PlaceList 的总数

        for (int i = 0; i < PlaceList.Count; i++)///在 initialPosition 数组中储存每个 Place 的位置三维
        {
            initialPositions[i] = PlaceList[i].transform.position;
        }

        Vector3[] finalPositions = new Vector3[PlaceList.Count]; // 保存洗牌后砖块的最终位置

        while (shuffleCount > 0)
        {
            // 随机选择两个不同的位置进行交换
            int index1 = Random.Range(0, PlaceList.Count);
            int index2 = Random.Range(0, PlaceList.Count);
            while (index2 == index1)
            {
                index2 = Random.Range(0, PlaceList.Count);
            }
            GameObject brick1 = PlaceList[index1];
            GameObject brick2 = PlaceList[index2];

            // 计算移动的方向和距离
            Vector3 moveDirection = (brick2.transform.position - brick1.transform.position).normalized;
            float moveDistance = Vector3.Distance(brick1.transform.position, brick2.transform.position);

            // 移动砖块直到到达目标位置
            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / 1f; // 根据移动时间计算插值比例
                brick1.transform.position += moveDirection * moveDistance * Time.deltaTime;
                brick2.transform.position -= moveDirection * moveDistance * Time.deltaTime;
                yield return null;
            }

            // 更新最终位置数组
            finalPositions[index1] = brick1.transform.position;
            finalPositions[index2] = brick2.transform.position;

            // 减少洗牌次数
            shuffleCount--;
        }

        // 恢复最终位置
        for (int i = 0; i < PlaceList.Count; i++)
        {
            PlaceList[i].transform.position = finalPositions[i];
        }



        // 解除卡片与砖头的关联
        foreach (GameObject brick in PlaceList)
        {

            Transform card = brick.transform.GetChild(0); // 假设卡片是子物体的第一个子对象
            
            
            card.SetParent(null);//使卡片脫離磚塊的子物件狀態

            Debug.Log("card=" + card.gameObject);

            GameObject C = card.gameObject;

            //CardPosPref(C, C.transform.position);
            CardName.Add(C);
            CardPos.Add(C.transform.position);


            ObjectManipulator objectManipulator = card.GetComponent<ObjectManipulator>();
            if (objectManipulator != null)
            {
                objectManipulator.enabled = true;
            }


        }

        //PointerReset();

        PointerCanTrace = false;

        RaceCanStart =true;
    }

}



public class RecordMissionInCardGame:System.EventArgs//用於紀錄翻牌遊戲數據
{
    public string Card;
    public string Target;

    public RecordMissionInCardGame(GameObject Card, GameObject Target)
    {

        this.Card = Card.name;
        this.Target = Target.name;

    
    }



}

