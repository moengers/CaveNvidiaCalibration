using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JoyconLib;

namespace Htw.Cave.Joycons
{
	public class InputManagerEntry
	{
		public enum Kind { KeyOrButton, Mouse, Axis }
		public enum Axis { X, Y, Third, Fourth, Fifth, Sixth, Seventh, Eigth }
		public enum Joy { All, First, Second }

		public static InputManagerEntry Empty(string name) => new InputManagerEntry { name = name };
		public static InputManagerEntry Button(string name) => new InputManagerEntry { name = name, kind = Kind.KeyOrButton, deadZone = 0.001f };
		public static InputManagerEntry Mouse(string name, Axis axis) => new InputManagerEntry { name = name, kind = Kind.Mouse, sensitivity = 0.1f, axis = axis };

		public string name = "";
		public string desc = "";
		public string btnNegative = "";
		public string btnPositive = "";
		public string altBtnNegative = "";
		public string altBtnPositive = "";
		public float gravity = 0.0f;
		public float deadZone = 0.0f;
		public float sensitivity = 0.0f;
		public bool snap = false;
		public bool invert = false;
		public Kind kind = Kind.Axis;
		public Axis axis = Axis.X;
		public Joy joystick = Joy.All;

		public InputManagerEntry WithBtn(string positive)
		{
			this.btnPositive = positive;
			this.gravity = 1000f;
			this.sensitivity = 1000f;

			return this;
		}

		public InputManagerEntry WithBtn(string positive, string negative)
		{
			this.btnNegative = negative;
			this.btnPositive = positive;
			this.gravity = 3f;
			this.sensitivity = 3f;
			this.snap = true;

			return this;
		}
	}

	public static class JoyconDefaults
	{
		public static InputManagerEntry[] StickEntries => new InputManagerEntry[] {
			InputManagerEntry.Button("Joycon Horizontal L").WithBtn("d", "a"),
			InputManagerEntry.Mouse("Joycon Horizontal R", InputManagerEntry.Axis.X),
			InputManagerEntry.Button("Joycon Vertical L").WithBtn("w", "s"),
			InputManagerEntry.Mouse("Joycon Vertical R", InputManagerEntry.Axis.Y),
			InputManagerEntry.Button("Joycon Stick L").WithBtn("q"),
			InputManagerEntry.Button("Joycon Stick R").WithBtn("e")
		};

		public static JoyconScheme[] StickSchemes => new JoyconScheme[] {
			new JoyconScheme("Horizontal L", JoyconAxisType.StickX),
			new JoyconScheme("Horizontal R", JoyconAxisType.StickX),
			new JoyconScheme("Vertical L", JoyconAxisType.StickY),
			new JoyconScheme("Vertical R", JoyconAxisType.StickY),
			new JoyconScheme("Stick L", JoyconAxisType.Button, Joycon.Button.STICK),
			new JoyconScheme("Stick R", JoyconAxisType.Button, Joycon.Button.STICK)
		};

		public static InputManagerEntry[] ButtonEntries => new InputManagerEntry[] {
			InputManagerEntry.Button("Joycon Trigger L").WithBtn("left ctrl"),
			InputManagerEntry.Button("Joycon Trigger R").WithBtn("mouse 0"),
			InputManagerEntry.Button("Joycon Bumper L").WithBtn("left shift"),
			InputManagerEntry.Button("Joycon Bumper R").WithBtn("mouse 1")
		};

		public static JoyconScheme[] ButtonSchemes => new JoyconScheme[] {
			new JoyconScheme("Trigger L", JoyconAxisType.Button, Joycon.Button.SHOULDER_2),
			new JoyconScheme("Trigger R", JoyconAxisType.Button, Joycon.Button.SHOULDER_2),
			new JoyconScheme("Bumper L", JoyconAxisType.Button, Joycon.Button.SHOULDER_1),
			new JoyconScheme("Bumper R", JoyconAxisType.Button, Joycon.Button.SHOULDER_1)
		};

		public static InputManagerEntry[] DpadEntries => new InputManagerEntry[] {
			InputManagerEntry.Button("Joycon Dpad Left L").WithBtn("f"),
			InputManagerEntry.Button("Joycon Dpad Right L").WithBtn("h"),
			InputManagerEntry.Button("Joycon Dpad Up L").WithBtn("t"),
			InputManagerEntry.Button("Joycon Dpad Down L").WithBtn("g"),
			InputManagerEntry.Button("Joycon Dpad Left R").WithBtn("j"),
			InputManagerEntry.Button("Joycon Dpad Right R").WithBtn("l"),
			InputManagerEntry.Button("Joycon Dpad Up R").WithBtn("i"),
			InputManagerEntry.Button("Joycon Dpad Down R").WithBtn("k")
		};

		public static JoyconScheme[] DpadSchemes => new JoyconScheme[] {
			new JoyconScheme("Dpad Left L", JoyconAxisType.Button, Joycon.Button.DPAD_LEFT),
			new JoyconScheme("Dpad Right L", JoyconAxisType.Button, Joycon.Button.DPAD_RIGHT),
			new JoyconScheme("Dpad Up L", JoyconAxisType.Button, Joycon.Button.DPAD_UP),
			new JoyconScheme("Dpad Down L", JoyconAxisType.Button, Joycon.Button.DPAD_DOWN),
			new JoyconScheme("Dpad Left R", JoyconAxisType.Button, Joycon.Button.DPAD_LEFT),
			new JoyconScheme("Dpad Right R", JoyconAxisType.Button, Joycon.Button.DPAD_RIGHT),
			new JoyconScheme("Dpad Up R", JoyconAxisType.Button, Joycon.Button.DPAD_UP),
			new JoyconScheme("Dpad Down R", JoyconAxisType.Button, Joycon.Button.DPAD_DOWN),
		};
	}
}
