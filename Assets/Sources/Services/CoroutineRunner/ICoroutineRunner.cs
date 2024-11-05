using System.Collections;
using UnityEngine;

namespace Assets.Sources.Services.CoroutineRunner
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
