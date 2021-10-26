using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace ExplosionForce2D.PropertyAttributes {

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
	AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public class BoolConditionalHide : PropertyAttribute {

		public string ConditionalSourceField = "";
		public bool HideInInspector = false;
		public bool reverseCondition = false;

		public BoolConditionalHide(string conditionalSourceField) {
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = false;
		}

		public BoolConditionalHide(string conditionalSourceField, bool hideInInspector) {
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = hideInInspector;
		}

		public BoolConditionalHide(string conditionalSourceField, bool hideInInspector, bool reverseCondition) {
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = hideInInspector;
			this.reverseCondition = reverseCondition;
		}
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
		AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public class BoolConditionalHidePlusMessageBox : PropertyAttribute {

		public string ConditionalSourceField = "";
		public bool HideInInspector = false;
		public bool reverseCondition = false;
		public string message = "";
		public string messageBoxType = "";

		public BoolConditionalHidePlusMessageBox(string conditionalSourceField) {
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = false;
		}

		public BoolConditionalHidePlusMessageBox(string conditionalSourceField, bool hideInInspector) {
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = hideInInspector;
		}

		public BoolConditionalHidePlusMessageBox(string conditionalSourceField, bool hideInInspector, bool reverseCondition) {
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = hideInInspector;
			this.reverseCondition = reverseCondition;
		}

		public BoolConditionalHidePlusMessageBox(string conditionalSourceField, bool hideInInspector, bool reverseCondition, string _message, string _messageType) {
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = hideInInspector;
			this.reverseCondition = reverseCondition;
			message = _message;
			messageBoxType = _messageType;
		}
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
		AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public class BoolConditionWarrningBox : PropertyAttribute {

		public string ConditionalSourceField = "";
		public bool bothCondition = false;
		public string message = "";

		public BoolConditionWarrningBox(string _conditionalSourceField, string _message, bool _bothCondition) {
			this.ConditionalSourceField = _conditionalSourceField;
			this.message = _message;
			this.bothCondition = _bothCondition;
		}
	}

	public class TagDraw : PropertyAttribute {
		public bool def = false; // default
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
		AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public class StringConditionalHide : PropertyAttribute {

		public string ConditionalSourceField = "";
		public bool HideInInspector = false;
		public bool reverseCondition = false;
		public string ConditionalString = "";

		public StringConditionalHide(string conditionalSourceField) {
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = false;
		}

		public StringConditionalHide(string conditionalSourceField, bool hideInInspector)
		{
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = hideInInspector;
		}

		public StringConditionalHide(string conditionalSourceField, bool hideInInspector, bool reverseCondition, string conditionalString)
		{
			this.ConditionalSourceField = conditionalSourceField;
			this.HideInInspector = hideInInspector;
			this.reverseCondition = reverseCondition;
			this.ConditionalString = conditionalString;
		}
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
		AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	public class StringListPopup : PropertyAttribute {

		public string[] list;
		public string conditionalSourceField;
		public string conditionalString;
		public bool hideInInspector;
		public bool reverseCondition;


		public StringListPopup(string [] _list) {
			this.list = _list;
			conditionalSourceField = "";
		}

		public StringListPopup(string _conditionalSourceField, string _conditionalString, bool _hideInInspector = true, bool _reverseCondition = false, params string [] _list) {
			this.list = _list;
			this.conditionalSourceField = _conditionalSourceField;
			this.conditionalString = _conditionalString;
			this.hideInInspector = _hideInInspector;
			this.reverseCondition = _reverseCondition;
		}
	}



	public class DrawRectWithColor : PropertyAttribute {

		public float lineHeight;
		public Color lineColor = Color.red;

		public DrawRectWithColor(float _lineHeight, float r, float g, float b) {
			this.lineHeight = _lineHeight;
			this.lineColor = new Color(r, g, b);
		}

		public DrawRectWithColor(float _lineHeight, float r, float g, float b, float a) {
			this.lineHeight = _lineHeight;
			this.lineColor = new Color(r, g, b, a);
		}
	}




}