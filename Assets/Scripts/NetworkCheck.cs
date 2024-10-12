using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCheck : MonoBehaviour
{
    public GameObject NetworkProblem;
    public GameObject changeOrient;
    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            NetworkProblem.SetActive(true);  
        }
        else
        {
            NetworkProblem.SetActive(false);
        }


        if(Screen.width<Screen.height)
        {
            changeOrient.SetActive(true);
        }
        else
        {
            changeOrient.SetActive(false);
        }

    }
}
