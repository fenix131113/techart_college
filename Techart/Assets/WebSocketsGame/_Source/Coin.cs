using UnityEngine;

namespace WebSocketsGame
{
    public class Coin : MonoBehaviour
    {
        private WebSocketManager _client;

        public void Init(WebSocketManager client) => _client = client;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerMove player))
                return;

            if (player.IsMine)
                _client.Send(new Command("collect_coin", null));
            
            Destroy(gameObject);
        }
    }
}