using System.Collections.Generic;
using Armory.Weapons;
using Multiplayer;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private float _mouseSens = 1f;
    [SerializeField] private ClientWeapon _weapon;

    private MultiplayerManager _multiplayerManager;

    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
    }

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

        if (isShoot && _weapon.TryShoot(out var shootInfo))
            SendShootInfoToServer(ref shootInfo);

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

        _multiplayerManager.SendMessageToServer("move", data);
    }

    private void SendShootInfoToServer(ref ShootInfo shootInfo)
    {
        shootInfo.Key = _multiplayerManager.GetSessionId();
        var json = JsonUtility.ToJson(shootInfo);
        _multiplayerManager.SendMessageToServer("shoot", json);
    }
}

[System.Serializable]
public struct ShootInfo
{
    public string Key;

    public float DirX;
    public float DirY;
    public float DirZ;

    public float PosX;
    public float PosY;
    public float PosZ;
}