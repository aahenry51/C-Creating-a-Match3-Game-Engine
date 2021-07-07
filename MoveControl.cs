using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Match3Engine
{
    public class MoveControl : MonoBehaviour
    {
        //public static Sequence SwitchElementsSequence1 = DOTween.Sequence();
        //public static Sequence SwitchElementsSequence2 = DOTween.Sequence();
        /// <summary>Switch the Elements Given Space (Moves the Element and switches Ref) [Returns DoMove Tween for both element]</summary>
        /// <param name="LayerNum1">Select grid layer (0-9)</param>
        /// <param name="LayerNum2">Select grid layer 2 (0-9)</param>
        /// <param name="pos1">Space pos1</param>
        /// <param name="pos2">Space pos2</param>
        /// <param name="speed">Time duration</param>
        /// <param name="snapping">Snap to position</param>
        public static DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[] 
            SwitchElements(int LayerNum1, Vector2Int pos1, int LayerNum2, Vector2Int pos2,float speed,bool snapping)
        {
            DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> E1 = null;
            DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> E2 = null;

            GameObject ele1 = Match3Engine.Grid.Layer[LayerNum1].GridSpaces[pos1.x, pos1.y].Element;
            GameObject ele2 = Match3Engine.Grid.Layer[LayerNum2].GridSpaces[pos2.x, pos2.y].Element;

            if (ele1 != null)
            {
                Vector3 pos = Match3Engine.Grid.CellLocationPos()[pos2.x, pos2.y];
                // GridControl.Layer[LayerNum2].transform.TransformPoint(Cellbound[0]);
                
                 E1 = ele1.transform.DOLocalMove(new Vector3(pos.x, pos.y, -LayerNum2), speed, false);
               
            }

            if (ele2 != null)
            {
                Vector3 pos = Match3Engine.Grid.CellLocationPos()[pos1.x, pos1.y];
                E2 = ele2.transform.DOLocalMove(new Vector3(pos.x, pos.y, -LayerNum1), speed, false);
            }

            Match3Engine.Grid.SwitchElementsRef(LayerNum1, pos1.x, pos1.y, LayerNum2, pos2.x, pos2.y);

            return new DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>[] { E1, E2 };
        }


    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}