using System;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;//rajout
public class Grid : MonoBehaviour
{
    [FormerlySerializedAs("m_ShapePrefab")]
    [SerializeField]
    private GameObject shapePrefab;

    public Ex2Config config;

    public Color[,] Colors { get; private set; }

    private int _width;
    private int _height;
    public Dictionary<int, float> HPReceived = new();//rajout
    public Circle[] circles;//rajout
    public float[,] Health { get; private set; }
    public SpriteRenderer[] spriteRenderers;



    // Start is called before the first frame update
    private void Start()
    {

        var size = (float)config.nbCircles;
        var ratio = Camera.main!.aspect;
        _height = (int)Math.Round(Math.Sqrt(size / ratio));
        _width = (int)Math.Round(size / _height);
        circles = new Circle[_height * _width];//rajout
        spriteRenderers=new SpriteRenderer[_height*_width];

        Colors = new Color[_width, _height];
        Health = new float[_width, _height];
        var bottomLeftCorner = new Vector3(-_width / 2.0f, -_height / 2.0f, 0);
        var halfHeight = _height / 2f;
        var invWidth = 1f / _width;
        var invHeight = 1f / _height;
        //var next_id=0;//rajout

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

                // HPReceived[next_id]=0;//rajout
                //shape.GetComponent<Circle>().id = next_id++;//rajout
                //circles[i * _height + j] = shape.GetComponent<Circle>();//rajout
                //Health[i,j]=circles[i * _height + j].Health;
                //spriteRenderers[i * _height + j]=circles[i * _height + j].GetComponent<SpriteRenderer>();
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateColors();
        //UpdateCircle();
    }

    private void UpdateColors()
    {
        for (var j = 0; j < _height; j++)
        {
            for (var i = 0; i < _width; i++)
            {
                if (j >= _height - 1) continue;
                (Colors[i, j], Colors[i, j + 1]) = (Colors[i, j + 1], Colors[i, j]);

                //Circle circle=circles[j*_width+i];//rajout
                //circle.GetComponent<SpriteRenderer>().color = Colors[i, j] * circle.Health / 1000;//rajout

            }
        }
    }
    /*
    private void UpdateCircle()
    {
        for (var j = 0; j < _height; j++)
        {
            for (var i = 0; i < _width; i++)
            {
                //(Colors[i, j], Colors[i, j + 1]) = (Colors[i, j + 1], Colors[i, j]);
                //Circle circle = circles[j * _width + i];//rajout
                //float radius=circle.getHealingRange();
                //float healingPerSecond=circle.getHealingPerSecond();
                //float baseHealth=circle.getBaseHeath();
                for (var j2 = 0; j2 < _height; j2++)
                {
                    for (var i2 = 0; i2 < _width; i2++)
                    {
                        if(Math.Sqrt((i-i2)^2+(j-j2)^2)<=3){
                            //Circle circle2 = circles[j2 * _width + i2];
                            float hpReceived= 1 * Time.deltaTime;
                            Health[i,j] += hpReceived;
                            Health[i,j] = Mathf.Clamp(Health[i,j], 0, 1000);
                        }
                    }
                }
                //circle.GetComponent<SpriteRenderer>().color = Colors[i, j] * Health[i,j] / 1000;//rajout
                spriteRenderers[i * _height + j].color = Colors[i, j] * Health[i,j] / 1000;//rajout
            }
        }
    }
    */
    public int getHeight(){
        return _height;
    }
    public int getWidth(){
        return _width;
    }
}
