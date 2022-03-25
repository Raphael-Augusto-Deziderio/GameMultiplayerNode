using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviour
{
    public GameObject personPrefab;
    public static NetworkManager instance;
    public SocketIOComponent socket;
    public Text txt;
    public Dictionary<string, Player> networkPlayers = new Dictionary<string, Player>();
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            socket = GetComponent<SocketIOComponent>();
            socket.On("JOIN_SUCCESS", OnJoinSuccess);
            socket.On("SPAWN_PLAYER", OnSpawnPlayer);
        
        } else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("r"))
        {
            EmitJoin();
        }
    }

    public void OnSpawnPlayer(SocketIOEvent pack)
    {
        Dictionary<string, string> result = pack.data.ToDictionary();
        Player dataPlayer = Instantiate(personPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Player>();
        dataPlayer.gameObject.name = result["id"];
        dataPlayer.name = result["id"];
        dataPlayer.gameObject.GetComponentInChildren<TextMesh>().text = result["name"];
        networkPlayers[result["id"]] = dataPlayer;
    }

    public void EmitJoin()
    {
        Debug.Log("EmitJoin");
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = Random.Range(1, 10).ToString();
        socket.Emit("JOIN_ROOM", new JSONObject(data));
        socket.Emit("TESTE", new JSONObject(data));
    }
    public void OnJoinSuccess(SocketIOEvent pack)
    {
        Debug.Log("OnJoinSuccess");
        OnSpawnPlayer(pack);
    }
}
