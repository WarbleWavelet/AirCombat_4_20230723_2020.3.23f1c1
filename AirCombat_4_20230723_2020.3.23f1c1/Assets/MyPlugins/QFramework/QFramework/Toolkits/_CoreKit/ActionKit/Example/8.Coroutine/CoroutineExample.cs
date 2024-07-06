using System.Collections;
using UnityEngine;

namespace QFramework.Example
{
    public class CoroutineExample : MonoBehaviour
    {
        private void Start()
        {
            ActionKit.Coroutine(SomeCoroutine).Start(this);
            
            SomeCoroutine().ToAction().Start(this);

            ActionKit.Sequence()
                .Coroutine(SomeCoroutine)
                .Start(this);

            ActionKit.Repeat(5) // -1¡¢0 means forever 1 means once  2 means twice
                    .Condition(() => Input.GetMouseButtonDown(1))
                    .Callback(() => Debug.Log("Mouse right clicked"))
                    .Start(this, () =>
                    {
                        Debug.Log("Right click finished");
                    });
        }

        IEnumerator SomeCoroutine()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("Hello:" + Time.time);
        }
    }
}