using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
  public IEnumerator Shake(float duration, float magnitude, float delayRate)
  {
    Vector3 origPos = transform.localPosition;

    float elapsed = 0.0f;

    while (elapsed < duration)
    {
      float x = Random.Range(-1f, 1f) * magnitude;
      float y = Random.Range(-1f, 1f) * magnitude;

      transform.localPosition = new Vector3(x, y, origPos.z);

      elapsed += Time.deltaTime;

      if (delayRate == 0)
      {
        yield return null;
      }
      else
      {
        yield return new WaitForSeconds(duration / delayRate);
      }
    }

    transform.localPosition = origPos;
  }
}
