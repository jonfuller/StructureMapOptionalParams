using NUnit.Framework;
using StructureMap;

namespace OptionalParam
{
    [TestFixture]
    public class Example
    {
        [Test]
        public static void Run()
        {
            var container = new Container(config =>
            {
                config.Scan(scanner =>
                {
                    scanner.TheCallingAssembly();
                    scanner.AddAllTypesOf<IDestination>();
                    scanner.WithDefaultConventions();
                    scanner.Convention<DefaultCtorParameterConvention>();
                });
            });

            var defaultLogCommand = container.GetInstance<LogCommand>();
            var errorLogCommand = container.GetInstance<LogCommand>(new {level = Level.Error}.AsArgs());
        }
    }

    public class LogCommand
    {
        readonly IDestination _destination;
        readonly Level _level;

        public LogCommand(IDestination destination, Level level = Level.Info)
        {
            _destination = destination;
            _level = level;
        }

        /* logging code here */
    }

    public interface IDestination { }

    public class ConsoleDestination : IDestination
    {
        public ConsoleDestination() { }
    }

    public enum Level
    {
        Info,
        Warning,
        Error
    }
}
