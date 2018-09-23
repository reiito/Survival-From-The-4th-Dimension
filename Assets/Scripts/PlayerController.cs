using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public GameController gameController;
  public GameObject cameraHandle;
  public Camera mainCamera;
  public Transform destroyCircle;

  public Sprite[] playerSprites = new Sprite[2];

  public float runSpeed = 40.0f;
  public float destroyRadius = 500f;
  public float timeDelay;
  public float currentTimeDelay;

  PlayerMovement playerMovement;
  Rigidbody2D rigidBody;

  float horizontalMove = 0.0f;
  float verticalMove = 0.0f;

  bool jump = true;

  private void Awake()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    playerMovement = GetComponent<PlayerMovement>();
    destroyCircle.gameObject.SetActive(false);
    currentTimeDelay = timeDelay;
  }

  private void Update()
  {
    if (gameController.GetGameOver())
    {
      if (gameController.GetLose())
        Destroy(gameObject);

      return;
    }

    Vector3 sliderToPlayer = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    sliderToPlayer.y = sliderToPlayer.y + Screen.width / 12;
    gameController.delayTimeSlider.gameObject.transform.position = sliderToPlayer;

    horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
    verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;

    if (!gameController.GetTimeStopped())
    {
      if (Input.GetButtonDown("PauseTime") && currentTimeDelay > 0)
      {
        PausePlayer();
      }

      if (verticalMove > 0)
      {
        jump = true;
      }
    }
    else
    {
      destroyCircle.gameObject.SetActive(true);
      currentTimeDelay -= Time.deltaTime;
      gameController.delayTimeSlider.value = currentTimeDelay;

      if (Input.GetButtonDown("PauseTime"))
      {
        UnpausePlayer();
      }
    }

    if (currentTimeDelay <= 0.0)
    {
      UnpausePlayer();
    }

    // NOTE: Test out holding to pause instead
  }

  private void FixedUpdate()
  {
    if (!gameController.GetTimeStopped())
    {
      playerMovement.Move(horizontalMove * Time.fixedDeltaTime, jump);
      jump = false;
    }
    else
    {
      if (horizontalMove > 0)
      {
        destroyCircle.position += new Vector3(0.1f, 0, 0);
      }
      else if (horizontalMove < 0)
      {
        destroyCircle.position -= new Vector3(0.1f, 0, 0);
      }

      if (verticalMove > 0)
      {
        destroyCircle.position += new Vector3(0, 0.1f, 0);
      }
      else if (verticalMove < 0)
      {
        destroyCircle.position -= new Vector3(0, 0.1f, 0);
      }

      // put restrictions here
    }
  }

  void PausePlayer()
  {
    GetComponent<SpriteRenderer>().sprite = playerSprites[1];
    StartCoroutine(cameraHandle.GetComponentInChildren<CameraShake>().Shake(.2f, .01f, 10));
    mainCamera.backgroundColor = new Color(0, 0, 0, 1);
    gameController.SetTimeStopped(true);
    rigidBody.gravityScale = 0f;
    rigidBody.velocity = new Vector2(0f, 0f);
    gameController.delayTimeSlider.gameObject.SetActive(true);
  }

  void UnpausePlayer()
  {
    mainCamera.backgroundColor = new Color(1, 1, 1, 1);
    GetComponent<SpriteRenderer>().sprite = playerSprites[0];
    destroyCircle.position = transform.position;
    destroyCircle.gameObject.SetActive(false);
    gameController.SetTimeStopped(false);
    rigidBody.gravityScale = 3f;
    gameController.delayTimeSlider.gameObject.SetActive(false);
  }
}
