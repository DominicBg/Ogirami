using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public enum EnemyAction { Passive, Active, Attack };
    public EnemyAction enemAction;
    public GameObject playerP;
    Vector3 t;
    // Use this for initialization
    void Start() {
        enemAction = EnemyAction.Active;

    }

    // Update is called once per frame
    void Update() {
        modeEnemy(enemAction);
        
    }

    void modeEnemy(EnemyAction enem)
    {
        if(enem == EnemyAction.Active)
        {
            transform.position = Vector3.Lerp(transform.localPosition, playerP.transform.position, Time.deltaTime / 2);
        }
       else if (enem == EnemyAction.Attack)
        {
            
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            

            enemAction = EnemyAction.Attack;

        }
    }

    
}
