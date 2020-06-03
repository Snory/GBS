using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Grid : MonoBehaviour
{

    private Node[,] _grid;
    [SerializeField]
    private Vector2 _gridWorldSize = new Vector2(200, 200);
    [SerializeField]
    private float _size = 1.0f;
    private int  _gridSizeX, _gridSizeY;
    private Camera _mainCamera;

    [SerializeField]
    private GameObject _test;


    public float Size { get; set; }

    private void Awake()
    {

        _gridSizeX = Mathf.RoundToInt(_gridWorldSize.x / _size);
        _gridSizeY = Mathf.RoundToInt(_gridWorldSize.y / _size);
        _grid = new Node[_gridSizeX, _gridSizeY];
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
       
    }


    // Start is called before the first frame update
    void Start()
    {
        //nastavit startovní pozici vlevo nahoře
        Vector3 worldBottomRight = this.transform.position + (Vector3.left * _gridWorldSize.x / 2) + (Vector3.down * _gridWorldSize.y / 2);

        for(int i = 0; i < _gridSizeX; i++)
        {
            for(int j = 0; j < _gridSizeY; j++)
            {
                if(j%2 == 0) { 
                    Instantiate(_test, worldBottomRight + new Vector3(i * _size, j * _size,0)  , Quaternion.identity);
                } else
                {
                    GameObject hex = Instantiate(_test, worldBottomRight + new Vector3(i * (_size) + _size / 2, j * (_size), 0), Quaternion.identity);
                    hex.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }
        }




       //vytvořit grid
       //vykreslit grid
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
