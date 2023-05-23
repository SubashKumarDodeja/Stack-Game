using System;

using UnityEngine;

public class MovingCube : MonoBehaviour
{
    public static MovingCube currentCube { get; private set; }

    public static MovingCube lastCube { get; private set; }
    public MoveDirection moveDirection { get; internal set; }

    [SerializeField]
    public float movingSpeed=1f;

    private Renderer _renderer;

   
    private void OnEnable()
    {
        if (lastCube == null)
            lastCube = GameObject.Find("Base").GetComponent<MovingCube>();

        currentCube = this;

        
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = GetRandomColor();
        transform.localScale = new Vector3( lastCube.transform.localScale.x, transform.localScale.y, lastCube.transform.localScale.z);

      
    }
    public void Stop()
    {

        movingSpeed = 0;
        float cutPart = GetCutPart();
        float max = MoveDirection.x == moveDirection ? lastCube.transform.localScale.x : lastCube.transform.localScale.z;
        if (Mathf.Abs(cutPart) > max)
        {
            lastCube = null;
            currentCube = null;
            FindObjectOfType<GameManager>().LevelFail();
        }
        else
        {

            float direction = cutPart > 0f ? 1f : -1f;
            if (moveDirection == MoveDirection.z)
            {
                SplitCubeZaxis(cutPart, direction);
                lastCube = this;
            }
            else
            {
                SplitCubeXaxis(cutPart, direction);
                lastCube = this;
            }
        }

    }

    private float GetCutPart()
    {
        if(MoveDirection.z == moveDirection)
            return currentCube.transform.position.z - lastCube.transform.position.z;
        else
            return currentCube.transform.position.x - lastCube.transform.position.x;
    }

    private void SplitCubeXaxis(float cutpart, float direction)
    {
        float newXsize = lastCube.transform.localScale.x - Math.Abs(cutpart); 
        float fallingCubeSize = transform.localScale.x - newXsize;
        float newZposition = lastCube.transform.position.x + (cutpart / 2);
        transform.localScale = new Vector3(newXsize, transform.localScale.y, transform.localScale.z);
        transform.localPosition = new Vector3(newZposition, transform.localPosition.y, transform.localPosition.z);

        float cubeEdge = transform.position.x + (newXsize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + (fallingCubeSize / 2f * direction);
        SpawnDropCube(fallingBlockZPosition, fallingCubeSize);
    }
    private void SplitCubeZaxis(float cutpart,float direction)
    {


        float newZsize = lastCube.transform.localScale.z - Math.Abs(cutpart);
        float fallingCubeSize = transform.localScale.z - newZsize;
        float newZposition = lastCube.transform.position.z + (cutpart/2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZsize);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newZposition);

        float cubeEdge = transform.position.z + (newZsize/2f * direction);
        float fallingBlockZPosition = cubeEdge + (fallingCubeSize/2f * direction);
        SpawnDropCube(fallingBlockZPosition, fallingCubeSize);
    }

    private void SpawnDropCube(float fallingBlockZPosition,float fallingCubeSize)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        
        cube.GetComponent<Renderer>().material = _renderer.material;
        if (moveDirection == MoveDirection.z)
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingCubeSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);

        }
        else
        {

            cube.transform.localScale = new Vector3(fallingCubeSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockZPosition, transform.position.y, transform.position.x);
        }

        cube.AddComponent<Rigidbody>();

        cube.GetComponent<Rigidbody>().useGravity = true;
        cube.GetComponent<Rigidbody>().isKinematic=false;
        Destroy(cube, 1f);

    }

    // Start is called before the first frame update
    

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(moveDirection== MoveDirection.z)
            this.transform.position += this.transform.forward * Time.deltaTime * movingSpeed;
       else
            this.transform.position += this.transform.right * Time.deltaTime * movingSpeed;
    }
}
