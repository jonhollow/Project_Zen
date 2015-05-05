using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour
{
    #region Fields

    public int m_PlayerID = 1;

    protected string m_ActionJump;
    protected string m_ActionHorizontal;
    protected string m_ActionVertical;
    protected string m_ActionAttack;

    // Jumping variables
    public float m_JumpHeight = 4.0f;
    public float m_TimeToJumpApex = 0.5f;
    public float m_MoveSpeed = 6.0f;
    public float m_AccelerationTimeAirborne = 0.2f;
    public float m_AccelerationTimeGrounded = 0.1f;
    public float m_AccelerationTimeWall = 0.1f;
    public float m_WallFallSpeed = 0.5f;
    public float m_WallMoveWait = 0.025f; // if opposite direction pressed we'll wait this much to move off, so the player can adjust for jump without disconnecting
    public int m_InAirJumps = 1;

    float m_Gravity; // move from player into the map, so each map can screw with gravity?
    float m_JumpVelocity;
    protected Vector3 m_Velocity = Vector3.zero;
    float m_VelocityXSmoothing = 0.0f;
    float m_VelocityYSmoothing = 0.0f;

    float m_WallMoveWaitTimer = 0.0f;
    int m_NumAirJumps = 0;

    // Player specific helpers
    protected float m_AttackChargeTimer = 0.0f;
    bool m_MovementEnabled = true;
    bool m_AttacksEnabled = true;

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
        // Adds the object to the level
        GameController.Instance.AddObjectToLevel(gameObject);

        m_ControllerHandle = GetComponent<Controller2D>();

        m_Gravity = -(2 * m_JumpHeight) / Mathf.Pow(m_TimeToJumpApex, 2.0f);
        m_JumpVelocity = Mathf.Abs(m_Gravity) * m_TimeToJumpApex;

        // Calculate input strings
        m_ActionJump = "Player" + m_PlayerID + " Jump";
        m_ActionHorizontal = "Player" + m_PlayerID + " Horizontal";
        m_ActionVertical = "Player" + m_PlayerID + " Vertical";
        m_ActionAttack = "Player" + m_PlayerID + " Attack";
	}

    /// <summary>
    /// Updates the player
    /// </summary>
    void Update()
    {
        if (m_MovementEnabled) Movement();
        if (m_AttacksEnabled) Attacks();

        ApplyForces();

        if (m_Velocity.magnitude > 0.0f)
            m_ControllerHandle.Move(m_Velocity * Time.deltaTime);
    }

    void Movement()
    {
        if (m_ControllerHandle.Info.above || m_ControllerHandle.Info.below)
        {
            m_Velocity.y = 0.0f;
        }

        float axisRaw = Input.GetAxis(m_ActionHorizontal);
        float moveX = 0.0f;
        if (axisRaw < -0.3f) moveX = -1.0f;
        if (axisRaw > 0.3f) moveX = 1.0f;

        // if we're on the left wall and we're trying to move right or
        // if we're on the right wall and we're trying to move left
        if (m_ControllerHandle.Info.left && moveX > 0.0f ||
            m_ControllerHandle.Info.right && moveX < 0.0f)
        {
            // Don't allow movement until we've held that direction for a set timer
            // this lets the player move a direction and get ready for a jump!
            m_WallMoveWaitTimer += Time.deltaTime;
            if (m_WallMoveWaitTimer < m_WallMoveWait)
                moveX = 0.0f;
        }
        else
            m_WallMoveWaitTimer = 0.0f;

        // If we're on the ground, reset air jump handle
        if (m_ControllerHandle.Info.below)
            m_NumAirJumps = 0;

        if (Input.GetButtonDown(m_ActionJump))
        {
            if (m_ControllerHandle.Info.below)
                m_Velocity.y = m_JumpVelocity;
            else if (m_ControllerHandle.Info.right)
            {
                Vector3 angledVelocity = new Vector3(-0.5f, 1.0f, 0.0f);
                m_Velocity = angledVelocity.normalized * m_JumpVelocity;
            }
            else if (m_ControllerHandle.Info.left)
            {
                Vector3 angledVelocity = new Vector3(0.5f, 1.0f, 0.0f);
                m_Velocity = angledVelocity.normalized * m_JumpVelocity;
            }
            else if (!m_ControllerHandle.Info.below && m_NumAirJumps < m_InAirJumps)
            {
                m_Velocity.y = m_JumpVelocity;
                m_NumAirJumps++;
            }
        }

        float targetVelocityX = moveX * m_MoveSpeed;
        m_Velocity.x = Mathf.SmoothDamp(m_Velocity.x, targetVelocityX, ref m_VelocityXSmoothing, (m_ControllerHandle.Info.below ? m_AccelerationTimeGrounded : m_AccelerationTimeAirborne));
    }

    void ApplyForces()
    {
        m_Velocity.y += m_Gravity * Time.deltaTime;

        // Wall sliding
        if ((m_ControllerHandle.Info.left || m_ControllerHandle.Info.right) && m_Velocity.y < 0.0f) // if we're on a wall and falling
        {
            m_Velocity.y = Mathf.SmoothDamp(m_Velocity.y, m_WallFallSpeed, ref m_VelocityYSmoothing, m_AccelerationTimeWall);
        }
    }


    void Attacks()
    {
    }

    #endregion
}
