using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChatSceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField ChatInput;
    public Text ChatText;
    GameObject networkClient;
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.name == "NetworkedClient")
            {
                networkClient = go;

                networkClient.GetComponent<NetworkedClient>().m_ChatText = ChatText;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendButton();
        }
    }

    public void SendButton()
    {
        string msg;

        msg = ClientToServerSignifiers.ChatMsg + ", " + ChatInput.text;

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);

        ChatText.text += ChatInput.text + "\n";

        ChatInput.text = "";
    }

    public void BackButton()
    {
        string msg;

        msg = ClientToServerSignifiers.ChatBack + "";

        networkClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);

        SceneManager.LoadScene("Menu");
    }
}
