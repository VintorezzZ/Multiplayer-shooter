using System.Collections.Generic;
using System.Threading.Tasks;
using Colyseus;
using UnityEngine;

namespace Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private EnemyController _enemy;

        private ColyseusRoom<State> _room;
        protected override void Awake()
        {
            base.Awake();

            Instance.InitializeClient();
            Connect();
        }

        private async Task Connect()
        {
            _room = await Instance.client.JoinOrCreate<State>("state_handler");

            _room.OnStateChange += OnStateChange;
        }

        private void OnStateChange(State state, bool isfirststate)
        {
            if (!isfirststate)
                return;

            state.players.ForEach((key, player) =>
            {
                if (key == _room.SessionId)
                    CreatePlayer(player);
                else
                    CreateEnemy(key, player);
            });

            _room.State.players.OnAdd += CreateEnemy;
            _room.State.players.OnRemove += RemoveEnemy;
        }

        private void CreatePlayer(Player player)
        {
            var pos = new Vector3(player.pX , player.pY, player.pZ);
            Instantiate(_player, pos, Quaternion.identity);
        }

        private void CreateEnemy(string key, Player player)
        {
            var pos = new Vector3(player.pX , player.pY, player.pZ);
            var enemy = Instantiate(_enemy, pos, Quaternion.identity);
            player.OnChange += enemy.OnChange;
        }

        private void RemoveEnemy(string key, Player player)
        {

        }

        public void SendMessageToServer(string key, Dictionary<string, object> data)
        {
            _room.Send(key, data);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _room?.Leave();
        }
    }
}