using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float Speed = 1.0f;

    private Camera m_camera = null;
    private Transform m_transform = null;

    // Use this for initialization
    public virtual void Start()
    {
        m_camera = Camera.main;
        m_transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if( m_camera != null )
        {
            Vector2 movement = new Vector2( Input.GetAxisRaw( "Horizontal" ), Input.GetAxisRaw( "Vertical" ) );
            Vector3 playerPosition = m_transform.position;
            Vector3 cameraForward = m_camera.transform.forward;
            cameraForward.y = 0.0f;
            cameraForward.Normalize();
            Vector3 cameraRight = new Vector3( cameraForward.z, 0.0f, -cameraForward.x );
            cameraForward *= movement.y;
            cameraRight *= movement.x;
            playerPosition += cameraForward * Speed * Time.deltaTime;
            playerPosition += cameraRight * Speed * Time.deltaTime;
            m_transform.position = playerPosition;
        }
    }
}
