using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform camTarget;
    public float camSpeed;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    void FixedUpdate()
    {
        if (camTarget != null)
        {
            var newPos = Vector2.Lerp(transform.position,
                camTarget.position, Time.deltaTime * camSpeed);
            var vect3 = new Vector3(newPos.x, newPos.y, -10f);

            var clampX = Mathf.Clamp(vect3.x, minX, maxX);
            var clampY = Mathf.Clamp(vect3.y, minY, maxY);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
