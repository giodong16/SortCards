/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [Range(1,6)]
    public int column = 5;
    public int row = 5;
    public float spaceX;
    public float spaceY;

    public GameObject columnPrefab;
    public Transform root;

    private void Start()
    {
        for(int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++) { 
                GameObject newColumn = Instantiate(columnPrefab,root);
                newColumn.transform.position = new Vector2 (,j);
            }
        }
    }
}
*/