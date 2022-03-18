using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManager : MonoBehaviour
{

    public static NetworkManager instance;
    public SocketIOComponent socket;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;

            socket = GetComponent<SocketIOComponent>();
            socket.On("PONG", OnReceivePong);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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


}
