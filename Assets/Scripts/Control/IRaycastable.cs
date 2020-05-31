namespace RPG.Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType(CursorType cursorType);
        bool HandleRaycast(PlayerController callingController);
    }
}


