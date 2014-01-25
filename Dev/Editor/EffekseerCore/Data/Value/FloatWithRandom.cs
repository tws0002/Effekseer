﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Effekseer.Utl;

namespace Effekseer.Data.Value
{
	public class FloatWithRandom
	{
		float _value_center = 0;
		float _value_max = 0;
		float _value_min = 0;
		
		float _max = float.MaxValue;
		float _min = float.MinValue;
		
		public float Center
		{
			get
			{
				return GetCenter();
			}

			set
			{
				SetCenter(value);
			}
		}

		public float Max
		{
			get
			{
				return GetMax();
			}

			set
			{
				SetMax(value);
			}
		}

		public float Min
		{
			get
			{
				return GetMin();
			}

			set
			{
				SetMin(value);
			}
		}

		public float Amplitude
		{
			get
			{
				return GetAmplitude();
			}

			set
			{
				SetAmplitude(value);
			}
		}

		public DrawnAs DrawnAs
		{
			get;
			set;
		}

		public float Step
		{
			get;
			set;
		}

		public event ChangedValueEventHandler OnChanged;

		internal FloatWithRandom(float value = 0, float max = float.MaxValue, float min = float.MinValue, DrawnAs drawnas = Data.DrawnAs.CenterAndAmplitude, float step = 1.0f)
		{
			_max = max;
			_min = min;
			_value_center = value.Clipping(_max, _min);
			_value_max = value.Clipping(_max, _min);
			_value_min = value.Clipping(_max, _min);
			
			DrawnAs = drawnas;
			Step = step;
		}

		protected void CallChanged(object value, ChangedValueType type)
		{
			if (OnChanged != null)
			{
				OnChanged(this, new ChangedValueEventArgs(value, type));
			}
		}

		public float GetCenter()
		{
			return _value_center;
		}

		public void SetCenter(float value, bool isCombined = false)
		{
			value = Math.Min(value, _max);
			value = Math.Max(value, _min);

			if (value == _value_center) return;

			float a = Amplitude;

			float old_center = _value_center;
			float new_center = value;
			float old_max = _value_max;
			float new_max = Math.Min(value + a, _max );
			float old_min = _value_min;
			float new_min = Math.Max(value - a, _min);

			var cmd = new Command.DelegateCommand(
				() =>
				{
					_value_center = new_center;
					_value_max = new_max;
					_value_min = new_min;

					CallChanged(new_center, ChangedValueType.Execute);
				},
				() =>
				{
					_value_center = old_center;
					_value_max = old_max;
					_value_min = old_min;

					CallChanged(old_center, ChangedValueType.Unexecute);
				},
				this,
				isCombined);

			Command.CommandManager.Execute(cmd);
		}

		internal void SetCenterDirectly(float value)
		{
			_value_center = value;
		}

		public float GetMax()
		{
			return _value_max;
		}

		public void SetMax(float value, bool isCombined = false)
		{
			value = Math.Min(value, _max);
			value = Math.Max(value, _min);

			if (value == _value_max) return;

			float old_max = _value_max;
			float new_max = value;
			float old_min = _value_min;
			float new_min = Math.Min(value, _value_min);
			float old_center = _value_center;
			float new_center = (new_max + new_min) / 2.0f;

			var cmd = new Command.DelegateCommand(
				() =>
				{
					_value_center = new_center;
					_value_max = new_max;
					_value_min = new_min;

					CallChanged(new_max, ChangedValueType.Execute);
				},
				() =>
				{
					_value_center = old_center;
					_value_max = old_max;
					_value_min = old_min;

					CallChanged(old_max, ChangedValueType.Unexecute);
				},
				this,
				isCombined);

			Command.CommandManager.Execute(cmd);
		}

		internal void SetMaxDirectly(float value)
		{
			_value_max = value;
		}

		public float GetMin()
		{
			return _value_min;
		}

		public void SetMin(float value, bool isCombined = false)
		{
			value = Math.Min(value, _max);
			value = Math.Max(value, _min);

			if (value == _value_min) return;

			float old_max = _value_max;
			float new_max = Math.Max(value, _value_max);
			float old_min = _value_min;
			float new_min = value;
			float old_center = _value_center;
			float new_center = (new_max + new_min) / 2.0f;

			var cmd = new Command.DelegateCommand(
				() =>
				{
					_value_center = new_center;
					_value_max = new_max;
					_value_min = new_min;

					CallChanged(new_min, ChangedValueType.Execute);
				},
				() =>
				{
					_value_center = old_center;
					_value_max = old_max;
					_value_min = old_min;

					CallChanged(old_min, ChangedValueType.Unexecute);
				},
				this,
				isCombined);

			Command.CommandManager.Execute(cmd);
		}

		internal void SetMinDirectly(float value)
		{
			_value_min = value;
		}

		public float GetAmplitude()
		{
			return Math.Max(_value_max - _value_center, _value_center - _value_min);
		}

		public void SetAmplitude(float value, bool isCombined = false)
		{
			value = Math.Abs(value);

			float old_center = _value_center;
			float new_center = _value_center;
			float old_max = _value_max;
			float new_max = Math.Min(_value_center + value, _max);
			float old_min = _value_min;
			float new_min = Math.Max(_value_center - value, _min);

			if (new_max == old_max && new_min == old_min) return;

			var cmd = new Command.DelegateCommand(
				() =>
				{
					_value_center = new_center;
					_value_max = new_max;
					_value_min = new_min;

					CallChanged(new_min, ChangedValueType.Execute);
				},
				() =>
				{
					_value_center = old_center;
					_value_max = old_max;
					_value_min = old_min;

					CallChanged(old_min, ChangedValueType.Unexecute);
				},
				this,
				isCombined);

			Command.CommandManager.Execute(cmd);
		}
	}
}