using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Match3Engine
{
    

    public class Swap : MonoBehaviour
    {
        public class SwapExtensions
        {
            public delegate void SwapCallBack(); // This defines what type of method you're going to call.
            public static SwapCallBack m_onStart;// This is the variable holding the method you're going to call.
            public static SwapCallBack m_onComplete;// This is the variable holding the method you're going to call.

            


            bool SpacePressed;
            Vector2Int SpacePressedPosition;
            //void* OnMoveStart;
        }
        public void OnComplete(SwapExtensions.SwapCallBack Complete)
        {
            SwapExtensions.m_onComplete = Complete;
        }
        public void OnStartSwap(SwapExtensions.SwapCallBack Start)
        {
            SwapExtensions.m_onStart = Start;
        }

        /// <summary>Swap One Element with One Other Element of Position Next to It</summary>
        public static class SingleSwap
        {
            
            public static bool CanSwapWithEmptySpaces = false;
            /// <summary>Swap Element While Mouse is Pressed</summary>
            public static bool PositionDragging= false;
            /// <summary>DG Moving tween Extension(Only when Move is Engaged)</summary>
            public static DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[] SingleSwapTween;
            /// <summary>Clicked Position of Swap</summary>
            public static Vector2Int SingleSwapPressedPosition = new Vector2Int(-1, -1);
            /// <summary>Assign Swap direction</summary>
            public static string SwapDirection;
            /// <summary>Single Swap is Active</summary>
            static bool _SingleEngaged;
            public static bool SingleEngaged 
            {
                get { return _SingleEngaged; }
            }
            /// <summary>Swapping is Complete</summary>
            static void SwapComplete()
            {
                SingleSwapPressedPosition = new Vector2Int(-1, -1);
                _SingleEngaged = false;
            }

            /// <summary>Swap the position of 2 elements</summary>
            /// <param name="LayerSwapBound">Swap on positions only where given layer element exist</param>
            /// <param name="LayerSwappingElementsBound">Only Swap over given Element in LayerSwapBound</param>
            /// <param name="LayerSwapping">Layer of swapping elements</param>
            /// <param name="LayerSwappingElements">Only Swap with given Elements</param>
            /// <param name="duaration">Timer for swap</param>
            /// <param name="snapping">Allow Snapping</param>
            public static void Engage(int LayerSwapBound, int[] LayerSwappingElementsBound, int LayerSwapping, int[] LayerSwappingElements, float duaration, bool snapping)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("Grid: " + Grid.MouseOverCellPos());
                    SingleSwapPressedPosition = new Vector2Int(-1, -1);
                }
                if (SingleSwapPressedPosition.x == -1)
                {
                    SingleSwapPressedPosition = Grid.MousePressedCellPos();

                }
                else if (!_SingleEngaged && SingleSwapPressedPosition.x != -1 && Grid.MouseOverCellPos() != SingleSwapPressedPosition && Grid.Layer[LayerSwapBound].GridSpaces[SingleSwapPressedPosition.x, SingleSwapPressedPosition.y].Element != null)
                {
                    
                    string swipedir = Grid.MouseDirectionFromCellPos(SingleSwapPressedPosition.x, SingleSwapPressedPosition.y);
                    Vector2Int nxt = new Vector2Int(-1, -1);
                    if (swipedir == "Down") { nxt = Grid.CellNextToPos(SingleSwapPressedPosition.x, SingleSwapPressedPosition.y, 1, 0, 0, 0); }//Debug.Log( "Down"); }
                    else if (swipedir == "Up") { nxt = Grid.CellNextToPos(SingleSwapPressedPosition.x, SingleSwapPressedPosition.y, 0, 1, 0, 0); }//Debug.Log("Up"); }
                    else if (swipedir == "Left") { nxt = Grid.CellNextToPos(SingleSwapPressedPosition.x, SingleSwapPressedPosition.y, 0, 0, 1, 0); }// Debug.Log("Left"); }
                    else if (swipedir == "Right") { nxt = Grid.CellNextToPos(SingleSwapPressedPosition.x, SingleSwapPressedPosition.y, 0, 0, 0, 1); }//Debug.Log("Right"); }
                    //SwapDirection = swipedir;
                    // Debug.Log("Engaged: " + SingleSwapPressedPosition+" to "+ nxt);

                    bool CanSwapElementsInLayer = false;
                    bool tempnxt = false;
                    bool tempclicked = false;
                    foreach (int LSE in LayerSwappingElements)
                    {
                        if (SingleSwapPressedPosition.x != -1) 
                        {
                            int num = Grid.Layer[LayerSwapping].GridSpaces[SingleSwapPressedPosition.x, SingleSwapPressedPosition.y].Elementnum;
                            if(num == LSE || (!Grid.Layer[LayerSwapping].GridSpaces[SingleSwapPressedPosition.x, SingleSwapPressedPosition.y].Exist() && CanSwapWithEmptySpaces))
                            {
                                tempclicked = true;
                               // Debug.Log("clicked: "+Grid.Layer[LayerSwapping].GridSpaces[SingleSwapPressedPosition.x, SingleSwapPressedPosition.y].Exist());
                            }
                            else { }
                        } //&& (CanSwapWithEmptySpaces || Grid.Layer[LayerSwapping].GridSpaces[SingleSwapPressedPosition.x, SingleSwapPressedPosition.y].Elementnum == LSE)) { tempclicked = true; }
                       
                        if (nxt.x != -1)
                        {
                            int num = Grid.Layer[LayerSwapping].GridSpaces[nxt.x, nxt.y].Elementnum;
                            if (num == LSE || (!Grid.Layer[LayerSwapping].GridSpaces[nxt.x, nxt.y].Exist() && CanSwapWithEmptySpaces))
                            {
                                tempnxt = true;
                            }
                        }
                            
                                
                               // && (CanSwapWithEmptySpaces || Grid.Layer[LayerSwapping].GridSpaces[nxt.x, nxt.y].Elementnum == LSE)) { tempnxt = true; }
                    }
                    if (tempnxt && tempclicked) { CanSwapElementsInLayer = true;  }
                    //if (tempnxt) { Debug.Log("tempnxt"); }
                    //if (tempclicked) { Debug.Log("tempclicked"); }

                    bool CanSwapElementsOverBoundLayer = false;
                    bool tempnxt2 = false;
                    bool tempclicked2 = false;
                    foreach (int LSE in LayerSwappingElementsBound)
                    {
                        // if (SingleSwapPressedPosition.x != -1 && Grid.Layer[LayerSwapBound].GridSpaces[SingleSwapPressedPosition.x, SingleSwapPressedPosition.y].Elementnum == LSE) { tempclicked2 = true; }
                        // if (nxt.x != -1 && Grid.Layer[LayerSwapBound].GridSpaces[nxt.x, nxt.y].Elementnum == LSE) { tempnxt2 = true; }

                        if (SingleSwapPressedPosition.x != -1)
                        {
                            int num = Grid.Layer[LayerSwapBound].GridSpaces[SingleSwapPressedPosition.x, SingleSwapPressedPosition.y].Elementnum;
                            if (num == LSE)// || (!Grid.Layer[LayerSwapBound].GridSpaces[SingleSwapPressedPosition.x, SingleSwapPressedPosition.y].Exist() && CanSwapWithEmptySpaces))
                            {
                                tempclicked2 = true;
                            }
                        } 

                        if (nxt.x != -1)
                        {
                            int num = Grid.Layer[LayerSwapBound].GridSpaces[nxt.x, nxt.y].Elementnum;
                            if (num == LSE)// || (!Grid.Layer[LayerSwapBound].GridSpaces[nxt.x, nxt.y].Exist() && CanSwapWithEmptySpaces))
                            {
                                tempnxt2 = true;
                            }
                        }
                    }
                    if (tempnxt2 && tempclicked2) { CanSwapElementsOverBoundLayer = true; }//Debug.Log("Can Swap over bounds"); }


                    if (CanSwapElementsInLayer && CanSwapElementsOverBoundLayer)// && (Grid.Layer[LayerSwapBound].GridSpaces[nxt.x, nxt.y].Element != null || (CanSwapWithEmptySpaces && Grid.Layer[LayerSwapBound].GridSpaces[nxt.x, nxt.y].Element == null)))
                    {
                       // Debug.Log("Move");
                        var p = MoveControl.SwitchElements(LayerSwapping, SingleSwapPressedPosition, LayerSwapping, nxt, duaration, snapping);
                        //movementQueue(LayerSwapping, SingleSwapPressedPosition, LayerSwapping, nxt, duaration, snapping);
                        SingleSwapTween = p;
                        SwapDirection = swipedir;
                        //press release unneeded
                        if (!PositionDragging)
                        {
                            _SingleEngaged = true;
                            p[0].OnComplete(SwapComplete);
                        }
                        else
                        {
                            //_SingleEngaged = true;
                           // p[0].OnComplete(SwapComplete);
                            SingleSwapPressedPosition = nxt;
                        }

                       // Debug.Log("iEngaged: " + SingleSwapPressedPosition);
                    }
                    else
                    {
                       // SwapDirection = "";
                    }
                    //p[1].OnComplete(c1);
                    // n = new Vector2Int(-1, -1);
                    //Debug.Log(n+ " " + );
                }
                else { SwapDirection = ""; }
            }

        }

        /// <summary>Swap Element(s) with Positions that are Held Down</summary>
        public static class MoveSwap
        {
            /// <summary>Clicked Position of Swap</summary>
            public static Vector2Int MoveSwapPressedPosition = new Vector2Int(-1, -1);
            /// <summary>Move Swap is Active</summary>
            static bool _MoveEngaged;
            public static bool MoveEngaged
            {
                get { return _MoveEngaged; }
            }
            public static void Engage(int LayerSwapBound, int[] LayerSwappingElementsBound, int LayerSwapping, int[] LayerSwappingElements, bool CanSwapWithEmptySpaces, float duaration, bool snapping)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    MoveSwapPressedPosition = new Vector2Int(-1, -1);
                }
                if (MoveSwapPressedPosition.x == -1)
                {
                    MoveSwapPressedPosition = Grid.MousePressedCellPos();

                }
            }

        }


        void Update()
        {
            
        }


    }
}