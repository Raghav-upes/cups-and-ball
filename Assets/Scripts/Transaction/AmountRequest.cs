using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class AmountRequest : MonoBehaviour
{
    string RequestType="Recharge";

    public TMP_Text requestName;
    public TMP_Text requestName2;
    public TMP_Text AmountBalance;


    public UILogin uILogin;
    public LoginController loginController;

    public TMP_InputField RequestAmount;
    public Button Send;
    void Start()
    {
        AmountBalance.text = DBManager.TotalBalance.ToString();
    }


 public async void DonDataAsync()
    {
        if (RequestType == "Withdraw")
        {
            await loginController.AddMoneyInTotalBalance(-int.Parse(RequestAmount.text), DBManager.LastBet);
            loginController.ShowUiUInfo();
            AmountBalance.text = DBManager.TotalBalance.ToString();

        }
    }

    public void verifyAmount()
    {
        int result;
        int ReqAmo;
        int.TryParse(RequestAmount.text,out ReqAmo);
 
            if (RequestType == "Withdraw")
            {
                Send.interactable = int.TryParse(RequestAmount.text, out result) && RequestAmount.text.Length >0 && ReqAmo != 0 && ReqAmo <= DBManager.TotalBalance && ReqAmo > 0;
            }
            else
            {

                Send.interactable = int.TryParse(RequestAmount.text, out result) && RequestAmount.text.Length > 0 && ReqAmo != 0 && ReqAmo > 0;
            }
        
    }


    public void SendRequest()
    {
        StartCoroutine(ReqRegistration());
    }


    IEnumerator ReqRegistration()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        form.AddField("amount", RequestAmount.text);
        form.AddField("requestType",RequestType);
        form.AddField("number", DBManager.MobileNumber);
        UnityWebRequest www = UnityWebRequest.Post("https://hard-wearing-formul.000webhostapp.com/reqregister.php", form);
        uILogin.showLoader();
        yield return www.SendWebRequest();
        uILogin.hideLoader();
        if (www.downloadHandler.text == "0")
        {
            Debug.Log("Request Send");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }


    }

    public void changeRequest(string requestType)
    {
        RequestType = requestType;

        if (requestType == "Withdraw")
        {
            requestName.text = "Withdraw Request";
            requestName2.text = "Enter withdraw amount";
        }
        else
        {
            requestName.text = "Recharge Request";
            requestName2.text = "Enter recharge amount";
        }

    }

}
