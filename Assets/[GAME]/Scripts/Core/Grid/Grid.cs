using System;
using System.Collections.Generic;

namespace GarawellGames.Core
{
    [Serializable]
    public class Grid
    {
        public List<Row> RowList;
        public List<Column> ColumnList;

        public int Width;
        public int Height;
        public float CellSize;

        public Grid(int w, int h, float cellSize = 1f)
        {
            RowList = new List<Row>();
            ColumnList = new List<Column>();
            Width = w;
            Height = h;
            CellSize = cellSize;

            Initialize();
        }

        public void Initialize()
        {
            for (int y = 0; y < Height; y++)
            {
                Row newRow = new Row(y);
                RowList.Add(newRow);

                for (int x = 0; x < Width; x++)
                {
                    if (y == 0)
                    {
                        Column newColumn = new Column(x);
                        ColumnList.Add(newColumn);
                    }

                    Cell newCell = new Cell(x, y);
                    newRow.CellList.Add(newCell);
                    
                    ColumnList[x].CellList.Add(newCell);
                }
            }
        }

        public List<Cell> GetAllCells()
        {
            List<Cell> allCells = new List<Cell>();

            foreach(Row row in RowList)
            {
                allCells.AddRange(row.CellList);
            }

            return allCells;
        }

        public List<ItemBase> GetAllItems()
        {
            List<ItemBase> items = new List<ItemBase>();

            foreach (var cell in GetAllCells())
            {
                if (cell.HasItem())
                {
                    items.Add(cell.GetItem());
                }
            }

            return items;
        }

        public List<Cell> GetAllNeighbours(Cell cell, bool includeCrossNeighbours = true)
        {
            List<Cell> neighbours = new List<Cell>();

            int up = cell.Y + 1;
            int left = cell.X - 1;
            int down = cell.Y - 1;
            int right = cell.X + 1;

            if(up < RowList.Count) neighbours.Add(RowList[up].CellList[cell.X]);
            if(left >= 0) neighbours.Add(RowList[cell.Y].CellList[left]);
            if(down >= 0) neighbours.Add(RowList[down].CellList[cell.X]);
            if(right < Width) neighbours.Add(RowList[cell.Y].CellList[right]);

            if(includeCrossNeighbours)
            {
                if(up < RowList.Count && right < Width) neighbours.Add(RowList[up].CellList[right]);
                if(up < RowList.Count && left >= 0) neighbours.Add(RowList[up].CellList[left]);
                if(down >= 0 && left >= 0) neighbours.Add(RowList[down].CellList[left]);
                if(down >= 0 && right < Width) neighbours.Add(RowList[down].CellList[right]);
            }

            return neighbours;
        }

        public Cell GetCellByItem(ItemBase item)
        {
            if(UninitializedOrEmptyRowOrColumnList() || CoordinateIsOutsideGrid(item.X, item.Y))
                return null;

            return RowList[item.Y].CellList[item.X];
        }

        public Cell GetCellByCoordinates(int x, int y)
        {
            if(UninitializedOrEmptyRowOrColumnList() || CoordinateIsOutsideGrid(x, y))
                return null;
            
            return RowList[y].CellList[x];
        }

        /*public List<RegularItem> GetAllRegularItems()
        {
            List<RegularItem> regularItems = new List<RegularItem>();
            foreach (var cell in GetAllCells())
            {
                if (cell.HasItem() && cell.GetItem().IsRegular())
                {
                    regularItems.Add((RegularItem)cell.GetItem());
                }
            }

            return regularItems;
        }*/

        public Cell GetCellByIndex(int index)
        {
            if(UninitializedOrEmptyRowOrColumnList())
                return null;

            int widthOfFirstRow = RowList[0].CellList.Count;
            int allCellsCount = RowList.Count * widthOfFirstRow;

            if(index < 0 || index >= allCellsCount)
                return null;

            int x = index % widthOfFirstRow;
            int y = index / widthOfFirstRow;

            if(CoordinateIsOutsideGrid(x, y))
                return null;

            return RowList[y].CellList[x];
        }

        public int GetIndexByCell(Cell cell)
        {
            if(UninitializedOrEmptyRowOrColumnList() || CoordinateIsOutsideGrid(cell.X, cell.Y))
                return -1;

            int index = RowList[0].CellList.Count * cell.Y + cell.X;
            return index;
        }

        public bool UninitializedOrEmptyRowOrColumnList()
        {
            return GridIsUninitializedOrEmpty() || RowList[0].CellListIsUninitializedOrEmpty();
        }

        public bool CoordinateIsOutsideGrid(int x, int y)
        {
            return y >= RowList.Count || y < 0 || x >= Width || x < 0;
        }

        public bool GridIsUninitializedOrEmpty()
        {
            return RowList == null || RowList.Count <= 0;
        }

        #region Boundaries

        public Cell GetTopMiddleCell()
        {
            if (UninitializedOrEmptyRowOrColumnList())
                return null;

            int middleIndex = Width / 2;
            return RowList[Height - 1].CellList[middleIndex];
        }

        public Cell GetBottomMiddleCell()
        {
            if (UninitializedOrEmptyRowOrColumnList())
                return null;

            int middleIndex = Width / 2;
            return RowList[0].CellList[middleIndex];
        }

        public Cell GetLeftMiddleCell()
        {
            if (UninitializedOrEmptyRowOrColumnList())
                return null;

            int middleIndex = Height / 2;
            return RowList[middleIndex].CellList[0];
        }

        public Cell GetRightMiddleCell()
        {
            if (UninitializedOrEmptyRowOrColumnList())
                return null;

            int middleIndex = Height / 2;
            return RowList[middleIndex].CellList[Width - 1];
        }
        #endregion
    }
}
