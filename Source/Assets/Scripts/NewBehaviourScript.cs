using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    public List<GameObject> objectsToSpawn;
    public Collider spawnArea;
    public float minDistanceBetweenObjects = 2.0f;
    public int numberOfObjectsToSpawn = 5;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        if (objectsToSpawn.Count >= numberOfObjectsToSpawn)
        {
            SpawnObjects();
        }
        else
        {
            Debug.LogWarning("Not enough unique objects to spawn.");
        }
    }

    void SpawnObjects()
    {
        while (spawnedObjects.Count < numberOfObjectsToSpawn)
        {
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Count)];

            if (!spawnedObjects.Contains(objectToSpawn))
            {
                spawnedObjects.Add(objectToSpawn);

                Vector3 objectSize = objectToSpawn.GetComponent<Collider>().bounds.size;
                Vector3 spawnPosition = GetRandomNonOverlappingPosition(objectSize);
                Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            }
        }
    }

    Vector3 GetRandomNonOverlappingPosition(Vector3 objectSize)
    {
        Vector3 randomPoint;

        do
        {
            float spawnX = Mathf.Clamp(
                Random.Range(
                    spawnArea.bounds.min.x + objectSize.x / 2,
                    spawnArea.bounds.max.x - objectSize.x / 2
                ),
                spawnArea.bounds.min.x + objectSize.x / 2,
                spawnArea.bounds.max.x - objectSize.x / 2
            );

            float spawnZ = Mathf.Clamp(
                Random.Range(
                    spawnArea.bounds.min.z + objectSize.z / 2,
                    spawnArea.bounds.max.z - objectSize.z / 2
                ),
                spawnArea.bounds.min.z + objectSize.z / 2,
                spawnArea.bounds.max.z - objectSize.z / 2
            );

            randomPoint = new Vector3(spawnX, transform.position.y, spawnZ);
        } while (IsOverlapping(randomPoint, objectSize));

        return randomPoint;
    }

    bool IsOverlapping(Vector3 point, Vector3 objectSize)
    {
        Collider[] colliders = Physics.OverlapBox(point, objectSize / 2 + Vector3.one * minDistanceBetweenObjects);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("SpawnedObject"))
            {
                return true;
            }
        }

        return false;
    }
}
