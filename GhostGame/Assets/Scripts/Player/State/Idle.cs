using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle :IState
{
    private PlayerController player;
    Rigidbody rb = null;

    Vector2 moveInput = Vector2.zero;
    bool isLightAttack = false;
    bool isStanAttack = false;

    public Idle(PlayerController player)
    {
        this.player = player;
        rb = player.GetComponent<Rigidbody>();
    }

    public void Enter()
    {
    }

    public void Update()
    {
        moveInput = player.PlayerInput.currentActionMap["Move"].ReadValue<Vector2>();
        if (moveInput.x != 0.0f || moveInput.y != 0.0f)
        {
            player.Change(player.move);
        }
        isLightAttack = player.PlayerInput.currentActionMap["LightAttack"].WasPressedThisFrame();
        if (isLightAttack)
        {
            player.Change(player.lightAttack);
        }
        isStanAttack = player.PlayerInput.currentActionMap["StanAttack"].WasPressedThisFrame();
        if (isStanAttack)
        {
            player.Change(player.stanAttack);
        }

    }

    public void Exit()
    {

    }

}
