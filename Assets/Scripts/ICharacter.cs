using System;
using UnityEngine;

public interface ICharacter
{
	void Knife();

	void ThrowBomb();

	void Jump();

	void Fire();

	void Movement(Vector2 direction, float speed);

	void Die();
}
