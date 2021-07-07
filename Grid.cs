using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3Engine
{
    public class Grid : MonoBehaviour
    {
        /// <summary>
        /// Utility functions that deal with available Modules.
        /// Modules defines:
        /// - Unmovable Grid Cell
        /// </summary>

        #region Base Grid
        private static int _BaseCellPosX = 0;
        private static int _BaseCellPosY = 0;
        private static GameObject _GridContainer;
        private static Vector2 _CellSize;
        private static Vector3[,] _CellLocationPos;

        //Read only
        /// <summary>The Maximum Amount of Cells in a Row (Read Only) To Set Use [CreateGridBase] </summary>
        public static int BaseCellPosX
        {
            get { return _BaseCellPosX; }
        }
        /// <summary>The Maximum Amount of Cells in a Colunm (Read Only) To Set Use [CreateGridBase] </summary>
        public static int BaseCellPosY
        {
            get { return _BaseCellPosY; }
        }
        /// <summary>The Parent Object of Grid Elements(Read Only) To Set Use [CreateGridBase] </summary>
        public static GameObject GridContainer
        {
            get { return _GridContainer; }
        }
        /// <summary>Size of Each Cell(Read Only) To Set Use [CreateGridBase] </summary>
        public static Vector2 CellSize
        {
            get { return _CellSize; }
        }

        /// <summary>The Local Position of Cell Center(Read Only)[Parent: GridContainer] To Set Use [CreateGridBase] </summary>
        public static Vector3[,] CellLocationPos()
        {
            Vector3[,] Cellp = new Vector3[_BaseCellPosX,_BaseCellPosY];//new Vector3(x * CellSize.x, -y * CellSize.y, 0);
            for (int i = 0; i < _BaseCellPosX; i++)
            {
                for (int j = 0; j < _BaseCellPosY; j++)
                {
                    RectTransform rect = _GridContainer.GetComponent<RectTransform>();
                    Cellp[i, j] = new Vector3((i * _CellSize.x) - rect.sizeDelta.x/2 + _CellSize.x/2, -j * _CellSize.y + rect.sizeDelta.x / 2 + _CellSize.y / 2, 0);
                }
            }
            return Cellp;
        }
        /// <summary>The Local Position of Cell TopLeft[0] and BottomRight[1](Read Only)[Parent: GridContainer] To Set Use [CreateGridBase] </summary>
        public static Vector3[] CellBounds(int i, int j)
        {
            Vector3[] pos = new Vector3[2];
            //Top Left
            pos[0] = new Vector3(CellLocationPos()[i, j].x - (CellSize.x / 2), CellLocationPos()[i, j].y + (CellSize.y / 2), 0);
            //Bottom Right
            pos[1] = new Vector3(CellLocationPos()[i, j].x + (CellSize.x / 2), CellLocationPos()[i, j].y - (CellSize.y / 2), 0);

            return pos;
        }
        /// <summary>Returns the Cell Grid position Mouse is Over </summary>
        public static Vector2Int MouseOverCellPos()
        {
            Vector3 [] Cellbound;
            Vector3 m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int MouseOver = new Vector2Int(-1, -1);

            Debug.Log("m: " + m);

            for (int i = 0; i < _BaseCellPosX; i++)
            {
                for (int j = 0; j < _BaseCellPosY; j++)
                {
                    Cellbound = CellBounds(i, j);
                    Vector3 TopLeft = _GridContainer.transform.TransformPoint(Cellbound[0]);
                    Vector3 BottomRight = _GridContainer.transform.TransformPoint(Cellbound[1]);
                    if (m.x > TopLeft.x && m.x < BottomRight.x && m.y < TopLeft.y && m.y > BottomRight.y)
                    {
                        MouseOver = new Vector2Int(i,j);//Touch within bounds of x and y
                    }
                   
                }
            }

            return MouseOver;
        }
        /// <summary>Returns the Cell Grid position Mouse Pressed down on (Single Frame) </summary>
        public static Vector2Int MousePressedCellPos()
        {
            Vector2Int MousePressed = new Vector2Int(-1, -1);
         
            if (Input.GetMouseButtonDown(0))
            {
                MousePressed = MouseOverCellPos();
              
            }
            return MousePressed;
        }
        /// <summary>Returns the Cell Grid position Mouse Released on (Single Frame) </summary>
        public static Vector2Int MouseReleasedCellPos()
        {
            Vector2Int MousePressed = new Vector2Int(-1, -1);
           
            if (Input.GetMouseButtonUp(0))
            {
                MousePressed = MouseOverCellPos();

            }
            return MousePressed;
        }
        /// <summary>Returns the Direction("Up","Down","Left","Right") Based on Cell Position to Mouse Position Point Vector </summary>
        /// <param name="i">The Cell Row Location</param>
        /// <param name="j">The Cell Colunm Location</param>
        public static string MouseDirectionFromCellPos(int i, int j)
        {
            _GridContainer.transform.TransformPoint(CellLocationPos()[i, j]);
            Vector3 temptile = _GridContainer.transform.TransformPoint(CellLocationPos()[i, j]);
            // Vector3 m = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
            Vector3 m = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));

            //Current position minus initial position
            Vector3 change = m - ((temptile));
            //float chy = tctrl.curpy - (temptile.GetComponent<obj_grid>().gridy new Vector2((tile_width / 2), -tile_height - 1););
            float deg = (float)((180 / 3.14) * Mathf.Atan2(change.y, change.x));

                if (deg < -45 && deg >= -135) { return "Down"; }//Down
                else if (deg < 135 && deg >= 45) { return "Up"; }//up
                else if (deg < -135 || deg >= 135) { return "Left"; }//Left
                else if (deg < 45 && deg >= 0 || deg < 0 && deg >= -45) { return "Right"; } //right
                else { return "null"; }

  
        }
        /// <summary>Returns a Cell Position Given the Distance From Another Cell Position </summary>
        /// <param name="ipos">The Cell Row Location</param>
        /// <param name="jpos">The Cell Colunm Location</param>
        /// <param name="Down">Amount of Cells Down to return Next Cell</param>
        /// <param name="Up">The Cell Colunm Location</param>
        /// <param name="Left">The Cell Colunm Location</param>
        /// <param name="Right">The Cell Colunm Location</param>
        public static Vector2Int CellNextToPos(int ipos,int jpos,int Down, int Up,int Left, int Right)
        {
            int AmountToMoveDown = Down - Up;
            int AmountToMoveRight = Right - Left;

            int newCellipos = ipos + AmountToMoveRight;
            int newCelljpos = jpos + AmountToMoveDown;

            if((newCellipos >= 0 && newCellipos< BaseCellPosX) && (newCelljpos >= 0 && newCelljpos < BaseCellPosY))
            {
                return new Vector2Int(newCellipos, newCelljpos);
            }
            else
            {
                return new Vector2Int(-1, -1);
            }
        }

        /// <summary>Set a GridBase at a given position. 
        ///  Initiate positions </summary>
        /// <param name="BaseX">The Maximum Amount of Cells in a Row</param>
        /// <param name="BaseY">The Maximum Amount of Cells in a Colunm</param>
        /// <param name="SizeDelta">Length of Cell</param>
        /// <param name="Parent_Base">Parent of GridContainer</param>
        public static void CreateGridBase(int BaseX, int BaseY,Vector2 SizeDelta,Transform Parent_Base)
        {
            _BaseCellPosX = BaseX;
            _BaseCellPosY = BaseY;
            _CellSize = SizeDelta;
            _GridContainer = new GameObject("Empty");
            _GridContainer.AddComponent(typeof(RectTransform));
            _GridContainer.name = "Grid_Container";
            _GridContainer.transform.SetParent(Parent_Base);
            _GridContainer.transform.localPosition = new Vector3(0, 0, 0);
            _GridContainer.transform.localScale = new Vector3(1, 1, 1);
            RectTransform rect = _GridContainer.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(BaseX * SizeDelta.x, BaseY * SizeDelta.y);
        }
        
        #endregion

        /// <summary> Edit Layer of grid</summary>
        public static GridLayer[] Layer = new GridLayer[10];
        static GameObject[] _OBJ_Layers = new GameObject[10];

        /// <summary>Switch the Elements Given Space (Does not move it just switches Space reference)</summary>
        /// <param name="LayerNum1">Select grid layer (0-9)</param>
        /// <param name="LayerNum2">Select grid layer 2 (0-9)</param>
        /// <param name="ipos1">The Space 1 Row Location</param>
        /// <param name="jpos1">The Space 1 Colunm Location</param>
        /// <param name="ipos2">The Space 2 Row Location</param>
        /// <param name="jpos2">The Space 2 Colunm Location</param>
        public static void SwitchElementsRef(int LayerNum1, int ipos1, int jpos1, int LayerNum2, int ipos2, int jpos2)
        {
            GameObject Ele1 = null;
            GameObject Ele2 = null;
            Ele1 = Layer[LayerNum1].GridSpaces[ipos1, jpos1].Element;
            Ele2 = Layer[LayerNum2].GridSpaces[ipos2, jpos2].Element;
            int ele1num = Layer[LayerNum1].GridSpaces[ipos1, jpos1].Elementnum;
            int ele2num = Layer[LayerNum2].GridSpaces[ipos2, jpos2].Elementnum;

            Layer[LayerNum1].GridSpaces[ipos1, jpos1].Element = Ele2;
            Layer[LayerNum2].GridSpaces[ipos2, jpos2].Element = Ele1;

            Layer[LayerNum1].GridSpaces[ipos1, jpos1].Elementnum = ele2num;
            Layer[LayerNum2].GridSpaces[ipos2, jpos2].Elementnum = ele1num;

        }
        /// <summary> Creates layer at depth</summary>
        /// <param name="LayerNum">The layer number of depth (higher = closer to screen)</param>
        public static void CreateGridLayer(int LayerNum)
        {
            if(_GridContainer != null)
            {
                _OBJ_Layers[LayerNum] = new GameObject("Empty");
                _OBJ_Layers[LayerNum].AddComponent(typeof(RectTransform));
                _OBJ_Layers[LayerNum].name = "GridLayer_"+LayerNum;
                _OBJ_Layers[LayerNum].transform.SetParent(_GridContainer.transform);
                _OBJ_Layers[LayerNum].transform.localPosition = new Vector3(0, 0, -LayerNum);
                _OBJ_Layers[LayerNum].transform.localScale = new Vector3(1, 1, 1);
                Layer[LayerNum] = new GridLayer();
                Layer[LayerNum].Layernum = LayerNum;
               // Layer[LayerNum].OBJ_GridLayer = _OBJ_Layers[LayerNum];
                
            }
            else
            {
                Debug.LogError("GridBase not found / Use CreateGridBase before creating a Layer");
            }
        }
        /// <summary>Set list of a GameObject elements that appear on Grid Layer </summary>
        /// <param name="Layer">Select grid layer (0-9)</param>
        /// <param name="Elements">The Element that appear on grid</param>
        /// <param name="GridElementPosition">Location of Grid elements set by Array location</param>
        public static void SetElements(int LayerNum, GameObject[] Elements, int[,] GridElementPosition)
        {
            if (Layer[LayerNum] == null)
            {
                CreateGridLayer(LayerNum);
                Layer[LayerNum].GridSpaces = new Space[_BaseCellPosX, _BaseCellPosY];
            }
            if (Layer[LayerNum] != null)
            {
                //Must be in the cell bounds
                if (GridElementPosition.GetLength(0) == _BaseCellPosX && GridElementPosition.GetLength(1) == _BaseCellPosY)
                {
                    for (int i = 0; i < _BaseCellPosX; i++)
                    {
                        for (int j = 0; j < _BaseCellPosY; j++)
                        {
                            SetSpaceClass(LayerNum, i, j, null,-1);
                            for (int ele = 0; ele < Elements.GetLength(0); ele++)
                            {
                                if (GridElementPosition[i, j] == ele)
                                {
                                    if (Elements[ele] != null)
                                    {
                                        GameObject p = Instantiate(Elements[ele]);
                                        p.transform.SetParent(_OBJ_Layers[LayerNum].transform);
                                        p.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                                        p.transform.localPosition = CellLocationPos()[i, j];
                                        SetSpaceClass(LayerNum, i, j, p,ele);
                                    }
                                    else
                                    {
                                        SetSpaceClass(LayerNum, i, j, null,-1);
                                        Debug.LogWarning("Element " + ele + " is null in set");
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("GridElementPosition - error/ int[,] must equal new int[BaseCellPosX,BaseCellPosY]");
                }
            }
            else
            {
                Debug.LogError("GridBase not found / Use CreateGridBase before creating a Layer");
            }
        }
        /// <summary>Set Space Presets</summary>
        static void SetSpaceClass(int LayerNum, int ipos, int jpos, GameObject Element, int Elementnum)
        {
            Layer[LayerNum].GridSpaces[ipos, jpos] = new Space();
            Layer[LayerNum].GridSpaces[ipos, jpos].Element = Element;
            Layer[LayerNum].GridSpaces[ipos, jpos].Elementnum = Elementnum;
            Layer[LayerNum].GridSpaces[ipos, jpos].posx = ipos;
            Layer[LayerNum].GridSpaces[ipos, jpos].posx = jpos;
        }

        public class Space
        {
            public GameObject Element = null;
            public int Elementnum;
            public int posx = -1;
            public int posy = -1;

            public bool Exist()
            {
                if (Element == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }

        public class GridLayer
        {
            public int Layernum;
            private GameObject OBJ_GridLayer
            {
                get { return _OBJ_Layers[Layernum]; }
            }

            public Space[,] GridSpaces = new Space[_BaseCellPosX, _BaseCellPosY];
  
        }

        

    }
}