using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public Transform target;

  float xMax = 15.35f;
  float xMin = -15.35f;

  float yMax = 16.5f;
  float yMin = 0.5f;

  private void LateUpdate()
  {
    if (target)
      transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), transform.position.z);
  }
}
