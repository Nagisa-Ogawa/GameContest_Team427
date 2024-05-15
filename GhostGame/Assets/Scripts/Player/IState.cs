using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace personal
{
    public interface IState 
    {
        void Enter();



        void Update();


        void Exit();
   
    }

}
