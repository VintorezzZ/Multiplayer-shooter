using System.Collections.Generic;
using Armory.Weapons;
using Multiplayer;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private float _mouseSens = 1f;
    [SerializeField] private Weapon _weapon;


    private void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        var jumpInput = Input.GetKeyDown(KeyCode.Space);

        var isShoot = Input.GetMouseButton(0);

        _player.SetInput(h, v, mouseX * _mouseSens);
        _player.RotateX(-mouseY * _mouseSens);

        if (jumpInput)
            _player.Jump();

        if (isShoot)
            _weapon.Shoot();

        SendMoveStateToServer();
    }

    private void SendMoveStateToServer()
    {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);

        var data = new Dictionary<string, object>()
        {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},
            {"rX", rotateX},
            {"rY", rotateY}
        };

        MultiplayerManager.Instance.SendMessageToServer("move", data);
    }
}