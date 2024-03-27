using System.Collections;
using UnityEngine;

namespace CodeBase.Game
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}