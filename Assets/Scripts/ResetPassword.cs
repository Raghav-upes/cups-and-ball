using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ResetPassword : MonoBehaviour
{
    public TMP_InputField phoneNumber;
    public Button button;
    public UILogin uILogin;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void verifyNumber()
    {
        button.interactable = phoneNumber.text.Length==10 && phoneNumber.text[0]!=2 && phoneNumber.text[0] != 3;
    }


    public void callRegister()
    {
        StartCoroutine(Registration());
    }



    IEnumerator Registration()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", phoneNumber.text);
        UnityWebRequest www = UnityWebRequest.Post("https://hard-wearing-formul.000webhostapp.com/register.php", form);
        uILogin.showLoader();
        yield return www.SendWebRequest();
        uILogin.hideLoader();
        if (www.downloadHandler.text == "0")
        {
           
          

        }
        else
        {
        
            Debug.Log("This Username is already taken");
        }


    }

}
