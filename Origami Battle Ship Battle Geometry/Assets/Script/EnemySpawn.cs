using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    Ship shipP;
	public EnemyTracker enemyTracker;
    public GameObject enemyPrefab;
	public GameObject radarPrefab;

    bool spawnDelay = true;
	public GameObject destinationPos;
    public float timerSpawn = 3;
    int spawnCount;
	[SerializeField]bool isNormalSpawner;

	Vector3 startPlayerPos;
	// Use this for initialization
	void Start () {
		shipP = Camera.main.GetComponent<Game_Manager> ().ship;
		startPlayerPos = shipP.transform.position;
	}

	// Update is called once per frame
	void Update () {

	
		if(spawnDelay && !Game_Manager.inMenu && World_Manager.canChange)
        {
            spawnDelay = false;

			//Enemy Spawn
			int spawnSide = (Random.Range(0,2) == 0) ? Random.Range(-5,-15) : Random.Range(5,15);
			GameObject enemy = Instantiate(enemyPrefab, new Vector3(spawnSide, transform.position.y, Random.Range(5, 10)), Quaternion.identity);
			EnemyScript enemyScript = enemy.GetComponent<EnemyScript> ();
			enemy.transform.SetParent (transform, true);

			enemyScript.ship = shipP;
			enemyScript.destination = destinationPos;
            //Radar spawn
			Transform parent =  ((isNormalSpawner) ? enemyTracker.normalUI : enemyTracker.fractalUI);
			GameObject radar = Instantiate(radarPrefab, parent.position, Quaternion.identity) as GameObject;
			radar.transform.SetParent (parent, true);
			enemyScript.radarTriangle = radar;
			enemyScript.isNormalEnemy = isNormalSpawner;

            StartCoroutine(spawnWait());
            spawnCount++;
            if(spawnCount >= 5)
            {
                spawnCount = 0;
                timerSpawn -= .1f; 
                if(timerSpawn <= 0.7f)
                {
                    timerSpawn = 0.7f;
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
