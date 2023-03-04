using System;
using UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class CustomNetworkManager : MonoBehaviour
{
    public static CustomNetworkManager Singleton;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Singleton = this;
        }
    }

    public void StartHost()
    {
        string localIp = IPHelper.GetLocalIPAddress();
        
        UIManager.Singleton.PlayerUI.HostIpText.text = "Host IP: \n" + localIp;
        
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(localIp, (ushort)7777);
        NetworkManager.Singleton.StartHost();
    }
    
    public void StartClient(string ip)
    {
        UIManager.Singleton.PlayerUI.HostIpText.text = "Host IP: \n" + ip;
        
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, (ushort)7777);
        NetworkManager.Singleton.GetComponent<UnityTransport>().OnTransportEvent += OnTransportEvent;
        NetworkManager.Singleton.StartClient();
        
        UIManager.Singleton.ConnectionSpinner.Show();
    }

    private void OnTransportEvent(NetworkEvent type, ulong id, ArraySegment<byte> payload, float time)
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().OnTransportEvent -= OnTransportEvent;

        switch (type)
        {
            case NetworkEvent.Disconnect:
                UIManager.Singleton.ConnectionSpinner.Hide();
                
                UIManager.Singleton.ModalWindow.Show("Error massage", "Failed to connect to host", "Ok", () =>
                {
                    UIManager.Singleton.NetworkUI.ClearIpAddressInputField();
                    UIManager.Singleton.NetworkUI.interactable = true;
                });
                
                break;
            case NetworkEvent.Connect:
                UIManager.Singleton.ConnectionSpinner.Hide();
                break;
        }
    }
}
