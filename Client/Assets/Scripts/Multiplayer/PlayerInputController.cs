using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    private void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        _player.SetInput(h, v);

        SendMoveStateToServer();
    }

    private void SendMoveStateToServer()
    {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity);

        var data = new Dictionary<string, object>()
        {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z}
        };

        MultiplayerManager.Instance.SendMessageToServer("move", data);
    }
}