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

        if(signifier == ServerToClientSignifiers.AccountCreationComplete)
        {
            gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.MainMenu);
        }
        else if(signifier == ServerToClientSignifiers.LoginComplete)
        {
            gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.MainMenu);
        }
        else if (signifier == ServerToClientSignifiers.GameStart)
        {
            //gameSystemManager.GetComponent<GameSystemManager>().ChangeState(GameStates.TicTacToe);

            SceneManager.LoadScene("Main");
        }
        else if (signifier == ServerToClientSignifiers.OpponentPlay)
        {
            Debug.Log("opponent play!");
        }
        else if (signifier == ServerToClientSignifiers.Msg)
        {
            Text ChatText = GameObject.Find("ChatText").GetComponent<Text>();

            if (ChatText)
                ChatText.text += csv[1] + "\n";

            int Index = Random.Range(0, 3);

            GameSystemManager Mgr = gameSystemManager.GetComponent<GameSystemManager>();
            string ChatMsg = Mgr.m_ID + " : " + m_Msg[Index];

            SendMessageToHost(ClientToServerSignifiers.TicTacToeSomethingPlay + ", " + ChatMsg);

            if (ChatText)
            {
                ChatText.text += ClientToServerSignifiers.TicTacToeSomethingPlay + ", " + Mgr.m_ID + " : " + m_Msg[Index] + "\n";
            }
        }
        else if (signifier == ServerToClientSignifiers.Owner)
        {
            int Index = Random.Range(0, 3);

            GameSystemManager Mgr = gameSystemManager.GetComponent<GameSystemManager>();
            string ChatMsg = Mgr.m_ID + " : " + m_Msg[Index];

            SendMessageToHost(ClientToServerSignifiers.TicTacToeSomethingPlay + ", " + ChatMsg);

            Text ChatText = GameObject.Find("ChatText").GetComponent<Text>();

            if (ChatText)
            {
                ChatText.text += ClientToServerSignifiers.TicTacToeSomethingPlay + ", " + Mgr.m_ID + " : " + m_Msg[Index] + "\n";
            }
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
    public const int JoinQueueForGameRoom = 3;
    public const int TicTacToeSomethingPlay = 4;
    public const int ObserverJoin = 5;
}

static public class ServerToClientSignifiers
{
    public const int LoginComplete = 1;
    public const int LoginFailed = 2;
    public const int AccountCreationComplete = 3;
    public const int AccountCreationFailed = 4;
    public const int OpponentPlay = 5;
    public const int GameStart = 6;
    public const int Msg = 7;
    public const int Owner = 8;
}