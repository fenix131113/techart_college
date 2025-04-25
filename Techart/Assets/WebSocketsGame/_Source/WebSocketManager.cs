using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace WebSocketsGame
{
    public class WebSocketManager : MonoBehaviour
    {
        private WebSocket _ws;

        public event Action<Command> OnDataReceived;

        private void Awake()
        {
            _ws = new WebSocket("ws://localhost:3000");
            _ws.OnMessage += OnMessageReceived;
            _ws.Connect();
        }

        public void Send(Command command)
        {
            var msg = JsonUtility.ToJson(command);
            _ws.Send(msg);
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            var response = JsonUtility.FromJson<Command>(e.Data);

            OnDataReceived?.Invoke(response);
        }
    }

    [Serializable]
    public class Command
    {
        public Command(string commandString, List<string> payload)
        {
            this.commandString = commandString;
            Payload = payload;
        }

        public string commandString;
        public List<string> Payload;
    }
}