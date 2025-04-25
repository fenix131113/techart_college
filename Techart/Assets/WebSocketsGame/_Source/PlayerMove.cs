using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using WebSockets;

namespace WebSocketsGame
{
    public class PlayerMove : MonoBehaviour
    {
        public int PlayerId { get; private set; }
        public bool IsMine { get; private set; }

        private WebSocketManager _client;

        public void Init(WebSocketManager client, int id, bool isMine)
        {
            _client = client;
            PlayerId = id;
            IsMine = isMine;
        }

        private void Start() => _client.OnDataReceived += OnDataReceived;

        private void OnDestroy() => _client.OnDataReceived -= OnDataReceived;

        private void FixedUpdate()
        {
            if (!IsMine)
                return;

            var move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (move.magnitude == 0)
                return;

            _client.Send(new Command("move",
                new List<string>
                    { move.x.ToString(CultureInfo.InvariantCulture), move.y.ToString(CultureInfo.InvariantCulture) }));
        }

        private void OnDataReceived(Command command)
        {
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                if (command.commandString != "move")
                    return;

                var loadedID = int.Parse(command.Payload[0]);

                if (loadedID != PlayerId)
                    return;

                var x = float.Parse(command.Payload[1], CultureInfo.InvariantCulture);
                var z = float.Parse(command.Payload[2], CultureInfo.InvariantCulture);

                transform.position += new Vector3(x, 0, z);
            });
        }
    }
}