using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Match3Engine;

public class Grid_Example1 : MonoBehaviour
{
    private int GridxMax = 7;
    private int GridyMax = 9;
    private int[,] GridSpaceExist = new int[5, 7]
    {
        {1, 1, 1, 1, 1, 1, 1, },
        {1, 0, 1, 1, 1, 1, 1,},
        {1, 0, 1, 1, 1, 1, 1, },
        {1, 1, 1, 1, 1, 1, 1, },
        {1, 0, 1, 1, 1, 1, 1, }

    };
    /*= new int[7, 9]
{
    {1, 1, 1, 1, 1, 1, 1, 1, 1},
    {1, 0, 1, 1, 1, 1, 1, 0, 1},
    {1, 0, 1, 1, 1, 1, 1, 0, 1},
    {1, 1, 1, 1, 1, 1, 1, 1, 1},
    {1, 0, 1, 1, 1, 1, 1, 0, 1},
    {1, 0, 1, 1, 1, 1, 1, 0, 1},
    {1, 1, 1, 1, 1, 1, 1, 1, 1}

};*/
    private int[,] GridBlockExist = new int[5, 7]
    {
        {1, 1, 1, 1, 1, 2, 1,},
        {1, 0, 1, 1, 1, 1, 2, },
        {2, 0, 2, 1, 2, 2, 1, },
        {1, 0, 1, 2, 2, 2, 2, },
        {1, 0, 2, 1, 2, 1, 1}

    };
        /*= new int[7, 9]
    {
        {1, 0, 1, 1, 1, 1, 1, 0, 1},
        {1, 0, 1, 1, 1, 1, 1, 0, 1},
        {1, 0, 1, 1, 2, 1, 1, 0, 1},
        {1, 0, 1, 2, 2, 2, 3, 0, 1},
        {1, 0, 1, 1, 2, 1, 3, 0, 1},
        {1, 0, 1, 1, 4, 3, 3, 0, 1},
        {1, 0, 1, 1, 1, 1, 1, 0, 1}

    };*/
    private GameObject GridParent;
    public GameObject i;
    public GameObject block1;
    public GameObject block2;

    void CreateGrid ()
    {
        GridParent = GameObject.Find("Game");
        GameObject RSpace = i;//(GameObject)Resources.Load("Image.prefab", typeof(GameObject));

        // GameObject RBlock = (GameObject)Resources.Load("Sprites/GridSprites/Block", typeof(GameObject));
        //Sprite Sp2 = (Sprite)Resources.Load("Sprites/GridSprites/space2", typeof(Sprite));

        // Sprite downright = (Sprite)Resources.Load("Sprites/GridSprites/downright2", typeof(Sprite));
        //Sprite downleft = (Sprite)Resources.Load("Sprites/GridSprites/downleft2", typeof(Sprite));
        //Sprite leftright = (Sprite)Resources.Load("Sprites/GridSprites/leftright0", typeof(Sprite));
        //Sprite leftup = (Sprite)Resources.Load("Sprites/GridSprites/leftup2", typeof(Sprite));
        //Sprite rightup = (Sprite)Resources.Load("Sprites/GridSprites/rightup2", typeof(Sprite));

        //Instantiate different blocks
        //GameObject Block1 = Instantiate(RBlock);
        //Block1.GetComponent<Image>().color = Color.white;
        //Block1.GetComponent<Image>().sprite = downleft;

        // GameObject Block2 = Instantiate(RBlock);
        //Block2.GetComponent<Image>().sprite = downright;

        //GameObject Block3 = Instantiate(RBlock);
        //Block3.GetComponent<Image>().color = Color.red;
        // Block3.GetComponent<Image>().sprite = leftright;

        // GameObject Block4 = Instantiate(RBlock);
        //Block4.GetComponent<Image>().color = Color.black;
        // Block4.GetComponent<Image>().sprite = leftup;

        // GameObject Block5 = Instantiate(RBlock);
        //Block4.GetComponent<Image>().color = Color.black;
        // Block5.GetComponent<Image>().sprite = rightup;


        //Create the Base of the Game
        Match3Engine.Grid.CreateGridBase(5, 7, RSpace.GetComponent<RectTransform>().sizeDelta, GridParent.transform);
        Match3Engine.Grid.GridContainer.transform.localScale = new Vector3(1, 1, 1);
         //Create each layer
         GameObject[] Spaces = new GameObject[2] { null, RSpace};
        Match3Engine.Grid.SetElements(0, Spaces, GridSpaceExist);
        GameObject[] Blocks = new GameObject[3] { null, block1, block2 };
       // GameObject[] Blocks = new GameObject[6] { null, Block1, Block2, Block3, Block4, Block5 };
       Match3Engine.Grid.SetElements(1, Blocks, GridBlockExist);
    }

    void SetSwap()
    {
        //Setup Single Swap
        Swap.SingleSwap.PositionDragging = false;
        Swap.SingleSwap.CanSwapWithEmptySpaces = true;
       Swap.SingleSwap.Engage(0, new int[] { 1 }, 1, new int[] { 1, 2}, .3f, false);
       //Debug.Log(Swap.SingleSwap.SwapDirection);
    }

    void SetSingleSwapDragAlongEmptySpaces()
    {
        Swap.SingleSwap.PositionDragging = false;
        Swap.SingleSwap.CanSwapWithEmptySpaces = true;
        Swap.SingleSwap.Engage(0, new int[] { 1 }, 1, new int[] { 1, 2, 3, 4 }, .05f, false);
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        SetSwap();
        //SetSingleSwapDragAlongEmptySpaces();
    }
}
