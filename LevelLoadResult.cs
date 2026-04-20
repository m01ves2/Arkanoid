namespace Arkanoid
{
    public class LevelLoadResult
    {
        public char[,] Field { get; }
        public string? ErrorMessage { get; } = null;
        public bool IsFallback { get; }

        public LevelLoadResult(char[,] field, string message = null)
        {
            Field = field;
            ErrorMessage = message;
        }
    }
}
