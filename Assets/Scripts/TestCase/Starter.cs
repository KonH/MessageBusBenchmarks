using UnityEngine;

namespace TestCase {
	public class Starter : MonoBehaviour {
		[SerializeField] GameObject _target;

		public void Update() {
			if ( Input.anyKey ) {
				_target.SetActive(true);
			}
		}
	}
}