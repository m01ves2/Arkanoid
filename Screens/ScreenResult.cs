namespace Arkanoid.Screens
{
    public enum ScreenResultType
    {
        None,
        StartGame,
        OpenSettings,
        Menu,
        Exit
    }

    public class ScreenResult
    {
        public ScreenResultType Type { get; }
        //public string Payload; // optional

        public ScreenResult(ScreenResultType type)
        {
            this.Type = type;
        }
    }
}
