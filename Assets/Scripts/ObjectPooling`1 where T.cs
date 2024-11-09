using System;
using System.Collections.Generic;

public class ObjectPooling<T> where T : class, new()
{
	public ObjectPooling(int initialBufferSize, Action<T> ResetAction = null, Action<T> OnetimeInitAction = null)
	{
		this.m_objectStack = new Stack<T>(initialBufferSize);
		this.m_resetAction = ResetAction;
		this.m_onetimeInitAction = OnetimeInitAction;
	}

	public T New()
	{
		if (this.m_objectStack.Count > 0)
		{
			T t = this.m_objectStack.Pop();
			if (this.m_resetAction != null)
			{
				this.m_resetAction(t);
			}
			return t;
		}
		T t2 = (T)((object)null);
		if (this.m_onetimeInitAction != null)
		{
			this.m_onetimeInitAction(t2);
		}
		return t2;
	}

	public void Store(T obj)
	{
		this.m_objectStack.Push(obj);
	}

	public int Count
	{
		get
		{
			return this.m_objectStack.Count;
		}
	}

	private Stack<T> m_objectStack;

	private Action<T> m_resetAction;

	private Action<T> m_onetimeInitAction;
}
