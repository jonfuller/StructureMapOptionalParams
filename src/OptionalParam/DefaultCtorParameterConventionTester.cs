using NUnit.Framework;
using StructureMap;

namespace OptionalParam
{
    [TestFixture]
    public class DefaultCtorParameterConventionTester
    {
        [Test]
        public void Process_to_Container_with_Only_Default_Param_As_Convention()
        {
            var container = new Container(registry => registry.Scan(x =>
            {
                x.TheCallingAssembly();
                x.Convention<DefaultCtorParameterConvention>();
            }));

            Assert.That(container.GetInstance<OptionalOnly>().Value, Is.EqualTo(3));
        }

        [Test]
        public void Process_to_Container_with_Only_Default_Param_As_With()
        {
            var container = new Container(registry => registry.Scan(x =>
            {
                x.TheCallingAssembly();
                x.With(new DefaultCtorParameterConvention());
            }));

            Assert.That(container.GetInstance<OptionalOnly>().Value, Is.EqualTo(3));
        }

        [Test]
        public void Process_to_Container_with_A_Default_Param_And_Another_Param_As_Convention()
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
        public void Process_to_Container_with_A_Default_Param_And_Another_Param_As_With()
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

        [Test]
        public void Process_to_Container_with_A_Default_Param_With_Custom_Registration()
        {
            var container = new Container(registry => registry.Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
                x.With(new DefaultCtorParameterConvention(
                    type => type.Name,
                    (r, type) => r.For(typeof(object)).Add(type)));
            }));

            var name = typeof(OptionalWithAnother).Name;
            var value = container.GetInstance(typeof(object), name);
            var typed = (OptionalWithAnother)value;

            Assert.That(value, Is.InstanceOf<OptionalWithAnother>());
            Assert.That(typed.Value, Is.EqualTo(3));
            Assert.That(typed.Server, Is.InstanceOf<Server>());
        }

        [Test]
        public void Process_to_Container_with_A_Default_Param_With_Custom_Name()
        {
            var container = new Container(registry => registry.Scan(x =>
            {
                x.TheCallingAssembly();
                x.WithDefaultConventions();
                x.With(new DefaultCtorParameterConvention(
                    type => "Custom" + type.Name,
                    (r, type) => r.For(typeof(object)).Add(type)));
            }));

            var value = container.GetInstance(typeof(object), "Custom" + typeof(OptionalWithAnother).Name);
            var typed = (OptionalWithAnother)value;

            Assert.That(value, Is.InstanceOf<OptionalWithAnother>());
            Assert.That(typed.Value, Is.EqualTo(3));
            Assert.That(typed.Server, Is.InstanceOf<Server>());
        }
    }

    public class NonOptional
    {
        public NonOptional(int a) { }
        public NonOptional(int a, int b) { }
        public NonOptional(int a, string b) { }
    }

    public class OptionalOnly
    {
        public int Value { get; set; }

        public OptionalOnly(int value = 3)
        {
            Value = value;
        }
    }

    public class OptionalWithAnother
    {
        public IServer Server { get; set; }
        public int Value { get; set; }

        public OptionalWithAnother(IServer server, int value = 3)
        {
            Server = server;
            Value = value;
        }
    }

    public interface IServer { }

    public class Server : IServer { }

    public abstract class AbstractClass { }
}