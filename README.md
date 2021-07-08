# C++ -Creating-a-Match3-Game-Engine
Using C++, The project goal is to create a GUI and a script to produce match3 games

The goal of this project is to create an Unity file that creates match3 games effortlessly.

The code uses the tweening service:
https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676

The code creates has a Match3Engine namespace that creates a grid and elements with layers.

  using Match3Engine; 
  
The code below creates a grid using the Match3Engine

    void CreateGrid ()
    {
        GridParent = GameObject.Find("Game");
        GameObject RSpace = background;//


        //Create the Base of the Game
        Match3Engine.Grid.CreateGridBase(5, 7, RSpace.GetComponent<RectTransform>().sizeDelta, GridParent.transform);
        Match3Engine.Grid.GridContainer.transform.localScale = new Vector3(1, 1, 1);
        
        //Create each layer
        GameObject[] Spaces = new GameObject[2] { null, RSpace};
        Match3Engine.Grid.SetElements(0, Spaces, GridSpaceExist);
        
        //Put Elements in layer
       GameObject[] Blocks = new GameObject[3] { null, block1, block2 };
       Match3Engine.Grid.SetElements(1, Blocks, GridBlockExist);
    }

The code creates a Swapping ability 

    void SetSwap()
    {
        //Setup Single Swap
        Swap.SingleSwap.PositionDragging = false;
        Swap.SingleSwap.CanSwapWithEmptySpaces = true;
        Swap.SingleSwap.Engage(0, new int[] { 1 }, 1, new int[] { 1, 2}, .3f, false);
       
    }
