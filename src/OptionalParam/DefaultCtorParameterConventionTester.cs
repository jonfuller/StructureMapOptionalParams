using NUnit.Framework;
using StructureMap;

namespace OptionalParam
{
    [TestFixture]
    public class DefaultCtorParameterConventionTester
    {
        [Test]
        public void Process_to_Container_with_Only_Default_Param()
        {
            var container = new Container(registry => registry.Scan(x =>
            {
                x.TheCallingAssembly();
                x.Convention<DefaultCtorParameterConvention>();
            }));

            Assert.That(container.GetInstance<OptionalOnly>().Value, Is.EqualTo(3));
        }

        [Test]
        public void Process_to_Container_with_Only_Default_Param2()
        {
            var container = new Container(registry => registry.Scan(x =>
            {
                x.TheCallingAssembly();
                x.With(new DefaultCtorParameterConvention());
            }));

            Assert.That(container.GetInstance<OptionalOnly>().Value, Is.EqualTo(3));
        }

        [Test]
        public void Process_to_Container_with_A_Default_Param_And_Another_Param()
        {
            var container = new Container(registry => registry.Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
                x.Convention<DefaultCtorParameterConvention>();
            }));

            Assert.That(container.GetInstance<OptionalWithAnother>().Value, Is.EqualTo(3));
            Assert.That(container.GetInstance<OptionalWithAnother>().Server, Is.InstanceOf<Server>());
        }

        [Test]
        public void Process_to_Container_with_A_Default_Param_And_Another_Param2()
        {
            var container = new Container(registry => registry.Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
                x.With(new DefaultCtorParameterConvention());
            }));

            Assert.That(container.GetInstance<OptionalWithAnother>().Value, Is.EqualTo(3));
            Assert.That(container.GetInstance<OptionalWithAnother>().Server, Is.InstanceOf<Server>());
        }
    }

    class NonOptional
    {
        public NonOptional(int a) { }
        public NonOptional(int a, int b) { }
        public NonOptional(int a, string b) { }
    }

    class OptionalOnly
    {
        public int Value { get; set; }

        public OptionalOnly(int value = 3)
        {
            Value = value;
        }
    }

    class OptionalWithAnother
    {
        public IServer Server { get; set; }
        public int Value { get; set; }

        public OptionalWithAnother(IServer server, int value = 3)
        {
            Server = server;
            Value = value;
        }
    }

    interface IServer { }

    class Server : IServer { }

    abstract class AbstractClass { }
}