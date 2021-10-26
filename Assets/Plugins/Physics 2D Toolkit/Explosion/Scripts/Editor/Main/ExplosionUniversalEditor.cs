using UnityEngine;
using UnityEditor;

public class ExplosionUniversalEditor : Editor {

	protected bool useGamebjectDepthPreviousValue = false;
	protected Transform objectTransform;

	protected GUIStyle radiusTextStyle;
	protected string radiusText = "";

	protected void SetUpRadiusText () {
		radiusTextStyle = new GUIStyle ();
		radiusTextStyle.fontSize = 11;
		radiusTextStyle.fontStyle = FontStyle.Bold;
		radiusTextStyle.normal.textColor = ExplosionForce2DPreferences.aboveHandleTextColor;
		radiusTextStyle.alignment = TextAnchor.MiddleCenter;
		radiusTextStyle.contentOffset = new Vector2 (13f,-18f);
	}
}
