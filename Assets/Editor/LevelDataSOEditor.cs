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

        float logRadius = LogController.GetPrefabSurfaceRadius(levelData.logPrefab);
        if (levelData.logPrefab == null)
        {
            EditorGUILayout.HelpBox("Gán logPrefab trước để xem preview vị trí.", MessageType.Warning);
            return;
        }
        if (logRadius <= 0f)
        {
            EditorGUILayout.HelpBox("Log prefab cần có CircleCollider2D với radius > 0.", MessageType.Warning);
            return;
        }

        placementMode = GUILayout.Toolbar(placementMode, new string[] { "Apple", "Knife" });
        EditorGUILayout.HelpBox("Click vào trong vòng tròn để thêm. Click gần 1 điểm đã có để xoá nó. Bán kính thớt tự đọc từ logPrefab, không cần nhập tay.", MessageType.Info);
        EditorGUILayout.LabelField("Detected Log Surface Radius: " + logRadius.ToString("F3"));

        Rect previewRect = GUILayoutUtility.GetRect(PreviewSize, PreviewSize);
        Vector2 center = previewRect.center;
        float radiusPixels = PreviewSize * 0.5f - 10f;
        float appleSpawnRadius = logRadius + levelData.appleOffsetFromSurface;

        Handles.BeginGUI();
        Handles.color = Color.gray;
        Handles.DrawWireDisc(center, Vector3.forward, radiusPixels);

        foreach (ApplePlacement placement in levelData.applePlacements)
        {
            Vector2 pointPosition = GetPointOnCircle(center, radiusPixels, logRadius, placement.angleDegrees, appleSpawnRadius);
            Handles.color = Color.red;
            Handles.DrawSolidDisc(pointPosition, Vector3.forward, 6f);
        }

        foreach (KnifePlacement placement in levelData.obstacleKnifePlacements)
        {
            Vector2 pointPosition = GetPointOnCircle(center, radiusPixels, logRadius, placement.angleDegrees, logRadius);
            Handles.color = Color.cyan;
            Handles.DrawSolidDisc(pointPosition, Vector3.forward, 6f);
        }
        Handles.EndGUI();

        HandleMouseInput(previewRect, center, radiusPixels, logRadius, levelData);

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

    private float GetPixelRadius(float radiusPixels, float logRadius, float pointRadius)
    {
        float normalizedRadius = logRadius > 0f ? pointRadius / logRadius : 0f;
        return Mathf.Clamp01(normalizedRadius) * radiusPixels;
    }

    private Vector2 GetPointOnCircle(Vector2 center, float radiusPixels, float logRadius, float angleDegrees, float pointRadius)
    {
        float radians = angleDegrees * Mathf.Deg2Rad;
        float pixelRadius = GetPixelRadius(radiusPixels, logRadius, pointRadius);
        return center + new Vector2(Mathf.Cos(radians), -Mathf.Sin(radians)) * pixelRadius;
    }

    private void HandleMouseInput(Rect previewRect, Vector2 center, float radiusPixels, float logRadius, LevelDataSO levelData)
    {
        Event currentEvent = Event.current;
        if (currentEvent.type != EventType.MouseDown) return;
        if (!previewRect.Contains(currentEvent.mousePosition)) return;

        Vector2 clickOffset = currentEvent.mousePosition - center;
        float clickDistance = clickOffset.magnitude;
        if (clickDistance > radiusPixels) return;

        if (placementMode == 0) HandleApplePlacement(currentEvent.mousePosition, clickOffset, center, radiusPixels, logRadius, levelData);
        else HandleKnifePlacement(currentEvent.mousePosition, clickOffset, center, radiusPixels, logRadius, levelData);

        EditorUtility.SetDirty(levelData);
        currentEvent.Use();
        Repaint();
    }

    private void HandleApplePlacement(Vector2 mousePosition, Vector2 clickOffset, Vector2 center, float radiusPixels, float logRadius, LevelDataSO levelData)
    {
        float appleSpawnRadius = logRadius + levelData.appleOffsetFromSurface;
        ApplePlacement existing = null;
        foreach (ApplePlacement placement in levelData.applePlacements)
        {
            Vector2 pointPosition = GetPointOnCircle(center, radiusPixels, logRadius, placement.angleDegrees, appleSpawnRadius);
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
        ApplePlacement newPlacement = new ApplePlacement();
        newPlacement.angleDegrees = angle;
        Undo.RecordObject(levelData, "Add Apple");
        levelData.applePlacements.Add(newPlacement);
    }

    private void HandleKnifePlacement(Vector2 mousePosition, Vector2 clickOffset, Vector2 center, float radiusPixels, float logRadius, LevelDataSO levelData)
    {
        KnifePlacement existing = null;
        foreach (KnifePlacement placement in levelData.obstacleKnifePlacements)
        {
            Vector2 pointPosition = GetPointOnCircle(center, radiusPixels, logRadius, placement.angleDegrees, logRadius);
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
        Undo.RecordObject(levelData, "Add Knife");
        levelData.obstacleKnifePlacements.Add(newPlacement);
    }
}