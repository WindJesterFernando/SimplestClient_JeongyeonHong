using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NetworkedClient : MonoBehaviour
{

    int[] m_ConnectionID = new int[3];
    int m_ConnectionCount = 0;
    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5491;
    byte error;
    bool isConnected = false;
    int ourClientID;
    string[] m_Msg = new string[3];
    public Text m_ChatText = null;
    public TicTacToe m_TicTacToe = null;

    GameObject gameSystemManager;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<GameSystemManager>() != null)
                gameSystemManager = go;
        }

        m_Msg[0] = "Hello";
        m_Msg[1] = "GG";
        m_Msg[2] = "I have a cute cat";

        Connect();

        //StartCoroutine("UpdateNetworkConnection");

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNetworkConnection();

    }

    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }
    
    private void Connect()
    {

        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.0.41", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);
                //SendMessageToHost("Hello from client");

            }

            else
            {
                m_ConnectionID[m_ConnectionCount] = connectionID;
                ++m_ConnectionCount;
            }
        }
    }
    
    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }
    
    public void SendMessageToHost(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        //Debug.Log("msg recieved = " + msg + ".  connection id = " + id);
        //if (msg != "Owner")
        //    m_ChatText.text += id + " : " + msg + "\n";

        //int Index = Random.Range(0, 3);

        //SendMessageToHost(m_Msg[Index]);

        string[] csv = msg.Split(',');

        int signifier = int.Parse(csv[0]);

        if (signifier == ServerToClientSignifiers.AccountCreationComplete)
        {
        }

        else if (signifier == ServerToClientSignifiers.AccountCreationFailed)
        {
        }

        else if (signifier == ServerToClientSignifiers.LoginComplete)
        {
            SceneManager.LoadScene("Chat");
        }

        else if (signifier == ServerToClientSignifiers.LoginFailed)
        {
        }

        else if (signifier == ServerToClientSignifiers.GameStart)
        {
        }

        else if (signifier == ServerToClientSignifiers.ChatMsg)
        {
            if (m_ChatText)
            {
                Debug.Log("ID 1");
                m_ChatText.text += csv[1] + "\n";
            }
        }

        else if (signifier == ServerToClientSignifiers.TicTacToePlay)
        {
            Debug.Log("TicTacToe");
            int Number = int.Parse(csv[1]);

            TicTacToe Tic = GameObject.Find("Canvas").GetComponent<TicTacToe>();

            if (Tic)
            {
                Tic.ChangeNumber(Number);
            }
        }

        else if (signifier == ServerToClientSignifiers.TicTacToeGameStart)
        {
        }

        else if (signifier == ServerToClientSignifiers.TicTacToeOwner)
        {
            TicTacToe Tic = GameObject.Find("Canvas").GetComponent<TicTacToe>();

            if (Tic)
            {
                Tic.SetOwner();
            }
        }


        else if (signifier == ServerToClientSignifiers.TicTacToeWin)
        {
        }

        else if (signifier == ServerToClientSignifiers.TicTacToeLoginComplete)
        {
            Debug.Log("Login Complete");
            SceneManager.LoadScene("TicTacToe");
        }

        else if (signifier == ServerToClientSignifiers.TicTacToeLoginFailed)
        {
        }
    }

    public bool IsConnected()
    {
        return isConnected;
    }


}

static public class ClientToServerSignifiers
{
    public const int CreateAccount = 1;
    public const int Login = 2;
    public const int ChatMsg = 3;
    public const int TicTacToeSomethingPlay = 4;
    public const int ObserverLogin = 5;
    public const int ChatBack = 6;
    public const int TicTacToeIn = 7;
    public const int TicTacToeOut = 8;
    public const int TicTacToeObserverIn = 9;
    public const int TicTacToeObserverOut = 10;
}

static public class ServerToClientSignifiers
{
    public const int LoginComplete = 1;
    public const int LoginFailed = 2;
    public const int AccountCreationComplete = 3;
    public const int AccountCreationFailed = 4;
    public const int GameStart = 5;
    public const int ChatMsg = 6;
    public const int TicTacToePlay = 7;
    public const int TicTacToeGameStart = 8;
    public const int TicTacToeOwner = 9;
    public const int TicTacToeWin = 10;
    public const int TicTacToeLoginComplete = 11;
    public const int TicTacToeLoginFailed = 12;
}
