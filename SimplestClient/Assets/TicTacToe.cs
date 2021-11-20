using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToe : MonoBehaviour
{
    // Start is called before the first frame update
    public Text[] TextArray;
    public bool[] IsClicked = new bool[9];
    private bool Turn = false;
    private bool Observer = false;
    GameObject networkClient;
    public GameObject ReplayButtonObj;
    public GameObject WinText;

    public void SetOwner()
    {
        Turn = true;
    }
    void Start()
    {
        for(int i = 0;i < 9; ++i)
        {
            IsClicked[i] = false;
        }

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.name == "NetworkedClient")
            {
                networkClient = go;

                networkClient.GetComponent<NetworkedClient>().m_TicTacToe = this;
                break;
            }
        }

        Observer = networkClient.GetComponent<NetworkedClient>().m_Observer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Button1Click()
    {
        if (Observer)
            return;

        if (IsClicked[0] || !Turn)
            return;

        TextArray[0].text = "O";
        IsClicked[0] = true;
        Turn = false;

        string msg;

        msg = ClientToServerSignifiers.TicTacToeSomethingPlay + ", 0";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void Button2Click()
    {
        if (Observer)
            return;

        if (IsClicked[1] || !Turn)
            return;

        TextArray[1].text = "O";
        IsClicked[1] = true;
        Turn = false;

        string msg;

        msg = ClientToServerSignifiers.TicTacToeSomethingPlay + ", 1";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void Button3Click()
    {
        if (Observer)
            return;

        if (IsClicked[2] || !Turn)
            return;

        TextArray[2].text = "O";
        IsClicked[2] = true;
        Turn = false;

        string msg;
        
        msg = ClientToServerSignifiers.TicTacToeSomethingPlay + ", 2";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void Button4Click()
    {
        if (Observer)
            return;

        if (IsClicked[3] || !Turn)
            return;

        TextArray[3].text = "O";
        IsClicked[3] = true;
        Turn = false;

        string msg;

        msg = ClientToServerSignifiers.TicTacToeSomethingPlay + ", 3";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void Button5Click()
    {
        if (Observer)
            return;

        if (IsClicked[4] || !Turn)
            return;

        TextArray[4].text = "O";
        IsClicked[4] = true;
        Turn = false;

        string msg;

        msg = ClientToServerSignifiers.TicTacToeSomethingPlay + ", 4";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void Button6Click()
    {
        if (Observer)
            return;

        if (IsClicked[5] || !Turn)
            return;

        TextArray[5].text = "O";
        IsClicked[5] = true;
        Turn = false;

        string msg;

        msg = ClientToServerSignifiers.TicTacToeSomethingPlay + ", 5";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void Button7Click()
    {
        if (Observer)
            return;

        if (IsClicked[6] || !Turn)
            return;

        TextArray[6].text = "O";
        IsClicked[6] = true;
        Turn = false;

        string msg;

        msg = ClientToServerSignifiers.TicTacToeSomethingPlay + ", 6";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void Button8Click()
    {
        if (Observer)
            return;

        if (IsClicked[7] || !Turn)
            return;

        TextArray[7].text = "O";
        IsClicked[7] = true;
        Turn = false;

        string msg;

        msg = ClientToServerSignifiers.TicTacToeSomethingPlay + ", 7";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
    public void Button9Click()
    {
        if (Observer)
            return;

        if (IsClicked[8] || !Turn)
            return;

        TextArray[8].text = "O";
        IsClicked[8] = true;
        Turn = false;

        string msg;

        msg = ClientToServerSignifiers.TicTacToeSomethingPlay + ", 8";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }

    public void ChangeNumber(int Number, string changeText)
    {
        TextArray[Number].text = changeText;
        IsClicked[Number] = true;
        Turn = true;

        Debug.Log("Change Number : " + Number);
    }

    public void ReplayButton()
    {
        Turn = false;

        for (int i = 0; i < 9; ++i)
        {
            TextArray[i].text = "";
        }

        string msg;

        msg = ClientToServerSignifiers.Replay + "";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }
}
