using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{
    GameObject submitButton, userNameInput, passwordInput, observerToggle;


    GameObject networkedClient;

    GameObject joinGameRoomButton;

    GameObject textNameInfo, textPasswordInfo;

    GameObject TicTacToeSquareButton;

    public string m_ID;
    public string m_Password;
    //static GameObject instance;

    void Start()
    {

        //instance = this.gameObject;

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach(GameObject go in allObjects)
        {
            if (go.name == "UserNameInputField")
                userNameInput = go;
            else if (go.name == "PasswordInputField")
                passwordInput = go;
            else if (go.name == "SubmitButton")
                submitButton = go;
            else if (go.name == "NetworkedClient")
                networkedClient = go;
            else if (go.name == "JoinGameRoomButton")
                joinGameRoomButton = go;
            else if (go.name == "TextInfo")
                textNameInfo = go;
            else if (go.name == "TextInfo (1)")
                textPasswordInfo = go;
            else if (go.name == "TicTacToeSquareButton")
                TicTacToeSquareButton = go;
            else if (go.name == "ObserverToggle")
                observerToggle = go;
        }

        submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);

        observerToggle.GetComponent<Toggle>().onValueChanged.AddListener(ObserverToggleChanged);

        joinGameRoomButton.GetComponent<Button>().onClick.AddListener(JoinGameRoomButtonPressed);
        TicTacToeSquareButton.GetComponent<Button>().onClick.AddListener(TicTacToeSquareButtonPressed);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitButtonPressed()
    {
        m_Password = passwordInput.GetComponent<InputField>().text;
        m_ID = userNameInput.GetComponent<InputField>().text;

        string msg;

        msg = ClientToServerSignifiers.CreateAccount + ", " + m_ID + ", " + m_Password;

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        Debug.Log(msg);
    }
    public void ObserverToggleChanged(bool newValue)
    {
        //Debug.Log("yoyo");

        observerToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);
    }

    public void JoinGameRoomButtonPressed()
    {
        m_Password = passwordInput.GetComponent<InputField>().text;
        m_ID = userNameInput.GetComponent<InputField>().text;

        if (observerToggle.GetComponent<Toggle>().isOn)
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.ObserverLogin + ", " + m_ID + ", " + m_Password);

        else
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.Login + ", " + m_ID + ", " + m_Password);
    }

    public void TicTacToeSquareButtonPressed()
    {
        m_Password = passwordInput.GetComponent<InputField>().text;
        m_ID = userNameInput.GetComponent<InputField>().text;

        Debug.Log("ID : " + m_ID + " Pass : " + m_Password);

        if (observerToggle.GetComponent<Toggle>().isOn)
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToeObserverIn + ", " + m_ID + ", " + m_Password);

        else
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToeIn + ", " + m_ID + ", " + m_Password);
    }
}
