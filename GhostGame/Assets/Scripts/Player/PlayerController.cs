using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace personal
{

    public class PlayerController : MonoBehaviour
    {

        public IState currentState { get; private set; }


        public Idle idle;
        public Move move;
        public LightAttack lightAttack;
        public StanAttack stanAttack;

        [SerializeField]
        private Rigidbody rb = null;
        public Rigidbody Rb
        {
                    get { return rb; }
            private set { rb = value; }

        }

        [SerializeField]
        private float speed = 3.0f; 
        public float Speed
        {
                    get { return speed; }
            private set {speed = value; }
        }

        [SerializeField]
        private GameObject hitEffectObj = null;
        public GameObject HitEffectObj
        {
            get { return hitEffectObj; }
            private set { hitEffectObj = value; }
        }

        [SerializeField]
        private GameObject stanHitEffectObj = null;
        public GameObject StanHitEffectObj
        {
            get { return stanHitEffectObj; }
            private set { stanHitEffectObj = value; }
        }


        [SerializeField]
        private GameObject playerArmObj = null;
        public GameObject PlayerArmObj
        {
            get { return playerArmObj; }
            private set { playerArmObj = value; }
        }


        // Start is called before the first frame update
        void Start()
        {
            idle = new Idle(this);
            move = new Move(this);
            lightAttack = new LightAttack(this);
            stanAttack= new StanAttack(this);

            Change(idle);
        }

        public void Change(IState nextState)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }
            if (nextState != null)
            {
                currentState = nextState;
                nextState.Enter();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(currentState != null)
            {
                currentState.Update();
            }
        }
    }
}
