using System.Collections;
using UnityEngine;

public class Algorithms : MonoBehaviour
{
    [SerializeField]
    private GameObject block;
    [SerializeField]
    private float speed;
    private GameObject[] block_arrays;

    [SerializeField]
    private int number_of_blocks;
    [SerializeField]
    private int max_height;
    [SerializeField]
    private float Blocks_gap;
    [SerializeField]
    private float Delay;
    [SerializeField]
    private Transform Generator;
    bool done = false;
    void Start()
    {
        done = false;
        
        
    }
    private void Reset_List()
    {
        if (block_arrays != null)
        {
            for (int i = 0; i < block_arrays.Length; i++)
            {
                Destroy(block_arrays[i]);
            }
            block_arrays = null;
        }
    }
    void GenerateBlocks()
    {
        block_arrays = new GameObject[number_of_blocks];

        for (int i = 0; i < number_of_blocks; i++)
        {
            int randomHeight = Random.Range(1, max_height + 1);
            GameObject instance = Instantiate(block, Generator.position, Quaternion.identity);
            instance.transform.position = new Vector3(Generator.position.x + i * instance.transform.localScale.x, Generator.position.y + (randomHeight / 2.0f), Generator.position.z);
            instance.transform.localScale = new Vector3(instance.transform.localScale.x - Blocks_gap, randomHeight, instance.transform.localScale.z);
            instance.transform.parent = Generator;

            block_arrays[i] = instance;

        }
    }

    void Swap(GameObject[] my_array, int index_x, int index_y)
    {
        
        GameObject temporary = my_array[index_x];
        my_array[index_x] = my_array[index_y];
        my_array[index_y] = temporary;

        Vector3 pos = my_array[index_x].transform.localPosition;

        LeanTween.moveLocalX(my_array[index_x], my_array[index_y].transform.localPosition.x, Delay);
        LeanTween.moveLocalZ(my_array[index_x], -3, Delay/2f).setLoopPingPong(1);
        LeanTween.moveLocalX(my_array[index_y], pos.x, Delay);
        LeanTween.moveLocalZ(my_array[index_y], 3, Delay / 2f).setLoopPingPong(1);
    }



    IEnumerator bubble_sort(GameObject[] unsorted_array)
    {
        done = true;
        for (int x = 0; x < number_of_blocks; x++)
        {

            for (int y = 0; y < number_of_blocks - x - 1; y++)
            {

                
                if (unsorted_array[y].transform.localScale.y > unsorted_array[y + 1].transform.localScale.y)
                {
                    yield return new WaitForSeconds(0.5f);
                    Swap(unsorted_array, y, y + 1);
                }
            }

        }
        done = false;
    }

    IEnumerator selection_sort(GameObject[] unsorted_array)
    {
        done = true;
        int minima;
        for (int x = 0; x < number_of_blocks; x++)
        {
            minima = x;
            yield return new WaitForSeconds(Delay);

            for (int y = x+1; y < number_of_blocks; y++)
            {
                if (unsorted_array[y].transform.localScale.y < unsorted_array[minima].transform.localScale.y)
                    minima = y;
                    
            }
            if (minima != x)
            {
                yield return new WaitForSeconds(Delay+0.1f);
                Swap(unsorted_array, x, minima);
            }
            unsorted_array[x].GetComponent<Renderer>().material.color = Color.red;  
        }
        done = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Selection"))
        {
            if (done != true)
            {
                Reset_List();
                GenerateBlocks();
                StartCoroutine(selection_sort(block_arrays));
            }
                
        }
        else if (collision.gameObject.CompareTag("Bubble"))
        {
            if (done != true)
            {
                Reset_List();

                GenerateBlocks();
                StartCoroutine(bubble_sort(block_arrays));
            }
                
        }
    }
}
