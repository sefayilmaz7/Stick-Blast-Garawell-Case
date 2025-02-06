using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellItemCorner : MonoBehaviour
{
    [SerializeField] private Collider2D cornerCollider;

    public void SwitchCornerCollider(bool enable)
    {
        cornerCollider.enabled = enable;
    }
}
