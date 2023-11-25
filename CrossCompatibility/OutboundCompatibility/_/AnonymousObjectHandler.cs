//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;

//namespace CalRemix.CrossCompatibility.OutboundCompatibility;

//internal class AnonymousObjectHandler<T> {
//    private record struct MemberSetter(Action<T, object> SetEvent, Type WantedType, MemberInfo Member, bool Alternative = false);

//    private static readonly Dictionary<string, MemberSetter> _setters = new();

//    static AnonymousObjectHandler() {
//        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

//        foreach (var field in typeof(T).GetFields(bindingFlags).Where((f) => !f.IsInitOnly)) {
//            AddSetter(field, new MemberSetter((obj, arg) => field.SetValue(obj, arg), field.FieldType, field));
//        }

//        foreach (var property in typeof(T).GetProperties(bindingFlags).Where((p) => p.GetSetMethod() != null)) {
//            AddSetter(property, new MemberSetter((obj, arg) => property.SetMethod.Invoke(obj, new object[] { arg }), property.PropertyType, property));
//        }

//        static void AddSetter(MemberInfo member, MemberSetter setter) {
//            _setters[member.Name] = setter;
//            foreach (var altName in AltNameAttribute.GetAlternativeNames(member)) {
//                _setters[altName] = setter with { Alternative = true };
//            }
//        }
//    }

//    public static void ApplyTo(T obj, object anonymous, Caller caller) {
//        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(anonymous)) {
//            string name = property.Name;
//            if (!_setters.TryGetValue(name, out var setter)) {
//                caller.LogError($"Property '{name}' was not found. Did you mean: {StringMatching.GetClosestMatch(name, _setters.Keys)}?");
//                continue;
//            }
            
//            if (setter.WantedType != property.PropertyType) {
//                string altTypes = string.Join(", ", TypeUnboxer.GetAlternativeTypes(property.PropertyType));
                
//                caller.LogError($"Property '{name}' ({property.PropertyType}) must be one of the following types: {setter.WantedType}{(string.IsNullOrEmpty(altTypes) ? "" : ", " + setter.WantedType)}");
//                continue;
//            }
//            setter.SetEvent(obj, property.GetValue(anonymous));
//        }
//    }
//}