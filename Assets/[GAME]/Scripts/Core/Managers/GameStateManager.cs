using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    public static event UnityAction OnLevelSucces;
    public static event UnityAction OnLevelFailed;
}
