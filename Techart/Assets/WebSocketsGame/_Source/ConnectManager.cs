using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using WebSockets;

namespace WebSocketsGame
{
    public class ConnectManager : MonoBehaviour
    {
        public int PlayerID { get; private set; } = -1;

        [SerializeField] private WebSocketManager client;
        [SerializeField] private PlayerMove playerPrefab;
        [SerializeField] private Transform spawnPoint;

        private readonly Dictionary<int, PlayerMove> _players = new();
        
        private void Start()
        {
            client.OnDataReceived += OnDataReceived;
            ConnectToGame();
        }

        private void OnDestroy() => client.OnDataReceived -= OnDataReceived;

        private void ConnectToGame()
        {
            client.Send(new Command("register_player", null));
        }

        private void SpawnPlayers(int[] playerIDs, int currentPlayerID)
        {
            foreach (var id in playerIDs)
            {
                if(_players.ContainsKey(id))
                    continue;
                
                var player = Instantiate(playerPrefab, spawnPoint);
                player.Init(client, id, id == currentPlayerID);
                _players.Add(id, player);
            }
        }
        
        private void OnDataReceived(Command command)
        {
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                if (command.commandString != "register_result")
                    return;
                
                PlayerID = int.Parse(command.Payload[0], CultureInfo.InvariantCulture);
                var result = JsonUtility.FromJson<RegistrationResult>(command.Payload[1]);
                
                SpawnPlayers(result.ids, PlayerID);
                
                Debug.Log("Registered player with index: " + PlayerID);
            });
        }

        [Serializable]
        public class RegistrationResult
        {
            public int[] ids;
        }
    }
}