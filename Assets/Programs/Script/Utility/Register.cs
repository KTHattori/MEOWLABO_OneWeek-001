using UnityEngine;
using UnityEngine.Events;

namespace RegisterObject
{
	[DefaultExecutionOrder(-100)]
	public class Register : MonoBehaviour 
	{
		[SerializeField]
		bool deactivateAfterRegister = false;
		[SerializeField]
        UnityEvent onAwake;
		void Awake()
		{
			onAwake.Invoke();
			if(deactivateAfterRegister) gameObject.SetActive(false);
		}
	}
}