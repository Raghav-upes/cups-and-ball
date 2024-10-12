using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField PhoneNumber;
    public TMP_Text msg;
    public Button submitBtn;

    [DllImport("__Internal")]
    private static extern void saveUsername(string key,string Value);

    public UILogin uILogin;




    public void callRegister()
    {
        StartCoroutine(Registration());
    }

  

    IEnumerator Registration()
    {
        WWWForm form= new WWWForm();
        form.AddField("name", username.text);
        form.AddField("password", password.text);
        form.AddField("number",PhoneNumber.text);
        UnityWebRequest www = UnityWebRequest.Post("https://hard-wearing-formul.000webhostapp.com/register.php", form);
        uILogin.showLoader();
        yield return www.SendWebRequest();
        uILogin.hideLoader();
        if (www.downloadHandler.text=="0")
            {
            Debug.Log("Created");
            DBManager.username = username.text;
            DBManager.MobileNumber = PhoneNumber.text;
            this.transform.parent.gameObject.SetActive(false);
            username.text = "";
            password.text = "";
            PhoneNumber.text = "";
            uILogin.showLogout();
            saveUsername("userName",DBManager.username);
         
        }
            else
            {
                Debug.Log(www.downloadHandler.text);
            msg.text = "This Username is already taken";
            }

    
    }


  

    public void verify()
    {

        submitBtn.interactable = (username.text.Length >= 8 && password.text.Length >= 8 && PhoneNumber.text.Length == 10 && PhoneNumber.text[0] != '1' && PhoneNumber.text[0] != '2');
        if(submitBtn.interactable ) 
        msg.text = "Click on button to Register";
    }

}
