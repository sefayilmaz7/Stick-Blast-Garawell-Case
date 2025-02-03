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
    }
}
