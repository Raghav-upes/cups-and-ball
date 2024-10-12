using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class MoneyExchange : MonoBehaviour
{

    public RowUI rowUI;
    public UILogin uILogin;
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetData()
    {
        StartCoroutine(GetTransactionsHistory());
    }


    IEnumerator GetTransactionsHistory()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        uILogin.showLoader();
        UnityWebRequest www = UnityWebRequest.Post("https://hard-wearing-formul.000webhostapp.com/transactions.php", form);
        Debug.Log(DBManager.username);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            foreach (Transform child in transform)
            {
               
                Destroy(child.gameObject);
            }
            string json = www.downloadHandler.text;
            TransactionsHistory wrapper = JsonUtility.FromJson<TransactionsHistory>(json);

            string text = "";
            foreach (Transaction t in wrapper.transactions)
            {
                text += t.date + " " + t.status + " " + t.request + " " + t.amount + "\n";

                var row = Instantiate(rowUI,transform).GetComponent<RowUI>();
                row.date.text = t.date;
                row.status.text = t.status;
                row.request.text = t.request;
                row.amount.text = t.amount;
            }

            Debug.Log(text);
        }
        uILogin.hideLoader();
    }

    // Class for mapping JSON data
    [Serializable]
    public class TransactionsHistory
    {
        public Transaction[] transactions;
    }

    [Serializable]
    public class Transaction
    {
        public string request;
        public string status;
        public string date;
        public string amount;
    }

}

