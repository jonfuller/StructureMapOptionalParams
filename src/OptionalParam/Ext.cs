using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OptionalParam
{
    public static class Ext
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> target, Action<T> action)
        {
            foreach(var item in target)
                action(item);
            return target;
        }

        public static bool HasOptionalParameters(this ConstructorInfo ctor)
        {
            return ctor.GetOptionalParameters().Any();
        }

        public static IEnumerable<ParameterInfo> GetOptionalParameters(this ConstructorInfo ctor)
        {
            return ctor.GetParameters().Where(param => param.Attributes.HasFlag(ParameterAttributes.Optional));
        }

        public static ConstructorInfo GetGreediestCtor(this Type target)
        {
            return target.GetConstructors().WithMax(ctor => ctor.GetParameters().Length);
        }

        public static T WithMax<T>(this IEnumerable<T> target, Func<T, int> selector)
        {
            int max = -1;
            T currentMax = default(T);

            foreach(var item in target)
            {
                var current = selector(item);
                if(current <= max)
                    continue;

                max = current;
                currentMax = item;
            }

            return currentMax;
        }
    }
}