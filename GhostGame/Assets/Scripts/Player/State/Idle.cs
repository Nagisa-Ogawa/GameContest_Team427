using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace personal
{
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

            if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
            {
                player.Change(player.move);
            }
            if (Input.GetKeyDown("joystick button 2"))
            {
                player.Change(player.lightAttack);
            }
            if(Input.GetKeyDown("joystick button 4"))
            {
                player.Change(player.stanAttack);
            }

        }

        public void Exit()
        {

        }

    }
}
