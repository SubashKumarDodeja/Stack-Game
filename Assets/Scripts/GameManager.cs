
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
     Spawner [] spawners = new Spawner[2];
    Spawner randomSpawner;
    private void Awake()
    {
         spawners = FindObjectsOfType<Spawner>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(MovingCube.currentCube!=null)
                MovingCube.currentCube.Stop();


            randomSpawner = spawners[Random.Range(0, 2)];
            randomSpawner.SpawnCube();
        }
        
    }

    internal void LevelFail()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
