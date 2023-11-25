using System;
using System.Collections.Generic;
using System.Reflection;

namespace CalRemix.CrossCompatibility.OutboundCompatibility;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
internal class AltNameAttribute : Attribute {
    private readonly string[] _altNames;

    public AltNameAttribute(params string[] names) {
        _altNames = names;
    }

    public static IEnumerable<string> GetAlternativeNames(MemberInfo member) {
        foreach (var altNamesAttr in member.GetCustomAttributes<AltNameAttribute>(inherit: false)) {
            var names = altNamesAttr._altNames;
            for (int i = 0; i < names.Length; i++) {
                yield return names[i];
            }
        }
    }
}