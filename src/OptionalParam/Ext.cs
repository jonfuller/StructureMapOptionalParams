using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StructureMap.Pipeline;

namespace OptionalParam
{
    public static class Ext
    {
        public static ExplicitArguments AsArgs(this object anonymousType)
        {
            return new ExplicitArguments(anonymousType.GetType()
                .GetProperties()
                .Select(prop => new { prop.Name, Value = prop.GetValue(anonymousType, new object[0]) })
                .ToDictionary(x => x.Name, x => x.Value));
        }

        public static IEnumerable<T> Eval<T>(this IEnumerable<T> target)
        {
            return target.ToList();
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> target, T toAppend)
        {
            foreach(var item in target)
                yield return item;
            yield return toAppend;
        }

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