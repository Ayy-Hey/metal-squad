using System;

public interface IState
{
	void SetIdle();

	void SetRun();

	void SetDie();

	void SetAttack();

	void SetHit();

	ECharactor GetState();
}
