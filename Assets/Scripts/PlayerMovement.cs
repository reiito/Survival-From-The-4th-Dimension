using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
  public float jumpForce = 750f;
  public float smoothing = .025f;
  public LayerMask groundLayer;
  public Transform groundTransform;
  public CameraShake cameraShake;

  public UnityEvent OnLandEvent;
  [System.Serializable]
  public class BoolEvent : UnityEvent<bool> { }

  float groundedRadius = .2f;
  bool onGround;
  Rigidbody2D playerBody;
  bool facingRight = true;
  Vector3 velocity = Vector3.zero;

  private void Awake()
  {
    playerBody = GetComponent<Rigidbody2D>();

    if (OnLandEvent == null)
      OnLandEvent = new UnityEvent();
  }

  private void FixedUpdate()
  {
    bool wasGrounded = onGround;
    onGround = false;

    Collider2D[] colliders = Physics2D.OverlapCircleAll(groundTransform.position, groundedRadius, groundLayer);
    for (int i = 0; i < colliders.Length; i++)
    {
      if (colliders[i].gameObject != gameObject)
      {
        onGround = true;
        if (!wasGrounded)
        {
          StartCoroutine(cameraShake.Shake(.05f, .01f, 0));
          OnLandEvent.Invoke();
        }
      }
    }
  }

  public void Move(float move, bool jump)
  {
    Vector3 targetVelocity = new Vector2(move * 10f, playerBody.velocity.y);
    playerBody.velocity = Vector3.SmoothDamp(playerBody.velocity, targetVelocity, ref velocity, smoothing);

    if (move > 0 && !facingRight)
    {
      Flip();
    }
    else if (move < 0 && facingRight)
    {
      Flip();
    }

    if (onGround && jump)
    {
      onGround = false;
      playerBody.AddForce(new Vector2(0f, jumpForce));
    }
  }

  void Flip()
  {
    facingRight = !facingRight;

    Vector3 theScale = transform.localScale;
    theScale.x *= -1;
    transform.localScale = theScale;
  }
}
