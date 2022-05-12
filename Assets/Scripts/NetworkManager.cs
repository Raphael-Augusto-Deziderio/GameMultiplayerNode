using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class NetworkManager : MonoBehaviour
{
    public GameObject personPrefab;
    public static NetworkManager instance;
    public SocketIOComponent socket;
    public InputField txtNickname;
    public GameObject login;
    public GameObject window;
    public Transform CanvasParent;
    public GameObject CanvasGO;
    public Dictionary<string, Player> networkPlayers = new Dictionary<string, Player>();
  
    // Start is called before the first frame update
    void Start()
    {
        login.SetActive(true);
        window.SetActive(true);
        if (instance == null)
        {
            instance = this;
            socket = GetComponent<SocketIOComponent>();
            socket.On("JOIN_SUCCESS", OnJoinSuccess);
            socket.On("SPAWN_PLAYER", OnSpawnPlayer);
            socket.On("UPDATE_POS_ROT", OnUpdatePosAndRot);
            
        } else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnJoinSuccess(SocketIOEvent pack)
    {
        Dictionary<string, string> result = pack.data.ToDictionary();
        Player dataPlayer = Instantiate(personPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, -90, 0)).GetComponent<Player>();
        dataPlayer.isLocalPlayer = true;
        // dataPlayer.transform.SetParent(CanvasParent);
        dataPlayer.gameObject.name = result["id"];
        dataPlayer.name = result["id"];
        dataPlayer.gameObject.GetComponentInChildren<TextMesh>().text = result["name"];
        networkPlayers[result["id"]] = dataPlayer;
        Debug.Log("JOIN");
    }

    public void OnSpawnPlayer(SocketIOEvent pack)
    {
        Dictionary<string, string> result = pack.data.ToDictionary();
        Vector3 pos = JsonToVector3(result["position"]);
        Quaternion rot = JsonToQuaternion(result["rotation"]);
        Debug.Log("pos: " + pos);
        Debug.Log("rot: " + rot);
        //Player dataPlayer = Instantiate(personPrefab, pos, Quaternion.Euler(0, -90, 0)).GetComponent<Player>();
        Player dataPlayer = Instantiate(personPrefab, pos, rot).GetComponent<Player>();
        dataPlayer.isLocalPlayer = false;
        // dataPlayer.transform.SetParent(CanvasParent);
        dataPlayer.gameObject.name = result["id"];
        dataPlayer.name = result["id"];
        dataPlayer.gameObject.GetComponentInChildren<TextMesh>().text = result["name"];
        networkPlayers[result["id"]] = dataPlayer;
        Debug.Log("SPAWN");
    }
     

    public void EmitJoin()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = txtNickname.text.ToString();
        socket.Emit("JOIN_ROOM", new JSONObject(data));
        login.SetActive(false);
        window.SetActive(true);
        Debug.Log("EMIT JOIN");
    }

    public void EmitPosAndRot(Vector3 newPos, Quaternion newRot)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["position"] = newPos.x + "d" + newPos.y + "d" + newPos.z;
        data["rotation"] = newRot.x + "d" + newRot.y + "d" + newRot.z + "d" + newRot.w;
        socket.Emit("MOVE_AND_ROT", new JSONObject(data));
    }

    void OnUpdatePosAndRot(SocketIOEvent pack)
    {
        Dictionary<string, string> result = pack.data.ToDictionary();
        Player netPlayer = networkPlayers[result["id"]];
        Vector3 pos = JsonToVector3(result["position"]);
        Vector4 rot = JsonToVector4(result["rotation"]);
        netPlayer.UpdatePosAndRot(pos, new Quaternion(rot.x, rot.y, rot.z, rot.w));
    }

    Vector3 JsonToVector3(string target)
    {
        Vector3 newVector;
        string[] newString = Regex.Split(target, "d");
        newVector = new Vector3(float.Parse(newString[0]), float.Parse(newString[1]), float.Parse(newString[2]));
        return newVector;
    }

    Vector4 JsonToVector4(string target)
    {
        Vector4 newVector;
        string[] newString = Regex.Split(target, "d");
        newVector = new Vector4(float.Parse(newString[0]), float.Parse(newString[1]), float.Parse(newString[2]), float.Parse(newString[3]));
        return newVector;
    }

    Quaternion JsonToQuaternion(string target)
    {
        Quaternion newVector;
        string[] newString = Regex.Split(target, "d");
        newVector = new Quaternion(float.Parse(newString[0]), float.Parse(newString[1]), float.Parse(newString[2]), float.Parse(newString[3]));
        return newVector;
    }

}
