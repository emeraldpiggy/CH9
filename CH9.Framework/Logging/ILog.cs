namespace CH9.Framework.Logging
{
    public interface ILog
    {
        void Log(LogLevel level, string format, params object[] args);
    }
}