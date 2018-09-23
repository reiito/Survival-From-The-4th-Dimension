using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct LevelGrid
{
  public GameObject grid;
  public float leftBoundry;
  public float rightBoundry;
  public float bottomBoundry;
  public float topBoundry;

  public LevelGrid(float lB, float rB, float bB, float tB)
  {
    grid = null;
    leftBoundry = lB;
    rightBoundry = rB;
    bottomBoundry = bB;
    topBoundry = tB;
  }
}

public class GameController : MonoBehaviour
{
  public PlayerController playerController;

  public LevelGrid levelGrid = new LevelGrid(-23f, 23f, -6f, 21f);

  public LayerMask[] badLayers = new LayerMask[2];

  public Image timeFill;
  public Material[] fillMats = new Material[2];

  public GameObject[] levelTiles = new GameObject[2];

  public Transform enemyParent;
  public GameObject enemyPrefab;

  public float survivalTime = 10.0f;
  public float spawnRate = .5f;
  public int startEnemyMin = 10;
  public int startEnemyMax = 30;

  public GameObject gameOverPanel;
  public Text statusText;
  public Text survivalTimeText;
  public Text endTimeText;
  public Slider survivalTimeSlider;
  public Slider delayTimeSlider;
  public Image survivalTimeFill;
  public Image delayTimeFill;

  float enemySpawnTime;

  bool timeStopped = false;
  public bool GetTimeStopped() { return timeStopped; }
  public void SetTimeStopped(bool value) { timeStopped = value; }

  bool gameOver = false;
  public bool GetGameOver() { return gameOver; }

  bool lose;
  public bool GetLose() { return lose; }
  public void SetLose(bool value) { lose = value; }

  private void Awake()
  {
    survivalTimeSlider.maxValue = survivalTime;
    survivalTimeSlider.value = survivalTimeSlider.maxValue;
    delayTimeSlider.gameObject.SetActive(false);

    enemySpawnTime = spawnRate;
  }

  private void Start()
  {
    delayTimeSlider.maxValue = playerController.timeDelay;
    delayTimeSlider.value = delayTimeSlider.maxValue;

    for (int i = 0; i < Random.Range(startEnemyMin, startEnemyMax); i++)
    {
      SpawnEnemy();
    }
  }

  private void Update()
  {
    if (Input.GetKeyDown("r"))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    if (Input.GetKeyDown("escape"))
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    if (gameOver)
      return;

    survivalTimeText.text = survivalTime.ToString("F2");

    if (lose)
    {
      GameOver();
    }

    if (!timeStopped)
    {
      levelTiles[0].SetActive(true);
      levelTiles[1].SetActive(false);

      timeFill.material = fillMats[0];

      survivalTimeText.color = new Color(0, 0, 0, 1);

      survivalTime -= Time.deltaTime;
      survivalTimeSlider.value = survivalTime;
      if (survivalTime < 0)
      {
        GameOver();
      }
    }
    else
    {
      timeFill.material = fillMats[1];

      survivalTimeText.color = new Color(1, 1, 1, 1);

      levelTiles[0].SetActive(false);
      levelTiles[1].SetActive(true);
    }

    if (delayTimeSlider.value <= 0f)
    {
      delayTimeSlider.gameObject.SetActive(false);
      delayTimeSlider.value = delayTimeSlider.maxValue;
    }

    enemySpawnTime -= Time.deltaTime;

    if (enemySpawnTime < 0)
    {
      SpawnEnemy();
      if (survivalTime < 5f)
      {
        enemySpawnTime = spawnRate / 2f;
      }
      else
      {
        enemySpawnTime = spawnRate;
      }
    }
  }

  void GameOver()
  {
    gameOver = true;
    gameOverPanel.SetActive(true);
    survivalTimeText.text = "0";
    if (lose)
    {
      endTimeText.gameObject.SetActive(true);
      statusText.text = "You Lose!";
      endTimeText.text = survivalTime.ToString("F2");
    }
  }

  void SpawnEnemy()
  {
    Vector3 spawnPosition = new Vector3(Random.Range(levelGrid.leftBoundry, levelGrid.rightBoundry), Random.Range(levelGrid.bottomBoundry, levelGrid.topBoundry), 0f);

    Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 0.5f);
    if (colliders.Length > 0)
    {
      return;
    }

    GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, new Quaternion(0, 0, 0, 0), enemyParent);
    spawnedEnemy.GetComponent<EnemyController>().game = this;
    spawnedEnemy.GetComponent<EnemyController>().player = playerController;
  }
}
