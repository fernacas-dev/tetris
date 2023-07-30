using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class GeneratorLogic : MonoBehaviour
{

    public GameObject[] tetrominos;

    // Start is called before the first frame update
    void Start()
    {
        AddTetromino();
    }

    public void AddTetromino()
    {
        GameObject shape = tetrominos[Random.Range(0, tetrominos.Length)];
        Instantiate( shape, transform.position, Quaternion.identity);
    }
}
