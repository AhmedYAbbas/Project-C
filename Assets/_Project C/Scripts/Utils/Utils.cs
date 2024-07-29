using TMPro;
using UnityEngine;

public static class Utils
{
    // Create Text in the World
    public static TextMeshProUGUI CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default, int fontSize = 40, Color? color = null, TextAlignmentOptions textAlignment = TextAlignmentOptions.Left)
    {
        if (color == null)
            color = Color.white;

        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAlignment);
    }

    // Create Text in the World
    public static TextMeshProUGUI CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAlignmentOptions textAlignment)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMeshProUGUI));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMeshProUGUI textMesh = gameObject.GetComponent<TextMeshProUGUI>();
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }

    // Get Mouse Position in World with Z = 0f
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
