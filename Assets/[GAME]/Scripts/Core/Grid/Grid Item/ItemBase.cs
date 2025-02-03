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
        [SerializeField] private ItemVisual itemVisual;
        public int X;
        public int Y;
        [SerializeField] private bool isSolved = false;

        public void AssignSolvedItem()
        {
            SetSolved();
            SwitchCollider(false);
            //itemVisual.ShowSolvedEffectWithoutTime();
        }

        public void Initialize(int[] coordinates, Vector2 position, Transform parent, Cell cell)
        {
            X = coordinates[0];
            Y = coordinates[1];
            itemVisual.InitializeVisual(position, parent);
            cell.SetItem(this);
        }

        public void Solve()
        {
            SetSolved();
            SwitchCollider(false);
            /*if (IsCircle())
            {
                itemVisual.ShowCircleEffect(() => itemVisual.ShowSolvedEffectCircle());
            }
            else if (IsCross())
            {
                itemVisual.ShowEraseEffect(() => itemVisual.ShowSolvedEffect());
            }*/
            /*else
            {
                itemVisual.ShowTitleSolvedEffect();
            }*/
        }

        public void InputReact()
        {
            //itemVisual.OnReactVFX(); 
        }

        public void SwitchCollider(bool value)
        {
            if (!itemCollider) return;
            itemCollider.enabled = value;
        }
        
        public void SetSolved()
        {
            isSolved = true;
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