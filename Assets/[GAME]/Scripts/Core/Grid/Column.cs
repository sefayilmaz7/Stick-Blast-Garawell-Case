using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GarawellGames.Core
{
    [Serializable]
    public class Column
    {
        public List<Cell> CellList;
        public int Index;
        public bool isSolved = false;

        public Column(int index)
        {
            CellList = new List<Cell>();
            Index = index;
        }

        public bool CellListIsUninitializedOrEmpty()
        {
            return CellList == null || CellList.Count <= 0;
        }

        public bool IsColumnFilled()
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

        public void ClearFilledColumn()
        {
            foreach (var cell in CellList)
            {
                if (cell.GetItem() is CellItem cellItem)
                {
                    cellItem.DropBlocks();
                }
            }
        }
        
    }
}
