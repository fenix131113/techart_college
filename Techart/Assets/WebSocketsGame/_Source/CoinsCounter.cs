using System.Globalization;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using WebSockets;

namespace WebSocketsGame
{
    public class CoinsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private WebSocketManager client;


        private void Start() => client.OnDataReceived += OnDataReceived;

        private void OnDestroy() => client.OnDataReceived -= OnDataReceived;

        private void OnDataReceived(Command command)
        {
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                if (command.commandString != "display_coins")
                    return;
                
                label.text = command.Payload[0];
            });
        }
    }
}