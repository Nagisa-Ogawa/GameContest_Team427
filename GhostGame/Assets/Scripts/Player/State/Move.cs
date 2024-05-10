using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : IState
{
    private PlayerController player;
    public Vector3 velocity;

    public Move(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        bool ISmove = false;
        velocity = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            velocity += player.transform.forward * player.Speed;
            ISmove = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            velocity += -player.transform.forward * player.Speed;
            ISmove = true;

        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += player.transform.right * player.Speed;
            ISmove = true;

        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity += -player.transform.right * player.Speed;
            ISmove = true;

        }

        player.Rb.velocity = velocity;
        if(ISmove == false)
        {
            player.Change(player.idle);
        }
    }

    public void Exit()
    {
        velocity = new Vector3(0, 0, 0);
       player.Rb.velocity = new Vector3(0, 0, 0);
    }

}
