using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace WebSockets
{
    public class WebSocketClient : MonoBehaviour
    {
        private WebSocket _ws;

        public event Action<Command> OnDataReceived;

        private void Start()
        {
            _ws = new WebSocket("ws://localhost:3000");
            _ws.OnMessage += OnMessageReceived;
            _ws.Connect();
            Send(new Command("move",
                new List<string>
                    { "1", "0" }));
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