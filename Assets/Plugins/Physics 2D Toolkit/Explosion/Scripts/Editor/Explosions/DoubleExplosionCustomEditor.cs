using UnityEngine;
using UnityEditor;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(DoubleExplosion))]
public class DoubleExplosionCustomEditor : ExplosionUniversalEditor {

	DoubleExplosion scriptRef;

	void OnEnable () {
		scriptRef = (DoubleExplosion)target;
		objectTransform = scriptRef.transform;
		useGamebjectDepthPreviousValue = scriptRef.useGamebjectDepth;
		SetUpRadiusText ();
	}

	void OnSceneGUI () {
		Handles.color = ExplosionForce2DPreferences.firstRadiusColor;
		Handles.DrawWireDisc (objectTransform.position + scriptRef.firstExplosionOffset, Vector3.forward, scriptRef.firstExplosionRadius);

		Handles.color = ExplosionForce2DPreferences.secondRadiusColor;
		Handles.DrawWireDisc (objectTransform.position + scriptRef.secondExplosionOffset, Vector3.forward, scriptRef.secondExplosionRadius);

		ScaleRadiusHandler ();
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI ();
		if (useGamebjectDepthPreviousValue) {
			scriptRef.minDepth = objectTransform.position.z;
			scriptRef.maxDepth = objectTransform.position.z;
		}
		if (scriptRef.useGamebjectDepth != useGamebjectDepthPreviousValue) {
			if (!useGamebjectDepthPreviousValue) {
				useGamebjectDepthPreviousValue = true;
				scriptRef.minDepth = objectTransform.position.z;
				scriptRef.maxDepth = objectTransform.position.z;
			} else {
				useGamebjectDepthPreviousValue = false;
				scriptRef.minDepth = -Mathf.Infinity;
				scriptRef.maxDepth = Mathf.Infinity;
			}
		}
	}


	private float capturedRadius_1;
	private Vector3 capturedHandlePosition_1;

	private float capturedRadius_2;
	private Vector3 capturedHandlePosition_2;

	
	private void ScaleRadiusHandler () {
		Handles.color = Color.white;


		var centerPosition_1 = scriptRef.transform.position + scriptRef.firstExplosionOffset;
		var centerPosition_2 = scriptRef.transform.position + scriptRef.secondExplosionOffset;
	
		var handlesPos = new [] {
			new Vector3(centerPosition_1.x + scriptRef.firstExplosionRadius, centerPosition_1.y, 0f),
			new Vector3(centerPosition_2.x + scriptRef.secondExplosionRadius, centerPosition_2.y, 0f)
		};
		if (Event.current.type == EventType.MouseDown && Event.current.button == 0 || Event.current.type == EventType.MouseUp && Event.current.button == 0) {
			capturedRadius_1 = scriptRef.firstExplosionRadius;
			capturedHandlePosition_1 = handlesPos [0];

			capturedRadius_2 = scriptRef.secondExplosionRadius;
			capturedHandlePosition_2 = handlesPos [1];
		}
	
		float size_1 = HandleUtility.GetHandleSize(scriptRef.transform.position + scriptRef.firstExplosionOffset) * 0.1f;
		float size_2 = HandleUtility.GetHandleSize(scriptRef.transform.position + scriptRef.secondExplosionOffset) * 0.1f;
	
		EditorGUI.BeginChangeCheck ();
		Handles.color = Color.white;
		Vector3 hPos0 = Handles.FreeMoveHandle(handlesPos[0], Quaternion.identity, size_1, Vector3.one * 0.5f, Handles.CubeHandleCap);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(scriptRef, "Changed Scale");
			float offset = (hPos0.x - capturedHandlePosition_1.x);
			if (capturedRadius_1 + offset <= 0f)
				scriptRef.firstExplosionRadius = 0f;
			else
				scriptRef.firstExplosionRadius = capturedRadius_1 + offset;
		}

		EditorGUI.BeginChangeCheck ();
		Handles.color = Color.white;
		Vector3 hPos1 = Handles.FreeMoveHandle(handlesPos[1], Quaternion.identity, size_2, Vector3.one * 0.5f, Handles.CubeHandleCap);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(scriptRef, "Changed Scale");
			float offset = (hPos1.x - capturedHandlePosition_2.x);
			if (capturedRadius_2 + offset <= 0f)
				scriptRef.secondExplosionRadius = 0f;
			else
				scriptRef.secondExplosionRadius = capturedRadius_2 + offset;
		}
	
		if (ExplosionForce2DPreferences.showTextAboveHandles) {
			radiusText = Math.Round (scriptRef.firstExplosionRadius, 2).ToString ();
			Handles.Label (handlesPos [0], radiusText, radiusTextStyle);

			radiusText = Math.Round (scriptRef.secondExplosionRadius, 2).ToString ();
			Handles.Label (handlesPos [1], radiusText, radiusTextStyle);
		}
	}
}
