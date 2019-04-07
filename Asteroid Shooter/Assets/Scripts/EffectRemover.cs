using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRemover : MonoBehaviour {

	void Start ()
    {
        Invoke("DestroyCollisionEffect", 1.5f);	
	}

    private void DestroyCollisionEffect()
    {
        Destroy(gameObject);
    }

}
