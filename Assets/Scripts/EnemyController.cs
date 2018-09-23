using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  public GameController game;
  public PlayerController player;

  float speed;
  float health;
  public float deathRate = 2;

  const float MAX_SPEED = 0.1f;
  const float MIN_SPEED = 0.05f;
  const float HEALTH_SIZE = 5;

  float topSpeed;
  bool inSlowArea = false;

  private void Awake()
  {
    speed = topSpeed = Random.Range(MIN_SPEED, MAX_SPEED);
    health = topSpeed * HEALTH_SIZE;
  }

  private void FixedUpdate()
  {
    if (health <= 0.0f || game.GetGameOver())
    {
      Destroy(gameObject);
    }

    // follow player
    if (player)
    {
      Follow();
      if (game.GetTimeStopped())
      {
        speed = topSpeed / 4f;
      }
      else
      {
        speed = topSpeed;
      }
    }
  }

  // movement behaviour
  protected virtual void Follow()
  {
    // face player
    Vector3 difference = player.GetComponent<Transform>().position - transform.position;
    float rotationZ = (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg) + -90.0f;
    transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

    // static follow
    transform.position = Vector3.MoveTowards(transform.position, player.GetComponent<Transform>().position, speed);

    speed += 0.1f;

    // lerp follow
    //transform.position = Vector3.Lerp(transform.position, playerTransform.position, Time.deltaTime / speed);
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      game.SetLose(true);
    }

    if (collision.gameObject.tag == "Slow")
    {
      Destroy(gameObject);
    }
  }
}
