using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTogether.Net;

public class LobbyManager : MonoBehaviour
{
    struct PlayerListEntry
    {
        string name;
        int score;
    }

    public string code = string.Empty; //The code or IP to connect with
    public string username;

    public GameObject codePanel;
    public GameObject ipPanel;
    public GameObject lobbyPanel;
    public GameObject mainPanel;
    public GameObject errorText;

    public Text roomCodeText;
    public Text playerListText;

    public InputField userNameInput;

    public Toggle motionToggle;

    private string ip = string.Empty;
    private ServerRoomCode thisCode;

    private List<PlayerListEntry> players;

    void Start()
    {
        players = new List<PlayerListEntry>();

        SetUsername(PlayerPrefs.GetString("Username", "Player"));

        userNameInput.text = username;

        motionToggle.isOn = (PlayerPrefs.GetInt("Gyro", 0) == 1) ? true : false;

        thisCode = GetComponent<ServerRoomCode>();

        MacrogameClient.Instance.OnNameRejected.AddListener(OnNameRejected);
        MacrogameClient.Instance.OnWeJoinedServer.AddListener(OnWeJoinedServer);
        MacrogameClient.Instance.OnOtherPlayerJoined.AddListener(OnOtherPlayerJoinedServer);
        MacrogameClient.Instance.OnPlayerNameReceived.AddListener(OnNameReceived);
        MacrogameClient.Instance.OnPlayerLeftServer.AddListener(OnPlayerLeft);

        if(MacrogameClient.Instance.IsConnected())
        {
            ClearPlayerList();
            MacrogameClient.Instance.RequestScoreUpdate();
            MacrogameClient.Instance.RequestNameList();
            SwitchToLobby();
        }
    }

    private void Update()
    {
        roomCodeText.text = "VR Player Score: " + MacrogameClient.Instance.GetVRScore();
    }

    private void OnDestroy()
    {
        MacrogameClient.Instance.OnNameRejected.RemoveListener(OnNameRejected);
        MacrogameClient.Instance.OnWeJoinedServer.RemoveListener(OnWeJoinedServer);
        MacrogameClient.Instance.OnOtherPlayerJoined.RemoveListener(OnOtherPlayerJoinedServer);
        MacrogameClient.Instance.OnPlayerNameReceived.RemoveListener(OnNameReceived);
        MacrogameClient.Instance.OnPlayerLeftServer.RemoveListener(OnPlayerLeft);
    }

    private void OnPlayerLeft(string name)
    {
        RemovePlayerFromPlayerList(name);
    }

    private void OnNameRejected()
    {
        EnableError("Name taken, please pick a new name in Options.");
        ClearPlayerList();
    }

    private void OnWeJoinedServer(string name)
    {
        ClearPlayerList();
        MacrogameClient.Instance.RequestScoreUpdate();
        MacrogameClient.Instance.RequestNameList();
        SwitchToLobby();
        //AddPlayerNameToPlayerList(name);
    }

    private void OnOtherPlayerJoinedServer(string name)
    {
        AddPlayerNameToPlayerList(name);
    }

    private void OnNameReceived(string name)
    {
        AddPlayerNameToPlayerList(name);
    }

    public void JoinLobby()
    {
        if (code != string.Empty)
        {
            Debug.Log("Recieved code: " + code);

            //roomCodeText.text = "Room Code: " + code;
            string failReason = "";

            string testip = "111.111.111.111";
            Debug.Log(testip.Split('.').Length);

            //Actual code to connect here...
            if (code.Length != 4 && !thisCode.AssertIP(code, out failReason)) //If it is not an IP or a Code
            {
                //Fail the request immediately
                EnableError("Invalid code or IP.");

                Debug.Log("Failed in JoinLobby() Length=" + code.Length);

                //If it failed because of the Assert, print why
                if (failReason != "")
                {
                    Debug.Log("IP Parse Error =\'" + failReason + "\' for input \'" + code + "\'");

                }
                else
                {
                    Debug.Log("No fail reason given.");

                }

                return;

            }

            if (code.Length == 4)
            {
                //Get the IP corresponding to the room code here
                transform.GetComponent<CodeToIP>().Submit(code);

            }
            else
            {
                ip = code;
                errorText.SetActive(false);

                 if (!MacrogameClient.Instance.AttemptConnection(ip))
                {
                    EnableError("A connection could not be established.");
                    Debug.Log("Failed to connect to IP: " + ip);


                }

            }

            //Debug.Log("Wow shit bork, we got the ip:" + ip);



        }

    }

    public void SetCode(string newCode)
    {
        code = newCode.ToUpper();

    }

    public void SetUsername(string newUsername)
    {
        PlayerPrefs.SetString("Username", newUsername);

        username = newUsername;

        MacrogameClient.Instance.playerName = username;

    }

    public bool IsValidIP(string ip)
    {
        string[] splitStrings = ip.Split('.');

        // Check if the IP can be split into four parts based of of '.'
        if (splitStrings.Length != 4)
        {
            return false;

        }

        // Check that each byte is within [0, 255]
        foreach (string byteString in splitStrings)
        {
            if (int.Parse(byteString) > 255 || int.Parse(byteString) < 0)
            {
                return false;

            }

        }

        // It is a string formatted as a valid IP, will not know if it will connect based off of this though
        return true;


    }

    public void SetIP()
    {
        ip = transform.GetComponent<CodeToIP>().GetIP();

        Debug.Log("Got IP: " + ip);

        code = ip;

        JoinLobby();

    }

    public void EnableError(string error)
    {
        Debug.Log(error);
        errorText.GetComponent<Text>().text = error;
        errorText.SetActive(true);

    }

    public void SwitchToLobby()
    {
        lobbyPanel.SetActive(true);
        codePanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void AddPlayerNameToPlayerList(string name)
    {
        string nameList = playerListText.text;

        if (name == MacrogameClient.Instance.playerName)
        {
            nameList += "\n<color=#ff00ffff>" + name + "</color> - " + MacrogameClient.Instance.GetPlayerScore(name);

        }
        else
        {
            nameList += "\n" + name + " - " + MacrogameClient.Instance.GetPlayerScore(name);

        }

        playerListText.text = nameList;
    }

    public void LeaveServer()
    {
        MacrogameClient.Instance.Disconnect();
    }

    public void ClearPlayerList()
    {
        playerListText.text = "Players Connected:";

    }

    public void RemovePlayerFromPlayerList(string name)
    {
        ClearPlayerList();

        foreach(string n in MacrogameClient.Instance.GetPlayerNames())
        {
            AddPlayerNameToPlayerList(n);
        }
    }

    public void UpdateMotionControlsPref (bool state)
    {
        //Note: state does not actually get passed, so we need to grab it

        state = motionToggle.isOn;

        PlayerPrefs.SetInt("Gyro", (state ? 1 : 0));

    }
}