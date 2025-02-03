using System;
using UnityEngine;

namespace GarawellGames.Core
{
    [Serializable]
    public class Cell
    {
        public int X;
        public int Y;
        public float Size;

        public ItemBase Item;

        public Cell(int x, int y, ItemBase item = null, float size = 1)
        {
            X = x;
            Y = y;
            Item = item;
            Size = size;
        }

        public Vector2 GetCenterPosition()
        {
            float pos = Size / 2f;
            return new Vector2(pos, pos);
        }

        public ItemBase GetItem()
        {
            return Item;
        }

        public void SetItem(ItemBase item)
        {
            Item = item;
        }

        public void ClearItem()
        {
            Item = null;
        }

        public bool HasItem()
        {
            return Item != null;
        }

        public bool TryGetItem(out ItemBase item)
        {
            if (Item != null)
            {
                item = Item;
                return true;
            }

            item = null;
            return false;
        }

        public bool TrySetItem(ItemBase item)
        {
            if (item != null)
            {
                Item = item;
                return true;
            }

            return false;
        }

        public bool TryClearItem()
        {
            if(!HasItem())
                return false;

            ClearItem();
            return true;
        }
    }
}
