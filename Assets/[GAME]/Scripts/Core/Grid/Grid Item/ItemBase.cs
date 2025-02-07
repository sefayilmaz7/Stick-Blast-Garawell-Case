using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GarawellGames.Core
{
    [Serializable][RequireComponent(typeof(ItemVisual))]
    public class ItemBase : MonoBehaviour
    {
        [SerializeField] private Collider2D itemCollider;
        [SerializeField] protected ItemVisual itemVisual;
        public int X;
        public int Y;

        public void Initialize(int[] coordinates, Vector2 position, Transform parent, Cell cell, TargetItem.TargetType targetType)
        {
            X = coordinates[0];
            Y = coordinates[1];
            itemVisual.InitializeVisual(position, parent, targetType);
            cell.SetItem(this);
        }

        //Getter/Setters
        public Vector2 GetCoordinates()
        {
            return new Vector2(X, Y);
        }

        public ItemVisual GetVisual()
        {
            return itemVisual;
        }

    }
}