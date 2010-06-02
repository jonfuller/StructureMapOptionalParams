using System.Linq;
using NUnit.Framework;

namespace OptionalParam
{
    [TestFixture]
    public class ExtTests
    {
        [Test]
        public void WithMaxGetsItemWithMaxCount()
        {
            var items = new[] { "hello", "goodbye", "whatever", "bogus" };

            Assert.That(items.WithMax(x => x.Length), Is.EqualTo("whatever"));
        }

        [Test]
        public void WithMaxGetsFirstItemWithMaxCountIfMultipleWithSame()
        {
            var items = new[] { "hello", "goodbye", "whatever", "88888888" };

            Assert.That(items.WithMax(x => x.Length), Is.EqualTo("whatever"));
        }

        [Test]
        public void DetectsGreediestConstructor()
        {
            var ctor = typeof(NonOptional).GetGreediestCtor();
            
            Assert.That(ctor.GetParameters().Length, Is.EqualTo(2));
        }

        [Test]
        public void UsesFirstDeclaredIfMultipleGreediestConstructors()
        {
            var ctor = typeof(NonOptional).GetGreediestCtor();

            Assert.That(ctor.GetParameters()[0].ParameterType, Is.EqualTo(typeof(int)));
            Assert.That(ctor.GetParameters()[1].ParameterType, Is.EqualTo(typeof(int)));
        }

        [Test]
        public void DetectsOptionalArguments()
        {
            Assert.That(typeof(OptionalOnly).GetGreediestCtor().HasOptionalParameters());

            Assert.That(typeof(NonOptional).GetGreediestCtor().HasOptionalParameters(), Is.False);
        }

        [Test]
        public void GetsOptionalArguments()
        {
            var optionalArgs = typeof(OptionalOnly).GetGreediestCtor().GetOptionalParameters();

            Assert.That(optionalArgs, Is.Not.Empty);
            Assert.That(optionalArgs.First().DefaultValue, Is.EqualTo(3));
        }
    }
}