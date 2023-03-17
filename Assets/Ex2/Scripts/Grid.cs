using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Grid : MonoBehaviour
{
    [FormerlySerializedAs("m_ShapePrefab")] [SerializeField]
    private GameObject shapePrefab;

    public Ex2Config config;

    public Color[,] Colors { get; private set; }

    private int _width;
    private int _height;

    // Start is called before the first frame update
    private void Start()
    {
        var size = (float)config.nbCircles;
        var ratio = Camera.main!.aspect;
        _height = (int)Math.Round(Math.Sqrt(size / ratio));
        _width = (int)Math.Round(size / _height);
        
        Colors = new Color[_width, _height];
        var bottomLeftCorner = new Vector3(-_width / 2.0f, -_height / 2.0f, 0);
        var halfHeight = _height / 2f;
        var invWidth = 1f / _width;
        var invHeight = 1f / _height;

        for (var i = 0; i < _width; i++)
        {
            for (var j = 0; j < _height; j++)
            {
                var r = i * invWidth;
                var g = Mathf.Abs(j - halfHeight) * invHeight;
                var b = r * g;
                Colors[i, j] = new Color(r, g, b);
                var shape = Instantiate(shapePrefab, bottomLeftCorner + new Vector3(i, j, 0), Quaternion.identity);
                shape.GetComponent<Circle>().i = i;
                shape.GetComponent<Circle>().j = j;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateColors();
    }

    private void UpdateColors()
    {
        for (var j = 0; j < _height; j++)
        {
            for (var i = 0; i < _width; i++)
            {
                if (j >= _height - 1) continue;
                (Colors[i, j], Colors[i, j + 1]) = (Colors[i, j + 1], Colors[i, j]);
            }
        }
    }
}
