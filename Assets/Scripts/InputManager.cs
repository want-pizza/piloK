using UnityEngine;

public static class InputManager
{
    public static KeyCode upKey = KeyCode.UpArrow;
    public static KeyCode downKey = KeyCode.DownArrow;
    public static KeyCode leftKey = KeyCode.LeftArrow;
    public static KeyCode rightKey = KeyCode.RightArrow;
    public static KeyCode jumpKey = KeyCode.Z;

    public static void ChangeKey(string direction, KeyCode newKey)
    {
        if (direction == "up") upKey = newKey;
        else if (direction == "down") downKey = newKey;
        else if (direction == "left") leftKey = newKey;
        else if (direction == "right") rightKey = newKey;
    }
}