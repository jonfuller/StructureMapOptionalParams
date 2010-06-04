using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Pipeline;

namespace OptionalParam
{
    public class DefaultCtorParameterConvention : IRegistrationConvention
    {
        static readonly Func<Type, string> DefaultNamer = type => type.Name;

        readonly IEnumerable<Func<Registry, Type, ConfiguredInstance>> _registrations;
        readonly Func<Type, string> _namer;

        public DefaultCtorParameterConvention()
            : this(DefaultNamer)
        {
        }

        public DefaultCtorParameterConvention(Func<Type, string> namer, params Func<Registry, Type, ConfiguredInstance>[] additionalRegistrations)
        {
            _namer = namer ?? DefaultNamer;
            _registrations = (additionalRegistrations ?? Enumerable.Empty<Func<Registry, Type, ConfiguredInstance>>())
                .Append((registry, type) => registry.For(type).Use(type))
                .Eval();
        }

        public void Process(Type type, Registry registry)
        {
            if(type.IsAbstract || type.IsEnum)
                return;

            var ctor = type.GetGreediestCtor();

            if(!ctor.HasOptionalParameters())
                return;

            var name = _namer(type);

            var instances = _registrations
                .Select(r => r(registry, type))
                .Eval()
                .Each(instance => instance.WithName(name));

            var parameters = ctor.GetOptionalParameters();

            foreach(var parameter in parameters)
            foreach(var instance in instances)
                    instance.Child(parameter.Name).Is(parameter.DefaultValue);
        }
    }

}
