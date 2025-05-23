using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class LeaveHouseManager : MonoBehaviour
{
    private int count;
    public GameObject phone;
    public GameObject bRoomTrig;
    public GameObject lRoomTrig;
    public GameObject bedRoomTrig;
    public GameObject phoneTrig;
    public GameObject exitDoorTrig;
    public GameObject bagTrig;
    public GameObject bag;
    public GameObject InstructionsUI;
    public GameObject Interactions;
    [SerializeField] private SpoonDisplay SpoonManager;
    private bool hasBag;
    private bool keyPressed;
    private TextMeshProUGUI UItext;

    void Start()
    {
        bRoomTrig.SetActive(false);
        lRoomTrig.SetActive(false);
        bedRoomTrig.SetActive(true);
        exitDoorTrig.SetActive(false);
        bagTrig.SetActive(false);
        phoneTrig.SetActive(false);
        phone.SetActive(false);
        count = 0;
        UItext = InstructionsUI.GetComponent<TextMeshProUGUI>();
        UItext.SetText("Tu aizmirsi telefonu! Kur tu viņu atstāji?\nTu nevari doties prom bez telefona");
        Interactions.SetActive(false);
        hasBag = false;
        keyPressed = false;
    }

    void Update()
    {
        if (count == 3)
        {
            count++;
            phone.SetActive(true);
            phoneTrig.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            keyPressed = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (UItext == null) return;
        keyPressed = false;
        if (other.gameObject.tag == "bedroom")
        {
            count++;
            bRoomTrig.SetActive(true);
            lRoomTrig.SetActive(true);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "livingroom")
        {
            count++;
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "bathroom")
        {
            count++;
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "phone")
        {
            UItext.SetText("Rekur viņš ir!\nSpied [E], lai paņemtu telefonu");
        }
        if (other.gameObject.tag == "bag")
        {
            UItext.SetText("Spied [E], lai paņemtu somu");
        }
         if (other.gameObject.tag == "exit")
        {
            UItext.SetText("Spied [E], lai dotos uz skolu");
        }
        
    }
    void OnTriggerStay(Collider other)
    {
        if (UItext == null) return;
        if (other.gameObject.tag == "phone" && keyPressed)
        {
            MainManager.Instance.isPhone = true;
            phone.SetActive(false);
            other.gameObject.SetActive(false);
            UItext.SetText("Tik ilgi meklēji...\nKo gaidi? Steidzies uz ārdurvīm");
            exitDoorTrig.SetActive(true);
            keyPressed = false;
        }
        if (other.gameObject.tag == "exit" && keyPressed)
        {
            if (!hasBag)
            {
                UItext.SetText("Tu aizmirsi paņemt mugursomu!\nTu taču dodies uz skolu!");
                bagTrig.SetActive(true);
                keyPressed = false;
                other.gameObject.SetActive(false);
            }
            else
            {
                keyPressed = false;
                MainManager.Instance.isBag = true;
            }
            
        }
        if (other.gameObject.tag == "bag" && keyPressed)
        {
            UItext.SetText("Kā tu vari būt tik aizmāršīga?\nTagad tu kavē!");
            SpoonManager.UpdateSpoonCount(2);
            exitDoorTrig.SetActive(true);
            hasBag = true;
            bag.SetActive(false);
            keyPressed = false;
            other.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (UItext == null) return;
        keyPressed = false;
        if (other.gameObject.tag == "phone")
        {
            UItext.SetText("Kur tu ej?\nAtgriezies un paņem telefonu!");
        }
        if (other.gameObject.tag == "bag")
        {
            UItext.SetText("Kur tu ej?\nAtgriezies un paņem somu!");
        }
        if (other.gameObject.tag == "exit")
        {
            UItext.SetText("Kur tu ej?\nAtgriezies pie ārdurvīm");
        }
    }
}