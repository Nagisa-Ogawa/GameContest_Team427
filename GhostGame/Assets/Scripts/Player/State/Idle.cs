using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle :IState
{
    private PlayerController player;
    Rigidbody rb;

    public Idle(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        rb = player.Rb;
    }

    public void Update()
    {
       
        if (Input.GetKey(KeyCode.W))
        {
            player.Change(player.move);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.Change(player.move);

        }
        if (Input.GetKey(KeyCode.D))
        {
            player.Change(player.move);

        }
        if (Input.GetKey(KeyCode.A))
        {
            player.Change(player.move);

        }

    }

    public void Exit()
    {

    }

}
