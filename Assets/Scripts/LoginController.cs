using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.CloudSave;
using Unity.Services.Authentication;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine.Networking;
using UnityEngine.Windows;

public class LoginController : MonoBehaviour
{
    private UILogin uILogin;
    public TMP_Text playerName;
    public TMP_Text totalBalance;
    public TMP_Text lastBet;
    public TMP_Text Total_balance_popup;
    public TMP_Text Last_Bet_popup;
    public TMP_InputField BetAmount;
    public GameObject gate;


    private bool waitKaro=true;

    public TMP_Text WinAmount;
    public TMP_Text LoseAmount;

   

    private string namePlayer;

    private int MaxAmount;


    public GameObject loader;


    public Shuffling sf;

    public Button confirmBtn;

    public GameObject Tutorial;

    public async Task WinHoGayaAsync()
    {
        loader.SetActive(true);
        WinAmount.text= "+"+(int.Parse(lastBet.text)*2).ToString();
        waitKaro = true;
        Debug.Log("lp");
       await AddMoneyInTotalBalance(int.Parse(lastBet.text) * 2, int.Parse(lastBet.text));

        sf.ResetCups();

        ShowUiUInfo();
        Debug.Log("lpoo");
        loader.SetActive(false);
        verifyBet();
    }

    public void LoseHoGaya()
    {
       StartCoroutine(sf.showLoseCups());

        LoseAmount.text="-"+lastBet.text;
        verifyBet();

    }

    private void Awake()
    {
  
        uILogin=GetComponent<UILogin>();
    }
    public void Login()
    {

        playerName.text = DBManager.username;
        ShowUiUInfo();
       
        uILogin.changeScene();

        if (playerName.text == "Guest")
        {
            Tutorial.SetActive(true);
        }
    }


    


    public async Task<bool> placeBet()
    {
        int total_Amount = int.Parse(totalBalance.text);
        int bet_Amount=int.Parse(BetAmount.text);
        if (bet_Amount == 0)
        {
            return false;
        }
        else
        {
            loader.SetActive(true);
           await AddMoneyInTotalBalance(-bet_Amount,int.Parse(BetAmount.text));
     
                lastBet.text = BetAmount.text;
                Last_Bet_popup.text = lastBet.text;
                totalBalance.text = (total_Amount - bet_Amount).ToString();
                Total_balance_popup.text = totalBalance.text;
                MaxAmount = int.Parse(totalBalance.text);
                gate.SetActive(true);
                loader.SetActive(false);
                return true;
            
            
        }
    
    }



    public void ShowUiUInfo()
    {
        playerName.text = DBManager.username;
        totalBalance.text = (DBManager.TotalBalance).ToString();
        Total_balance_popup.text=totalBalance.text;
        lastBet.text= (DBManager.LastBet).ToString();
        Last_Bet_popup.text=lastBet.text;
        MaxAmount=int.Parse(totalBalance.text);
    }


    public void verifyBet()
    {
        int total_Amount = int.Parse(totalBalance.text);
        if (BetAmount.text.Length > 0)
        {
            int result;
            int bet_Amount;
            int.TryParse(BetAmount.text,out bet_Amount);
   
            confirmBtn.interactable = BetAmount.text.Length > 0 && int.TryParse(BetAmount.text, out result) && BetAmount.text.Length > 0 && bet_Amount > 0 && total_Amount >= bet_Amount;
        }
        else
        {
            confirmBtn.interactable = false;
        }

    }


    public async Task AddMoneyInTotalBalance(int amount,int lastBet)
    {

        WWWForm form = new WWWForm();

        form.AddField("name", DBManager.username);
        form.AddField("TotalBalance", DBManager.TotalBalance + amount);
        form.AddField("LastBet", lastBet);
        if (DBManager.username != "Guest")
        {
            UnityWebRequest www = UnityWebRequest.Post("https://hard-wearing-formul.000webhostapp.com/updateValue.php", form);

            var operation = www.SendWebRequest();

            // Wait until the operation is done
            while (!operation.isDone)
            {
               
                await Task.Yield();
            }

            if (www.downloadHandler.text[0] == '0')
            {
                DBManager.TotalBalance = DBManager.TotalBalance + amount;
                DBManager.LastBet = lastBet;
                Debug.Log(www.downloadHandler.text);

            }
            else
            {

                Debug.Log(www.downloadHandler.text);
            }
        }
        else
        {
            DBManager.TotalBalance = DBManager.TotalBalance + amount;
            DBManager.LastBet = lastBet;
        }

    }





}
