using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelDataSO))]
public class LevelDataSOEditor : Editor
{
    private const float PreviewSize = 300f;
    private const float PointPickRadius = 10f;

    private int placementMode;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelDataSO levelData = (LevelDataSO)target;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Placement Editor", EditorStyles.boldLabel);

        placementMode = GUILayout.Toolbar(placementMode, new string[] { "Apple", "Knife" });
        EditorGUILayout.HelpBox("Click vào trong vòng tròn để thêm. Click gần 1 điểm đã có để xoá nó.", MessageType.Info);

        Rect previewRect = GUILayoutUtility.GetRect(PreviewSize, PreviewSize);
        Vector2 center = previewRect.center;
        float radiusPixels = PreviewSize * 0.5f - 10f;

        Handles.BeginGUI();
        Handles.color = Color.gray;
        Handles.DrawWireDisc(center, Vector3.forward, radiusPixels);

        foreach (ApplePlacement placement in levelData.applePlacements)
        {
            Vector2 pointPosition = GetPointOnCircle(center, radiusPixels, levelData.logRadius, placement.angleDegrees, placement.radius);
            Handles.color = Color.red;
            Handles.DrawSolidDisc(pointPosition, Vector3.forward, 6f);
        }

        foreach (KnifePlacement placement in levelData.obstacleKnifePlacements)
        {
            Vector2 pointPosition = GetPointOnCircle(center, radiusPixels, levelData.logRadius, placement.angleDegrees, placement.radius);
            Handles.color = Color.cyan;
            Handles.DrawSolidDisc(pointPosition, Vector3.forward, 6f);
        }
        Handles.EndGUI();

        HandleMouseInput(previewRect, center, radiusPixels, levelData);

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear Apples"))
        {
            Undo.RecordObject(levelData, "Clear Apples");
            levelData.applePlacements.Clear();
            EditorUtility.SetDirty(levelData);
        }
        if (GUILayout.Button("Clear Knives"))
        {
            Undo.RecordObject(levelData, "Clear Knives");
            levelData.obstacleKnifePlacements.Clear();
            EditorUtility.SetDirty(levelData);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Total Apples: " + levelData.applePlacements.Count);
        EditorGUILayout.LabelField("Total Obstacle Knives: " + levelData.obstacleKnifePlacements.Count);
    }

    private Vector2 GetPointOnCircle(Vector2 center, float radiusPixels, float logRadius, float angleDegrees, float pointRadius)
    {
        float radians = angleDegrees * Mathf.Deg2Rad;
        float normalizedRadius = logRadius > 0f ? pointRadius / logRadius : 0f;
        float pixelRadius = Mathf.Clamp01(normalizedRadius) * radiusPixels;
        return center + new Vector2(Mathf.Cos(radians), -Mathf.Sin(radians)) * pixelRadius;
    }

    private void HandleMouseInput(Rect previewRect, Vector2 center, float radiusPixels, LevelDataSO levelData)
    {
        Event currentEvent = Event.current;
        if (currentEvent.type != EventType.MouseDown) return;
        if (!previewRect.Contains(currentEvent.mousePosition)) return;

        Vector2 clickOffset = currentEvent.mousePosition - center;
        float clickDistance = clickOffset.magnitude;
        if (clickDistance > radiusPixels) return;

        if (placementMode == 0) HandleApplePlacement(currentEvent.mousePosition, clickOffset, clickDistance, center, radiusPixels, levelData);
        else HandleKnifePlacement(currentEvent.mousePosition, clickOffset, clickDistance, center, radiusPixels, levelData);

        EditorUtility.SetDirty(levelData);
        currentEvent.Use();
        Repaint();
    }

    private void HandleApplePlacement(Vector2 mousePosition, Vector2 clickOffset, float clickDistance, Vector2 center, float radiusPixels, LevelDataSO levelData)
    {
        ApplePlacement existing = null;
        foreach (ApplePlacement placement in levelData.applePlacements)
        {
            Vector2 pointPosition = GetPointOnCircle(center, radiusPixels, levelData.logRadius, placement.angleDegrees, placement.radius);
            if (Vector2.Distance(pointPosition, mousePosition) < PointPickRadius) existing = placement;
        }

        if (existing != null)
        {
            Undo.RecordObject(levelData, "Remove Apple");
            levelData.applePlacements.Remove(existing);
            return;
        }

        float angle = Mathf.Atan2(-clickOffset.y, clickOffset.x) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;
        float radius = clickDistance / radiusPixels * levelData.logRadius;
        ApplePlacement newPlacement = new ApplePlacement();
        newPlacement.angleDegrees = angle;
        newPlacement.radius = radius;
        Undo.RecordObject(levelData, "Add Apple");
        levelData.applePlacements.Add(newPlacement);
    }

    private void HandleKnifePlacement(Vector2 mousePosition, Vector2 clickOffset, float clickDistance, Vector2 center, float radiusPixels, LevelDataSO levelData)
    {
        KnifePlacement existing = null;
        foreach (KnifePlacement placement in levelData.obstacleKnifePlacements)
        {
            Vector2 pointPosition = GetPointOnCircle(center, radiusPixels, levelData.logRadius, placement.angleDegrees, placement.radius);
            if (Vector2.Distance(pointPosition, mousePosition) < PointPickRadius) existing = placement;
        }

        if (existing != null)
        {
            Undo.RecordObject(levelData, "Remove Knife");
            levelData.obstacleKnifePlacements.Remove(existing);
            return;
        }

        float angle = Mathf.Atan2(-clickOffset.y, clickOffset.x) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;
        KnifePlacement newPlacement = new KnifePlacement();
        newPlacement.angleDegrees = angle;
        newPlacement.radius = levelData.logRadius;
        Undo.RecordObject(levelData, "Add Knife");
        levelData.obstacleKnifePlacements.Add(newPlacement);
    }
}