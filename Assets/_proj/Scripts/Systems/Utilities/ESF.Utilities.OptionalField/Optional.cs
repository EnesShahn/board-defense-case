using System;
using UnityEngine;

namespace EnesShahn.Utilities.OptionalField
{
	[Serializable]
	public struct Optional<T>
	{
		[SerializeField] private bool enabled;
		[SerializeField] private T _value;

		public bool Enabled => enabled;
		public T Value
		{
			get { return _value; }
			set
			{
				enabled = true;
				_value = value;
			}
		}

		public Optional(T initialValue)
		{
			enabled = true;
			_value = initialValue;
		}
	}
}