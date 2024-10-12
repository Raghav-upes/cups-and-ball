using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class UILogin : MonoBehaviour
{
    public GameObject gameBoard;
    public GameObject loginPanel;
   public GameObject BackgroundPanel;
    public GameObject BetPopUP;
    public GameObject signIn;

   public GameObject WinPanel;
    public GameObject LosePanel;

    public GameObject LogoutBtn;

    LoginController loginController;

    public Button gate;

    public GameObject loader;

    public Login lg;

    public TMP_Text textPlay;

    public GameObject msg1;
    public Button TransactionBtn;
    public GameObject user1D;
    public GameObject user2D;


    public TMP_Text username1;
    public TMP_Text username2;

    public TMP_Text TotalBalance1;
    public TMP_Text TotalBalance2;

    public TMP_Text Mnumber;


    public TMP_Text RandomNum;

    private string telegramLink="www.google.com";

    [DllImport("__Internal")]
    private static extern string loadUsername();

    [DllImport("__Internal")]
    private static extern void deleteUsername();
    private string url= "https://hard-wearing-formul.000webhostapp.com/telegram.json";




    public void loadUserfromJS()
    {
 
        string str = loadUsername();
        if (str != "0")
        {
            Debug.Log(str);
            lg.username.text = str.Split('\t')[0];
            lg.password.text = str.Split('\t')[1];
            lg.callRegister();
           showLogout();
        }

        

    }

    public void showLoader()
    {
        loader.SetActive(true);
    }

    public void hideLoader()
    {
        loader.SetActive(false);
    }
    private void Start()
    {
        RandomNum.text = UnityEngine.Random.Range(200, 500).ToString();
        loadUserfromJS();
    }

    public void showLogout()
    {
        textPlay.text = "Play";
        signIn.SetActive(false);
        LogoutBtn.SetActive(true);
        TransactionBtn.interactable = true;
    
    }

    public void showLogin()
    {
        textPlay.text = "Play as guest";
        signIn.SetActive(true);
        LogoutBtn.SetActive(false);
        TransactionBtn.interactable = false;
   
    }






    public void delUserfromJS()
    {
        showLogin();
        DBManager.LoggedOut();
        deleteUsername();
  

    }

    private void Awake()
    {  
        loginController = GetComponent<LoginController>();  
      
    }

    public void opentelegram()
    {
        Debug.Log(telegramLink);
        Application.OpenURL(telegramLink);
    }


    IEnumerator getData()
    {
        showLoader(); // Assuming you have a function to show a loading indicator

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error fetching data: " + www.error);
            // Handle error appropriately
        }
        else
        {
          Dataop op=  JsonUtility.FromJson<Dataop>(www.downloadHandler.text);
          telegramLink=op.telegramLink;

        }

        hideLoader(); // Assuming you have a function to hide the loading indicator
    }

    class Dataop
    {
       public string telegramLink;
    }

    public void processJson()
    {
        StartCoroutine(getData());
    }

    public void changeScene()
    {
        gameBoard.SetActive(true);
        loginPanel.SetActive(false);
        BackgroundPanel.SetActive(true);
        BetPopUP.SetActive(true);
    }

    public void BackScene()
    {
        gameBoard.SetActive(false);
        loginPanel.SetActive(true);
        BackgroundPanel.SetActive(false);
        BetPopUP.SetActive(false);
    }
    public async void closePopup()
    {
        if (!await (loginController.placeBet()))
            return;
        BetPopUP.SetActive(false);
        msg1.SetActive(false);
    }

    public async Task VictoryAsync()
    {
       
         await loginController.WinHoGayaAsync();
         WinPanel.SetActive(true);
        msg1.SetActive(true);
    }
    public void Defeted()
    {

        loginController.LoseHoGaya();
       StartCoroutine(someDelay());
    }
    IEnumerator someDelay()
    {
        yield return new WaitForSeconds(2.5f);
        LosePanel.SetActive(true);
        msg1.SetActive(true);
    }

    public void changeUserDetails()
    {
            if(DBManager.username=="Guest")
        {
            user1D.SetActive(true);
            user2D.SetActive(false);
        }
        else
        {
            user1D.SetActive(false);
            user2D.SetActive(true);
        }
        username1.text = username2.text = DBManager.username;
        TotalBalance1.text = TotalBalance2.text = DBManager.TotalBalance.ToString();
        Mnumber.text = DBManager.MobileNumber;
    }

}
