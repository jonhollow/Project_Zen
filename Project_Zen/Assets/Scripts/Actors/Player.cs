using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour
{
    #region Fields

    // Jumping variables
    public float m_JumpHeight = 4.0f;
    public float m_TimeToJumpApex = 0.5f;
    public float m_MoveSpeed = 6.0f;
    public float m_AccelerationTimeAirborne = 0.2f;
    public float m_AccelerationTimeGrounded = 0.1f;

    // move from player into the map, so each map can screw with gravity?
    float m_Gravity;
    float m_JumpVelocity;
    Vector3 m_Velocity = Vector3.zero;
    float m_VelocityXSmoothing = 0.0f;

    /// <summary>
    /// Handle to the controller
    /// </summary>
    Controller2D m_ControllerHandle = null;

    #endregion

    #region Methods

    /// <summary>
	/// Grabs necessary handles
	/// </summary>
	void Start() 
    {
        m_ControllerHandle = GetComponent<Controller2D>();

        m_Gravity = -(2 * m_JumpHeight) / Mathf.Pow(m_TimeToJumpApex, 2.0f);
        m_JumpVelocity = Mathf.Abs(m_Gravity) * m_TimeToJumpApex;
        print("Gravity: " + m_Gravity + " Jump Velocity: " + m_JumpVelocity);
	}

    /// <summary>
    /// Updates the player
    /// </summary>
    void Update()
    {

        if (m_ControllerHandle.Info.above || m_ControllerHandle.Info.below)
        {
            m_Velocity.y = 0.0f;
        }

        Vector3 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && m_ControllerHandle.Info.below)
        {
            m_Velocity.y = m_JumpVelocity;
        }


        float targetVelocityX = input.x * m_MoveSpeed;
        m_Velocity.x = Mathf.SmoothDamp(m_Velocity.x, targetVelocityX, ref m_VelocityXSmoothing, (m_ControllerHandle.Info.below ? m_AccelerationTimeGrounded : m_AccelerationTimeAirborne));
        m_Velocity.y += m_Gravity * Time.deltaTime;

        m_ControllerHandle.Move(m_Velocity * Time.deltaTime);
    }

    #endregion
}
