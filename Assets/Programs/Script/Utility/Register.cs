using UnityEngine;
using UnityEngine.Events;

namespace RegisterObject
{
	[DefaultExecutionOrder(-100)]
	public class Register : MonoBehaviour 
	{
		[SerializeField]
        UnityEvent onAwake;
		void Awake()
		{
			onAwake.Invoke();
		}
	}
}