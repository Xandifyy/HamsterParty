using UnityEngine;
using UnityEditor;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(SingleExplosion))]
public class SingleExplosionCustomEditor : ExplosionUniversalEditor {

	SingleExplosion scriptRef;

	void OnEnable () {
		scriptRef = (SingleExplosion)target;
		objectTransform = scriptRef.transform;
		useGamebjectDepthPreviousValue = scriptRef.useGamebjectDepth;
		SetUpRadiusText ();
	}

	void OnSceneGUI () {
		Handles.color = ExplosionForce2DPreferences.firstRadiusColor;
		Handles.DrawWireDisc (objectTransform.position + scriptRef.explosionOffset, Vector3.forward,scriptRef.explosionRadius);
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

	private float capturedRadius;
	private Vector3 capturedHandlePosition;

	private void ScaleRadiusHandler () {
		Handles.color = Color.white;

		var centerPosition = scriptRef.transform.position + scriptRef.explosionOffset;

		var handlesPos = new [] {
			new Vector3(centerPosition.x + scriptRef.explosionRadius, centerPosition.y, 0f)
		};
		if (Event.current.type == EventType.MouseDown && Event.current.button == 0 || Event.current.type == EventType.MouseUp && Event.current.button == 0) {
			capturedRadius = scriptRef.explosionRadius;
			capturedHandlePosition = handlesPos [0];
		}

		float size = HandleUtility.GetHandleSize(scriptRef.transform.position + scriptRef.explosionOffset) * 0.1f;

		EditorGUI.BeginChangeCheck ();
		Handles.color = Color.white;
		Vector3 hPos0 = Handles.FreeMoveHandle(handlesPos[0], Quaternion.identity, size, Vector3.one * 0.5f, Handles.CubeHandleCap);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(scriptRef, "Changed Scale");
			float offset = (hPos0.x - capturedHandlePosition.x);
			if (capturedRadius + offset <= 0f)
				scriptRef.explosionRadius = 0f;
			else
				scriptRef.explosionRadius = capturedRadius + offset;
		}


		if (ExplosionForce2DPreferences.showTextAboveHandles) {
			radiusText = Math.Round (scriptRef.explosionRadius, 2).ToString ();
			Handles.Label (handlesPos [0], radiusText, radiusTextStyle);
		}
	}

}

