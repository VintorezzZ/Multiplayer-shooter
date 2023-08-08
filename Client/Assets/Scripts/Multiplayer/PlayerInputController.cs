using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private float _mouseSens = 1f;

    private void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        var jumpInput = Input.GetKeyDown(KeyCode.Space);

        _player.SetInput(h, v, mouseX * _mouseSens);
        _player.RotateX(-mouseY * _mouseSens);

        if (jumpInput)
            _player.Jump();

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