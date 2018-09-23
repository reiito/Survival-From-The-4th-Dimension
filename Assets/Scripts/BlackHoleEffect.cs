using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BlackHoleEffect : MonoBehaviour
{
  public Shader shader;
  public Transform blackHole;
  public float ratio;
  public float radius;

  public GameController gameController;

  Camera mainCamera;
  Material _material;

  Material material
  {
    get
    {
      if (_material == null)
      {
        _material = new Material(shader);
        _material.hideFlags = HideFlags.HideAndDontSave;
      }

      return _material;
    }
  }

  private void OnEnable()
  {
    mainCamera = GetComponent<Camera>();
    ratio = 1f / mainCamera.aspect;
  }

  private void OnDisable()
  {
    if (_material)
    {
      DestroyImmediate(_material);
    }
  }

  Vector3 wtsp;
  Vector2 pos;

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (shader && material && blackHole)
    {
      wtsp = mainCamera.WorldToScreenPoint(blackHole.transform.position);

      if (wtsp.z > 0)
      {
        pos = new Vector2(wtsp.x / mainCamera.pixelWidth, wtsp.y / mainCamera.pixelHeight);

        _material.SetVector("_Position", pos);
        _material.SetFloat("_Ratio", ratio);
        _material.SetFloat("_Rad", radius);
        _material.SetFloat("_Distance", Vector3.Distance(blackHole.transform.position, transform.position));

        Graphics.Blit(source, destination, _material);
      }
    }
  }
}
