using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{
    GameObject submitButton, userNameInput, passwordInput, createToggle, loginToggle;


    GameObject networkedClient;

    GameObject joinGameRoomButton;

    GameObject textNameInfo, textPasswordInfo;

    GameObject TicTacToeSquareButton;

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
            else if (go.name == "CreateToggle")
                createToggle = go;
            else if (go.name == "LoginToggle")
                loginToggle = go;
            else if (go.name == "NettowkredClient")
                networkedClient = go;
            else if (go.name == "JoinGameRoomButton")
                joinGameRoomButton = go;
            else if (go.name == "TextInfo")
                textNameInfo = go;
            else if (go.name == "TextInfo (1)")
                textPasswordInfo = go;
            else if (go.name == "TicTacToeSquareButton")
                TicTacToeSquareButton = go;
        }

        submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);

        loginToggle.GetComponent<Toggle>().onValueChanged.AddListener(LoginToggleChanged);
        createToggle.GetComponent<Toggle>().onValueChanged.AddListener(CreateToggleChanged);

        joinGameRoomButton.GetComponent<Button>().onClick.AddListener(JoinGameRoomButtonPressed);
        TicTacToeSquareButton.GetComponent<Button>().onClick.AddListener(TicTacToeSquareButtonPressed);

        ChangeState(GameStates.LoginMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitButtonPressed()
    {
        string p = passwordInput.GetComponent<InputField>().text;
        string n = userNameInput.GetComponent<InputField>().text;

        string msg;
        
        if(createToggle.GetComponent<Toggle>().isOn)
            msg = ClientToServerSignifiers.CreateAccount + ", " + n + ", " + p;
        else
            msg = ClientToServerSignifiers.Login + ", " + n + ", " + p;


        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
        Debug.Log(msg);
    }

    public void LoginToggleChanged(bool newValue)
    {
        //Debug.Log("yoyo");

        createToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);
    }

    public void CreateToggleChanged(bool newValue)
    {
        loginToggle.GetComponent<Toggle>().SetIsOnWithoutNotify(!newValue);
    }

    public void ChangeState(int newState)
    {
        submitButton.SetActive(false);
        userNameInput.SetActive(false);
        passwordInput.SetActive(false);
        createToggle.SetActive(false);
        loginToggle.SetActive(false);

        textNameInfo.SetActive(false);
        textPasswordInfo.SetActive(false);

        joinGameRoomButton.SetActive(false);

        TicTacToeSquareButton.SetActive(false);

        if (newState == GameStates.LoginMenu)
        {
            submitButton.SetActive(true);
            userNameInput.SetActive(true);
            passwordInput.SetActive(true);
            createToggle.SetActive(true);
            loginToggle.SetActive(true);

            textNameInfo.SetActive(true);
            textPasswordInfo.SetActive(true);
        }
        else if(newState == GameStates.MainMenu)
        {
            joinGameRoomButton.SetActive(true);
        }
        else if (newState == GameStates.WaitingInQueueForOtherPlayer)
        {
            //back button could be a thing
        }
        else if (newState == GameStates.TicTacToe)
        {
            //set tic tac toe game board UI stuffs to active
            TicTacToeSquareButton.SetActive(true);
        }
    }

    public void JoinGameRoomButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinQueueForGameRoom + "");
        ChangeState(GameStates.WaitingInQueueForOtherPlayer);
    }

    public void TicTacToeSquareButtonPressed()
    {

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.TicTacToeSomethingPlay + "");
    }
}

static public class GameStates
{
    public const int LoginMenu = 1;
    public const int MainMenu = 2;
    public const int WaitingInQueueForOtherPlayer = 3;
    public const int TicTacToe = 4;
}
