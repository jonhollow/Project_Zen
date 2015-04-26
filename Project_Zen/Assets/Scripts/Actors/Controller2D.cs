using UnityEngine;
using System.Collections;

/// <summary>
/// Used to control movement in the game
/// </summary>
[RequireComponent (typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    #region Helper Structs

    /// <summary>
    /// Wrapper to help determine how to cast the rays for collision
    /// </summary>
    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    };

    /// <summary>
    /// Helps keep track of where collision has occured
    /// </summary>
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            // Don't hate this 'cause it's beautiful
            above = below = left = right = false;
        }
    };

    #endregion

    #region Fields

    public LayerMask m_CollisionMask;
    CollisionInfo m_CollisionInfo;
    BoxCollider2D boxCollider = null;
    RaycastOrigins raycastOrigins;
    const float skinWidth = 0.015f; // used to adjust the bounding area for the actor
   
    // How to cast the rays, number cast/distance cast
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    float horizontalRaySpacing = 0.0f;
    float verticalRaySpacing = 0.0f;

    #endregion

    #region Properties

    public CollisionInfo Info
    {
        get { return m_CollisionInfo; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Initializes the controller
    /// </summary>
	void Start() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
	}

    /// <summary>
    /// Constructs our bounds with our skin width
    /// </summary>
    /// <returns>Our adjusted bounds</returns>
    Bounds GetBounds()
    {
        Bounds boxBounds = boxCollider.bounds;
        boxBounds.Expand(skinWidth * -2.0f);
        return boxBounds;
    }

    /// <summary>
    /// Adjusts the origins for the raycasts, based on our bounds
    /// </summary>
    void UpdateRaycastOrigins()
    {
        Bounds boxBounds = GetBounds();

        raycastOrigins.bottomLeft = new Vector2(boxBounds.min.x, boxBounds.min.y);
        raycastOrigins.bottomRight = new Vector2(boxBounds.max.x, boxBounds.min.y);
        raycastOrigins.topLeft = new Vector2(boxBounds.min.x, boxBounds.max.y);
        raycastOrigins.topRight = new Vector2(boxBounds.max.x, boxBounds.max.y);
    }

    /// <summary>
    /// Calculates the rays we can cast based on our bounds and the number of desired rays, on both axis
    /// </summary>
    void CalculateRaySpacing()
    {
        // Clamp values count values
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);

        // Cast rays
        Bounds boxBounds = GetBounds();
        verticalRaySpacing = boxBounds.size.x / (verticalRayCount - 1);
        horizontalRaySpacing = boxBounds.size.y / (horizontalRayCount - 1);
    }

    /// <summary>
    /// Adjusts upward velocity based on collision
    /// </summary>
    /// <param name="_velocity">Current velocity</param>
    void VerticalCollisions(
        ref Vector3 _velocity
        )
    {
        // Determine move direction and ray length
        float directionY = Mathf.Sign(_velocity.y);
        float rayLength = Mathf.Abs(_velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; ++i)
        {
            Vector2 rayOrigin = (directionY == -1.0f ? raycastOrigins.bottomLeft : raycastOrigins.topLeft);
            rayOrigin += Vector2.right * (verticalRaySpacing * i + _velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, m_CollisionMask);

            if (hit)
            {
                // Adjust velocity
                _velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                // Update collision info
                m_CollisionInfo.below = directionY == -1.0f;
                m_CollisionInfo.above = directionY == 1.0f;
            }

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
        }
    }

    /// <summary>
    /// Adjusts horizontal velocity based on collision
    /// </summary>
    /// <param name="_velocity">Current velocity</param>
    void HorizontalCollisions(
        ref Vector3 _velocity
        )
    {
        // Determine move direction and ray length
        float directionX = Mathf.Sign(_velocity.x);
        float rayLength = Mathf.Abs(_velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; ++i)
        {
            Vector2 rayOrigin = (directionX == -1.0f ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight);
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, m_CollisionMask);

            if (hit)
            {
                // Adjust velocity
                _velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                // Update collision info
                m_CollisionInfo.left = directionX == -1.0f;
                m_CollisionInfo.right = directionX == 1.0f;
            }

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Moves the actor, adjusts desired velocity based on collision constraints
    /// </summary>
    /// <param name="_velocity">The desired velocity</param>
    public void Move(
        Vector3 _velocity
        )
    {
        UpdateRaycastOrigins();
        m_CollisionInfo.Reset();

        if (_velocity.x != 0.0f) HorizontalCollisions(ref _velocity);
        if (_velocity.y != 0.0f) VerticalCollisions(ref _velocity);

        transform.Translate(_velocity);
    }

    #endregion
}
