using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreWeapon : MonoBehaviour
{
	public float attackDamage = 20f;
	public float punchMulti = 1f;
	public float overheadMulti = 2f;
	public float jumpMulti = 5f;
	public float kickMulti = 0.5f;

	public Vector3 attackOffset;
	public float punchRange = 0.1f;
	public float weaponRange = 0.5f;

	public Transform punchPoint;
	public Transform overheadPoint;
	public Transform kickPoint;
	public Transform jumpPoint;

	public LayerMask attackMask;

	public void Punch() {
		Vector3 pos = punchPoint.position;

		Collider[] colInfos = Physics.OverlapSphere(pos, punchRange, attackMask);

		foreach (var colInfo in colInfos) {
			if (colInfo.TryGetComponent<PlayerHealth>(out PlayerHealth comp)) {
				comp.ModifyHealth(-(attackDamage * punchMulti));
				return;
			}
		}
	}

	public void Overhead() {
		Vector3 pos = overheadPoint.position;

		Collider[] colInfos = Physics.OverlapSphere(pos, weaponRange, attackMask);

		foreach (var colInfo in colInfos) {
			if (colInfo.TryGetComponent<PlayerHealth>(out PlayerHealth comp)) {
				comp.ModifyHealth(-(attackDamage * overheadMulti));
				return;
			}
		}
	}

	public void Kick() {
		Vector3 pos = kickPoint.position;

		Collider[] colInfos = Physics.OverlapSphere(pos, punchRange, attackMask);

		foreach (var colInfo in colInfos) {
			if (colInfo.TryGetComponent<PlayerHealth>(out PlayerHealth comp)) {
				comp.ModifyHealth(-(attackDamage * kickMulti));
				return;
			}
		}

	}

	public void Jump() {
		Vector3 pos = jumpPoint.position;

		Collider[] colInfos = Physics.OverlapSphere(pos, weaponRange, attackMask);

		foreach (var colInfo in colInfos) {
			if (colInfo.TryGetComponent<PlayerHealth>(out PlayerHealth comp)) {
				comp.ModifyHealth(-(attackDamage * jumpMulti));
				return;
			}
		}
	}
}
