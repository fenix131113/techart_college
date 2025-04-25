using UnityEngine;

namespace WebSockets
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private WebSocketClient client;

        private void Start() => client.OnDataReceived += OnDataReceived;

        private void OnDestroy() => client.OnDataReceived -= OnDataReceived;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                Attack(true);
            else if (Input.GetMouseButtonDown(1))
                Attack(false);
        }

        private void Attack(bool main) =>
            client.Send(new Command(main ? "attack_main" : "attack_second", null));

        private void OnDataReceived(Command command)
        {
            UnityMainThreadDispatcher.Enqueue(() =>
            {
                bool canAttack;

                switch (command.commandString)
                {
                    case "attack_main":
                        canAttack = bool.Parse(command.Payload[0]);

                        if (!canAttack)
                        {
                            Debug.Log("Can't attack RMB while cooldown");
                            return;
                        }

                        Debug.Log("RMB Attack");
                        break;

                    case "attack_second":
                        canAttack = bool.Parse(command.Payload[0]);

                        if (!canAttack)
                        {
                            Debug.Log("Can't attack LMB while cooldown");
                            return;
                        }

                        Debug.Log("LMB Attack");
                        break;
                }
            });
        }
    }
}