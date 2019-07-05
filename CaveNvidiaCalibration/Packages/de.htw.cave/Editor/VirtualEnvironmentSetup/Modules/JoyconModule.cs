using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Htw.Cave.Joycons;

namespace Htw.Cave.VirtualEnvironmentSetup.Modules
{
	public class JoyconModule : Module
	{
		public override string DisplayName { get => "Nintendo Joy-Cons"; }
		public override bool IsSkippable { get => true; }

		private Texture2D background;
		private bool dpadBindings;

		public JoyconModule()
		{
			this.background = new Texture2D(1, 1, TextureFormat.RGBA32, false);
			this.dpadBindings = false;

			this.background.SetPixel(0, 0, new Color(0.89f, 0f, 0.06f));
			this.background.Apply();
		}

		public override void OnHeaderGUI()
		{
			WindowDesignUtility.DrawTitle("Nintendo Joy-Cons", Color.white, this.background);
		}

		public override void OnGUI(ModuleMaker maker)
		{
			WindowDesignUtility.SectionLabel("Bindings");
			this.dpadBindings = EditorGUILayout.Toggle("Dpad", this.dpadBindings);
		}

		public override void Build(ModuleMaker maker, GameObject root, GameObject head)
		{
			GameObject joycon = new GameObject("Joycon Controller");
			joycon.transform.parent = root.transform;
			joycon.transform.localPosition = Vector3.zero;

			JoyconController controller = joycon.AddComponent<JoyconController>();

			JoyconEditorInternals.CreateAxis(JoyconDefaults.StickEntries);
			JoyconEditorInternals.CreateAxis(JoyconDefaults.ButtonEntries);

			string assetPath = ModuleMaker.AssetPath + "/Joycon Binding.asset";
			JoyconBinding asset = AssetDatabase.LoadAssetAtPath<JoyconBinding>(assetPath);

			if(asset == null)
			{
				asset = ScriptableObject.CreateInstance<JoyconBinding>();

				if(this.dpadBindings)
				{
					JoyconEditorInternals.CreateAxis(JoyconDefaults.DpadEntries);
					asset.Add(JoyconDefaults.DpadSchemes);
				}

				AssetDatabase.CreateAsset(asset, assetPath);
			}

			controller.Binding = asset;
		}
	}
}
