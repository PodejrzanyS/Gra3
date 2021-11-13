using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public Text text;
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    public float waveCountdown;
    int cos = 1;

    private float searchCountdown = 1f;

    public SpawnState state = SpawnState.COUNTING;
    void Start()
    {
        
        if (spawnPoints.Length ==0)
        {
            Debug.LogError("nie ma spawnow");
        }
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
               
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
            SetCD();

        }

    }
    void WaveCompleted()
    {
        Debug.Log("Wave Completed");
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        if (nextWave+1 > waves.Length -1)
        {
            nextWave = 0;
            Debug.Log("all waves completed now looping");
        }else
        {
            nextWave++;
        }

       

    }
    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
            if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }
    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning wave: " + _wave.name);
        state = SpawnState.SPAWNING;
        cos = cos + 1;
        for (int i = 0; i < _wave.count * cos; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);

        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);

    }
    void SetCD()
    {
        string wcd = waveCountdown.ToString("F0");
        if (waveCountdown >= 0)
        {
            text.text = "Next wave in " + wcd;
        }
        else
        {
            text.text = " ";
        }
       
    }
}
