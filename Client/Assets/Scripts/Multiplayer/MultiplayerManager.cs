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
        private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

        protected override void Awake()
        {
            base.Awake();

            Instance.InitializeClient();
            Connect();
        }

        private async Task Connect()
        {
            var data = new Dictionary<string, object>()
            {
                { "speed", _player.Speed },
                { "hp", _player.MaxHealth }
            };

            _room = await Instance.client.JoinOrCreate<State>("state_handler", data);

            _room.OnStateChange += OnStateChange;
            _room.OnMessage<string>("Shoot", ApplyShootFromServer);
        }

        private void ApplyShootFromServer(string shootInfoJson)
        {
            var shootInfo = JsonUtility.FromJson<ShootInfo>(shootInfoJson);

            if (!_enemies.ContainsKey(shootInfo.Key))
            {
                Debug.LogError("Enemy doesn't exist!");
                return;
            }

            _enemies[shootInfo.Key].Shoot(shootInfo);
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
            var playerModel = Instantiate(_player, pos, Quaternion.identity);
            player.OnChange += playerModel.OnChange;
        }

        private void CreateEnemy(string key, Player player)
        {
            var pos = new Vector3(player.pX , player.pY, player.pZ);
            var enemy = Instantiate(_enemy, pos, Quaternion.identity);
            enemy.Init(player, key);

            _enemies.Add(key, enemy);
        }

        private void RemoveEnemy(string key, Player player)
        {
            if (!_enemies.ContainsKey(key))
                return;

            var enemy = _enemies[key];
            enemy.Dispose();
            _enemies.Remove(key);
        }

        public void SendMessageToServer(string key, Dictionary<string, object> data)
        {
            _room.Send(key, data);
        }

        public void SendMessageToServer(string key, string data)
        {
            _room.Send(key, data);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _room?.Leave();
        }

        public string GetSessionId()
        {
            return _room.SessionId;
        }
    }
}