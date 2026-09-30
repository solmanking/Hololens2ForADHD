using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

public class CardSetPosition : MonoBehaviour
{
    // Start is called before the first frame update

    private bool hasCard;


    public Vector3 SetPosition;
    
    //public BoxCollider CardColider;

    public ObjectManipulator ObjectManipulatorController;
    public Rigidbody Cardrd;

    public CardGameManager CGM;
    public bool HasHide;


    public GameObject Deck;



    void Start()
    {
        hasCard = false;

        HasHide = false;

        CGM = GameObject.Find("GameManager").GetComponent<CardGameManager>();
        Deck = GameObject.Find("Blue_ManyDeck_00");
    }

    // Update is called once per frame
    void Update()
    {
        


    }


    public void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag=="card" && hasCard==false)
        {


            CardColiderCheck(other.gameObject);




            other.transform.SetParent(this.transform);
            Vector3 CardPutPos = new Vector3(0,1,0);
            Quaternion CardShowQuaternion = Quaternion.EulerAngles(0,0,0);
            other.transform.localPosition = CardPutPos;
            other.transform.localRotation = CardShowQuaternion;



            CardFixScale(other.gameObject, Deck);//Deck


            hasCard = true;
            Debug.Log("成功");
        }      
    }

    public void OnTriggerStay(Collider other)
    {




    }


    public void CardFixScale(GameObject ThisCard,GameObject Deck)///修正卡片放置可能造成的形狀歪掉
    {
        Vector3 DeckSize = Deck.transform.localScale;
        float scaleX = DeckSize.x;
        float scaleY = DeckSize.y;
        float scaleZ = DeckSize.z;


        ThisCard.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

    }




    public void CardColiderCheck(GameObject o)
    {
        ObjectManipulatorController=o.gameObject.GetComponent<ObjectManipulator>();
        Cardrd = o.gameObject.GetComponent<Rigidbody>();
        if (ObjectManipulatorController.enabled == true)
        {
            
            ObjectManipulatorController.enabled = false;

        }
        else
        {

            ObjectManipulatorController.enabled = true;
        }


        if (Cardrd.isKinematic == false)
        {
            Cardrd.isKinematic = true;
        
        }
    
    
    }

    public bool checkHasCard()
    {
        
        bool hasCard = this.hasCard;

        return hasCard;


    }


}
