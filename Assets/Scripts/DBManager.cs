using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    public static string username="Guest";
    public static int TotalBalance=100;
    public static int LastBet=0;
    public static string MobileNumber="9090";

    public static bool LoggedIn
    {
        get { return username != "Guest"; }
    }


    public static void LoggedOut()
    {
        username= "Guest";
        TotalBalance = 100;
        LastBet = 0;
        MobileNumber = "9090";
    }

}


