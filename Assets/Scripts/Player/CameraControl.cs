using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float RotationSpeed = 1.0f;
    public float DistanceSpeed = 10.0f;
    public float DeadZone = 0.1f;
    public float MinDistance = 3.0f;
    private Transform m_playerTransform = null;
    private Transform m_transform = null;
    private float m_distanceToPlayer = 10.0f;
    private Vector3 m_PlayerOffset = new Vector3();

    // Use this for initialization
    public virtual void Start()
    {
        m_playerTransform = FindObjectOfType<PlayerControls>().transform;
        m_transform = transform;

        m_distanceToPlayer = ( m_transform.position - m_playerTransform.position ).magnitude;
        m_PlayerOffset = m_transform.position - m_playerTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if( m_playerTransform != null )
        {
            Vector3 playerPosition = m_playerTransform.position;
            Vector3 cameraPosition = m_transform.position;

            float horizontalMovement = Input.GetAxis( "CameraHorizontal" );
            float virticalMovement  = Input.GetAxis( "CameraVertical" );
            if( Mathf.Abs( virticalMovement ) > DeadZone )
            {
                virticalMovement *= RotationSpeed * Time.deltaTime;
                Vector2 rotation = Rotate( m_PlayerOffset.x, m_PlayerOffset.y, virticalMovement );
                m_PlayerOffset.x = rotation.x;
                m_PlayerOffset.y = rotation.y;
            }

            if( Mathf.Abs( horizontalMovement ) > DeadZone )
            {
                horizontalMovement *= RotationSpeed * Time.deltaTime;
                Vector2 rotation = Rotate( m_PlayerOffset.x, m_PlayerOffset.z, horizontalMovement );
                m_PlayerOffset.x = rotation.x;
                m_PlayerOffset.z = rotation.y;
            }
            Vector3 direction = m_PlayerOffset.normalized;
            m_transform.position = playerPosition + ( direction * m_distanceToPlayer );
            m_transform.forward = -direction;
        }
    }

    private Vector2 Rotate( float a, float b, float amount )
    {
        Vector2 dir = new Vector2( a, b );
        dir.x = dir.x * Mathf.Cos( amount ) - dir.y * Mathf.Sin( amount );
        dir.y = dir.x * Mathf.Sin( amount ) + dir.y * Mathf.Cos( amount );
        return dir;
    }
}
