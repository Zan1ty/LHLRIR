using UnityEngine;
using System.Collections;

public static class Constants
{
    static int rightyPlayerNum = 1;
    static string winner;
    static int leftyAmount = 3;
  
    public static int RightyPlayerNum
    {
        get { return rightyPlayerNum; }
        set { rightyPlayerNum = value; }
    }

    public static string Winner
    {
        get { return winner; }
        set { winner = value; }
    }

    public static int LeftyAmount
    {
        get { return leftyAmount; }
        set { leftyAmount = value; }
    }
}
