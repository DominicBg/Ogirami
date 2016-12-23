using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public GameObject g;
    bool spawnDelay = true;
    public GameObject playerRef;
    float timerSpawn = 3;
    int spawnCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(spawnDelay)
        {
            spawnDelay = false;
            GameObject enemy = Instantiate(g, new Vector3(Random.Range(-10,10), transform.position.y, Random.Range(5, 10)), Quaternion.identity);
            enemy.GetComponent<EnemyManager>().playerP = playerRef;
            StartCoroutine(spawnWait());
            spawnCount++;
            if(spawnCount >= 5)
            {
                spawnCount = 0;
                timerSpawn -= .25f; 
                if(timerSpawn <= 1f)
                {
                    timerSpawn = 1f;
                }
                
            }
        }
	}

    IEnumerator spawnWait()
    {
        Debug.Log(spawnCount);
        Debug.Log(timerSpawn);
        yield return new WaitForSeconds(timerSpawn);
        spawnDelay = true;
    }
}
