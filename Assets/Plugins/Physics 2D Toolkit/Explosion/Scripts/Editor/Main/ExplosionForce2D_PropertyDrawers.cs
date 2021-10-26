using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace ExplosionForce2D.PropertyAttributes {
	
	[CustomPropertyDrawer(typeof(BoolConditionalHide))]
	public class BoolConditionalHidePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			BoolConditionalHide condHAtt = (BoolConditionalHide)attribute;
			bool enabled = GetConditionalHideAttributeResult(condHAtt, property);
			bool wasEnabled = GUI.enabled;
			GUI.enabled = enabled;
			if (!condHAtt.HideInInspector || enabled)
				EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = wasEnabled;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			BoolConditionalHide condHAtt = (BoolConditionalHide)attribute;
			bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

			if (!condHAtt.HideInInspector || enabled)
				return EditorGUI.GetPropertyHeight(property, label);
			else
				return -EditorGUIUtility.standardVerticalSpacing;
		}

		private bool GetConditionalHideAttributeResult(BoolConditionalHide condHAtt, SerializedProperty property) {
			bool enabled = true;
			string propertyPath = property.propertyPath;
			string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField);
			SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

			if (sourcePropertyValue != null) {
				if (condHAtt.reverseCondition) {
					if (sourcePropertyValue.boolValue == false)
						enabled = true;
					else
						enabled = false;
				} else {
					if (sourcePropertyValue.boolValue == false)
						enabled = false;
					else
						enabled = true;
				}
			} else {
				Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
			}
			return enabled;
		}
	}

	[CustomPropertyDrawer(typeof(TagDraw))]
	public class TagDrawPropertyDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.String) {
				EditorGUI.BeginProperty (position, label, property);

				var att = this.attribute as TagDraw;

				if (att.def) {
					property.stringValue = EditorGUI.TagField (position, label, property.stringValue);
				} else {
					List<string> tagList = new List<string> ();
					tagList.AddRange (UnityEditorInternal.InternalEditorUtility.tags);
					string propertyString = property.stringValue;
					int index = -1;
					if (propertyString == "") {
						index = 0;
					} else {
						for (int i = 1; i < tagList.Count; i++) {
							if (tagList [i] == propertyString) {
								index = i;
								break;
							}
						}
					}
					index = EditorGUI.Popup (position, label.text, index, tagList.ToArray ());
					if (index == 0)
						property.stringValue = "";
					else if (index >= 1)
						property.stringValue = tagList [index];
					else
						property.stringValue = "";
				}
				EditorGUI.EndProperty ();
			} else {
				EditorGUI.PropertyField (position, property, label);
			}
		}
	}

	[CustomPropertyDrawer(typeof(StringListPopup))]
	public class StringListPopupDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			StringListPopup condHAtt = (StringListPopup)attribute;
			bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

			bool wasEnabled = GUI.enabled;
			GUI.enabled = enabled;
			if (!condHAtt.hideInInspector || enabled) {
				var list = condHAtt.list;
				int index = Mathf.Max (0, Array.IndexOf (list, property.stringValue));
				index = EditorGUI.Popup (position, property.displayName, index, list);
				property.stringValue = list [index];
			}
			GUI.enabled = wasEnabled;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			StringListPopup condHAtt = (StringListPopup)attribute;
			bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

			if (!condHAtt.hideInInspector || enabled)
				return EditorGUI.GetPropertyHeight(property, label);
			else
				return -EditorGUIUtility.standardVerticalSpacing;
		}

		private bool GetConditionalHideAttributeResult(StringListPopup condHAtt, SerializedProperty property) {
			bool enabled = true;
			string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
			string conditionPath = propertyPath.Replace(property.name, condHAtt.conditionalSourceField); //changes the path to the conditionalsource property path
			SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

			if (sourcePropertyValue != null) {
				if (condHAtt.reverseCondition) {
					if (sourcePropertyValue.stringValue != condHAtt.conditionalString)
						enabled = true;
					else
						enabled = false;
				} else {
					if (sourcePropertyValue.stringValue != condHAtt.conditionalString)
						enabled = false;
					else
						enabled = true;
				}
			} else {
				if(condHAtt.conditionalSourceField != "")
					Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.conditionalSourceField);
			}
			return enabled;
		}
	}

	[CustomPropertyDrawer(typeof(StringConditionalHide))]
	public class StringConditionalHideDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			StringConditionalHide condHAtt = (StringConditionalHide)attribute;
			bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

			bool wasEnabled = GUI.enabled;
			GUI.enabled = enabled;
			if (!condHAtt.HideInInspector || enabled)
				EditorGUI.PropertyField(position, property, label, true);

			GUI.enabled = wasEnabled;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			StringConditionalHide condHAtt = (StringConditionalHide)attribute;
			bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

			if (!condHAtt.HideInInspector || enabled)
				return EditorGUI.GetPropertyHeight(property, label);
			else
				return -EditorGUIUtility.standardVerticalSpacing;
		}

		private bool GetConditionalHideAttributeResult(StringConditionalHide condHAtt, SerializedProperty property) {
			bool enabled = true;
			string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to
			string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField); //changes the path to the conditionalsource property path
			SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

			if (sourcePropertyValue != null) {
				if (condHAtt.reverseCondition) {
					if (sourcePropertyValue.stringValue != condHAtt.ConditionalString)
						enabled = true;
					else
						enabled = false;
				} else {
					if (sourcePropertyValue.stringValue != condHAtt.ConditionalString)
						enabled = false;
					else
						enabled = true;
				}
			} else {
				Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
			}
			return enabled;
		}
	}


	[CustomPropertyDrawer(typeof(BoolConditionWarrningBox))]
	public class BoolConditionWarrningBoxDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			BoolConditionWarrningBox condHAtt = (BoolConditionWarrningBox)attribute;
			bool showWarrning = GetConditionalHideAttributeResult(condHAtt, property);
				
			EditorGUI.PropertyField(position, property, label, true);
			if (showWarrning)
				EditorGUILayout.HelpBox(condHAtt.message, MessageType.Warning);
		}

		private bool GetConditionalHideAttributeResult(BoolConditionWarrningBox condHAtt, SerializedProperty property) {
			bool showWarrning = true;
			string propertyPath = property.propertyPath;
			string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField);
			SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

			if (sourcePropertyValue != null) {
				if (sourcePropertyValue.boolValue == property.boolValue) {
					if (condHAtt.bothCondition == true && sourcePropertyValue.boolValue == true) {
						showWarrning = true;
					} else if (condHAtt.bothCondition == false && sourcePropertyValue.boolValue == false) {
						showWarrning = true;
					} else {
						showWarrning = false;
					}
				} else {
					showWarrning = false;
				}
			} else {
				Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
			}
			return showWarrning;
		}
	}

	[CustomPropertyDrawer(typeof(BoolConditionalHidePlusMessageBox))]
	public class BoolConditionalHidePlusMessageBoxPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			BoolConditionalHidePlusMessageBox condHAtt = (BoolConditionalHidePlusMessageBox)attribute;
			bool enabled = GetConditionalHideAttributeResult(condHAtt, property);
			bool wasEnabled = GUI.enabled;
			GUI.enabled = enabled;
			if (!condHAtt.HideInInspector || enabled) {
				EditorGUI.PropertyField (position, property, label, true);
				MessageType msgType = MessageType.None;
				if (condHAtt.messageBoxType == "info")
					msgType = MessageType.Info;
				else if (condHAtt.messageBoxType == "warrning")
					msgType = MessageType.Warning;
				else if (condHAtt.messageBoxType == "error")
					msgType = MessageType.Error;
				EditorGUILayout.HelpBox(condHAtt.message, msgType);
			}

			GUI.enabled = wasEnabled;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			BoolConditionalHidePlusMessageBox condHAtt = (BoolConditionalHidePlusMessageBox)attribute;
			bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

			if (!condHAtt.HideInInspector || enabled)
				return EditorGUI.GetPropertyHeight(property, label);
			else
				return -EditorGUIUtility.standardVerticalSpacing;
		}

		private bool GetConditionalHideAttributeResult(BoolConditionalHidePlusMessageBox condHAtt, SerializedProperty property) {
			bool enabled = true;
			string propertyPath = property.propertyPath;
			string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField);
			SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

			if (sourcePropertyValue != null) {
				if (condHAtt.reverseCondition) {
					if (sourcePropertyValue.boolValue == false)
						enabled = true;
					else
						enabled = false;
				} else {
					if (sourcePropertyValue.boolValue == false)
						enabled = false;
					else
						enabled = true;
				}
			} else {
				Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + condHAtt.ConditionalSourceField);
			}
			return enabled;
		}
	}

	[CustomPropertyDrawer(typeof(DrawRectWithColor))]
	public class DrawRectWithColorDrawer : DecoratorDrawer {

		DrawRectWithColor drawRectWithColor {
			get { return ((DrawRectWithColor)attribute); }
		}

		public override void OnGUI(Rect r) {
			r.height = drawRectWithColor.lineHeight;
			EditorGUI.DrawRect (r,drawRectWithColor.lineColor);
		}
	}
}

