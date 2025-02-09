using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarawellGames.Core
{
    [Serializable]
    public class Row
    {
        public List<Cell> CellList;
        public int Index;
        public bool isSolved = false;

        public Row(int index)
        {
            CellList = new List<Cell>();
            Index = index;
        }

        public bool CellListIsUninitializedOrEmpty()
        {
            return CellList == null || CellList.Count <= 0;
        }
        
        public bool IsRowFilled()
        {
            foreach (var cell in CellList)
            {
                if (cell.GetItem() is CellItem cellItem && !cellItem.IsFilled)
                {
                    return false;
                }
            }

            return true;
        }
        
        public void ClearFilledRow()
        {
            foreach (var cell in CellList)
            {
                if (cell.GetItem() is CellItem cellItem)
                {
                    cellItem.DropBlocks();
                }
            }
        }

        public IEnumerator EarnPrizes()
        {
            foreach (var cell in CellList)
            {
                if (cell.GetItem() is CellItem cellItem)
                {
                    cellItem.EarnPrize();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }
}
