using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour// спавнер объектов. За дополнительную стоимость могу сделать реализацию через пул объектов. Прием для оптимизации
{
    public static int countSpawnedObjects = 0;

    public bool enabled;
    public int maxCountSpawnedObjects = 5;
    public GameObject spawnedObject;
    public float timeSpawn = 2f;
    public Vector2 largestPointsSpawn;

    private float timer;

    private void Start()
    {
        timer = timeSpawn;
    }

    private void Update() 
    {
        timer -= Time.deltaTime; //реализация таймера
        if (timer <= 0 && countSpawnedObjects < maxCountSpawnedObjects)
        {
            SpawnObject(); // как нужное время прошло - спавним дерево
        }
    }
    private void SpawnObject()
    {
        timer = timeSpawn;
        countSpawnedObjects++;

        Instantiate(spawnedObject, transform.position + new Vector3(Random.Range(-largestPointsSpawn.x, largestPointsSpawn.x), 
            Random.Range(-largestPointsSpawn.y, largestPointsSpawn.y), 0), Quaternion.identity);
        //спавним в рандомном месте
    }
}
