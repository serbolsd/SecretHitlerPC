using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectReconnect : MonoBehaviour
{
  Client cliente;
  // Start is called before the first frame update
  void Start()
  {
    cliente = FindObjectOfType<Client>();
  }

  public void RediConnect()
  {
    if (null != cliente)
    {
      if (cliente.connected)
      {
        cliente.disconectedTest();
      }
      else
      {
        cliente.reconnect();
      }

    }

  }

}
