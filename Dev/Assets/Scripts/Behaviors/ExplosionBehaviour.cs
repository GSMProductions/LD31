using UnityEngine;
using System.Collections;

public class ExplosionBehaviour : MonoBehaviour {

        void OnAnimationFinish ()
        {
                Destroy (gameObject);
        }
        
}
