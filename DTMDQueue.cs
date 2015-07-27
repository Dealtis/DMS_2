using System;
using System.Collections.Generic;

namespace DMSvStandard
{
	public class DTMDQueue<T>: List<T>
	{
		public event EventHandler OnAdd;

		public void AddItem(T item)
		{
			base.Add(item);

			if (null != OnAdd)
				OnAdd(this, null);
		}
	}
}

