using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JoyconLib;

namespace Htw.Cave.Joycons
{
	[Serializable]
	public enum JoyconAxisType
	{
		Button,
		StickX,
		StickY
	}

	/// <summary>
	/// Matches a Joy-Con axis with an Input axis.
	/// </summary>
	[Serializable]
	public class JoyconScheme
	{
		public string name = "";
		public JoyconAxisType axis;
		public Joycon.Button button;

		public JoyconScheme(string name, JoyconAxisType axis, Joycon.Button button = Joycon.Button.STICK)
		{
			this.name = name;
			this.axis = axis;
			this.button = button;
		}
	}

	/// <summary>
	/// Binds the provided Joy-Con axes to the Input axes.
	/// </summary>
	[CreateAssetMenu(fileName = "New Joycon Binding", menuName = "Htw.Cave/Joycon Binding", order = 40)]
	public class JoyconBinding : ScriptableObject
	{
		public const string Identifier = "Joycon ";

		private static Dictionary<string, JoyconAxis> bindings;

		public static void Bind(string name, JoyconAxis axis)
		{
			if(!string.IsNullOrEmpty(name) && axis != null)
				bindings.Add(name, axis);
		}

		public static bool IsLeftAxis(string axis)
		{
			return axis[axis.Length - 1] == 'L';
		}

		public static bool IsRightAxis(string axis)
		{
			return axis[axis.Length - 1] == 'R';
		}

		public static JoyconAxis ResolveAxis(string axis)
		{
			return bindings[axis];
		}

		public static string ResolveName(string axis, bool isLeft = true)
		{
			if(!axis.StartsWith(Identifier))
				axis = Identifier + axis;

			char t = axis[axis.Length - 1];

			if(t != 'L' && t != 'R')
				axis = isLeft ? axis + " L" : axis + " R";

			return axis;
		}

		public static string ResolveName(string axis, Joycon joycon)
		{
			return ResolveName(axis, joycon == null ? true : joycon.isLeft);
		}

		[SerializeField]
		private List<JoyconScheme> schemes;
		public List<JoyconScheme> Schemes
		{
			get => this.schemes;
			set => this.schemes = value;
		}

		public void Awake()
		{
			this.schemes = new  List<JoyconScheme>();

			AddDefaultSchemes();
		}

		public void Activate()
		{
			bindings = new Dictionary<string, JoyconAxis>();

			foreach(JoyconScheme scheme in this.schemes)
			{
				JoyconAxis axis = null;

				if(scheme.axis == JoyconAxisType.Button)
					axis = new JoyconButton(scheme.button);
				else
					axis = new JoyconStick(scheme.axis == JoyconAxisType.StickX);


				Bind(scheme.name, axis);
			}
		}

		public void Add(params JoyconScheme[] schemes)
		{
			foreach(JoyconScheme scheme in schemes)
			{
				if(scheme != null && !Exists(scheme))
					this.schemes.Add(scheme);
			}
		}

		public bool Exists(string name)
		{
			return this.schemes.Count(s => s.name == name) > 0;
		}

		public bool Exists(JoyconScheme scheme)
		{
			return Exists(scheme.name);
		}

		private void AddDefaultSchemes()
		{
			Add(JoyconDefaults.StickSchemes);
			Add(JoyconDefaults.ButtonSchemes);
		}
	}
}
