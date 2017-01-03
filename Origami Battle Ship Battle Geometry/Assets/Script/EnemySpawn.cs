using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public Ship shipP;
    public GameObject enemyPrefab;
    bool spawnDelay = true;
    public GameObject playerRef;
	public GameObject destinationPos;
    float timerSpawn = 3;
    int spawnCount;


	Vector3 startPlayerPos;
	// Use this for initialization
	void Start () {
		startPlayerPos = playerRef.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(spawnDelay)
        {
            spawnDelay = false;
			GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(-10,10), transform.position.y, Random.Range(5, 10)), Quaternion.identity);
			enemy.transform.SetParent (transform, true);
            enemy.GetComponent<EnemyManager>().ship = shipP;
            enemy.GetComponent<EnemyManager>().playerP = playerRef;
			enemy.GetComponent<EnemyManager> ().destination = destinationPos;
            
            StartCoroutine(spawnWait());
            spawnCount++;
            if(spawnCount >= 5)
            {
                spawnCount = 0;
                timerSpawn -= .15f; 
                if(timerSpawn <= 1f)
                {
                    timerSpawn = 1f;
                }
                
            }
        }
	}

    IEnumerator spawnWait()
    {
 
        yield return new WaitForSeconds(timerSpawn);
        spawnDelay = true;
    }
}
