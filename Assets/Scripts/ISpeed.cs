using System;

public interface ISpeed
{
	void NormalSpeed();

	void DownSpeed(float down_percent);

	void UpSpeed(float up_percent);

	float GetSpeedScale();
}
