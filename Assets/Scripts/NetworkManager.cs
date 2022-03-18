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
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            socket = GetComponent<SocketIOComponent>();
            //socket.On("PONG", OnReceivePong);
            socket.On("SPAWN", OnReceiveSpawn);
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
            SendSpawnPerson();
         
        }
    }

    //Enviar ping ao servidor
    public void SendPingToServer()
    {
        Dictionary<string, string> pack = new Dictionary<string, string>();
        pack["message"] = "ping!!";
        Debug.Log("Mensagem enviada ao servidor: " + pack["message"]);
        socket.Emit("PING", new JSONObject(pack));
    }

    public void OnReceivePong(SocketIOEvent pack)
    {
        Debug.Log("CHEGUEI NO UNITY DE VOLTA");

        Dictionary<string, string> result = pack.data.ToDictionary();
        Debug.Log("Mensagem do servidor: " + result["message"]);
    }

    public void SendSpawnPerson()
    {
        GameObject spawn = Instantiate(personPrefab, transform.position, transform.rotation) as GameObject;
        spawn.SetActive(true);
        Dictionary<string, string> pack = new Dictionary<string, string>();
        pack["message"] = transform.position.ToString();
        socket.Emit("SPAWN", new JSONObject(pack));
    }

    public void OnReceiveSpawn(SocketIOEvent pack)
    {
        Debug.Log("CHEGUEI");
        Dictionary<string, string> result = pack.data.ToDictionary();
        string result2 = "RECEBI DO SERVER: "+result;
        txt.text = result2;
    }

}
