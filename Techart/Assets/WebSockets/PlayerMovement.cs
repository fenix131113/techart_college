using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace WebSockets
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private WebSocketClient client;

        private void Start() => client.OnDataReceived += OnDataReceived;

        private void OnDestroy() => client.OnDataReceived -= OnDataReceived;

        private void FixedUpdate()
        {
            var move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (move.magnitude == 0)
                return;

            client.Send(new Command("move",
                new List<string>
                    { move.x.ToString(CultureInfo.InvariantCulture), move.y.ToString(CultureInfo.InvariantCulture) }));
        }

        private void OnDataReceived(Command command)
        {
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                if (command.commandString != "move")
                    return;
                
                var x = float.Parse(command.Payload[0], CultureInfo.InvariantCulture);
                var z = float.Parse(command.Payload[1], CultureInfo.InvariantCulture);

                transform.position += new Vector3(x, 0, z);
            });
        }
    }
}