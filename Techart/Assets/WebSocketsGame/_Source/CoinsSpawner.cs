using System.Globalization;
using Unity.Mathematics;
using UnityEngine;
using WebSockets;

namespace WebSocketsGame
{
    public class CoinsSpawner : MonoBehaviour
    {
        [SerializeField] private WebSocketManager client;
        [SerializeField] private Coin coinPrefab;


        private void Start() => client.OnDataReceived += OnDataReceived;

        private void OnDestroy() => client.OnDataReceived -= OnDataReceived;

        private void OnDataReceived(Command command)
        {
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                if (command.commandString != "spawn_coin")
                    return;

                var coin = Instantiate(coinPrefab,
                    new Vector3(float.Parse(command.Payload[0], CultureInfo.InvariantCulture), 0.63f,
                        float.Parse(command.Payload[1], CultureInfo.InvariantCulture)),
                    quaternion.identity);

                coin.Init(client);
            });
        }
    }
}