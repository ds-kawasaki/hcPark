using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField][Tooltip("x軸の回転角度")] private float rotateX = 0.0f;
    [SerializeField][Tooltip("y軸の回転角度")] private float rotateY = 0.0f;
    [SerializeField][Tooltip("z軸の回転角度")] private float rotateZ = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(this.rotateX, this.rotateY, this.rotateZ) * Time.deltaTime);
    }
}
