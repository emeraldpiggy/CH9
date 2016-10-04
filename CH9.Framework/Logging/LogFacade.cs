namespace CH9.Framework.Logging
{
    public struct LogFacade
    {
        //This should always be empty
        public static LogFacade Default = new LogFacade("NLog");

        public string Name { get; set; }

        public LogFacade(string name)
        {
            Name = name;
        }

    }
}