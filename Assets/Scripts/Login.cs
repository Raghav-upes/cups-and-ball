using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_Text msg;
    public Button submitBtn;
    public GameObject Signino;
    public UILogin uiLogin;

    [DllImport("__Internal")]
    private static extern void saveUsername(string key, string Value);



    public void callRegister()
    {
        StartCoroutine(LoginStart());
    }



    IEnumerator LoginStart()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", username.text);
        form.AddField("password", password.text);
        UnityWebRequest www = UnityWebRequest.Post("https://hard-wearing-formul.000webhostapp.com/login.php", form);
        uiLogin.showLoader();
        yield return www.SendWebRequest();
        uiLogin.hideLoader();
        if (www.downloadHandler.text[0] == '0')
        {
            DBManager.username=username.text;
            DBManager.TotalBalance = int.Parse(www.downloadHandler.text.Split('\t')[1]);
            DBManager.LastBet = int.Parse(www.downloadHandler.text.Split('\t')[2]);
            DBManager.MobileNumber = www.downloadHandler.text.Split('\t')[3];
            username.text = "";
            string dop=password.text;
            password.text = "";
            msg.text = "Enter Username and Password";
            Signino.SetActive(false);
            Debug.Log(www.downloadHandler.text);
            uiLogin.showLogout();
            saveUsername("userName", DBManager.username+"\t"+dop);

        }
        else
        {
            msg.text = "Wrong Credentials";
            Debug.Log("UserLoginn Falied"+www.downloadHandler.text);
        }
     

    }


}
