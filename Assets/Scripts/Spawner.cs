
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private MovingCube cubePrefab;
    public MoveDirection moveDirection; 

    GameObject start;
    private void Start()
    {
        start = GameObject.Find("Base");
    }
    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);
        if(MovingCube.lastCube !=null && MovingCube.lastCube.gameObject != start)
        {
            float x = MoveDirection.x == moveDirection ? transform.position.x : MovingCube.lastCube.transform.position.x;
            float z = MoveDirection.z == moveDirection ? transform.position.z : MovingCube.lastCube.transform.position.z;

            cube.transform.position = new Vector3(x, 
                MovingCube.lastCube.transform.position.y + cubePrefab.transform.localScale.y,
                z);
           
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.moveDirection = moveDirection;
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, cubePrefab.transform.localScale);
    }
}
