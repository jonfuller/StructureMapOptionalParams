using System;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace OptionalParam
{
    public class DefaultCtorParameterConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if(type.IsAbstract)
                return;

            var instance = registry.For(type).Use(type);

            if(!type.GetGreediestCtor().HasOptionalParameters())
                return;

            type.GetGreediestCtor().GetOptionalParameters().Each(param =>
                instance.Child(param.Name).Is(param.DefaultValue));
        }
    }
}
