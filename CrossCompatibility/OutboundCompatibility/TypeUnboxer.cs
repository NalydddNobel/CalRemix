using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;

namespace CalRemix.CrossCompatibility.OutboundCompatibility;

internal static class TypeUnboxer {
    internal static readonly Dictionary<Type, Dictionary<Type, Func<object, object>>> _unboxFactories = new();

    static TypeUnboxer() {
        TypeUnboxer<Func<IEnumerable<NPC>, bool>>.Add(typeof(Func<bool>), (obj) => (nearbyNPCs) => ((Func<bool>)obj)());
        TypeUnboxer<int>.Add(typeof(short), (obj) => (short)obj);
        TypeUnboxer<int>.Add(typeof(ushort), (obj) => (ushort)obj);
        TypeUnboxer<int>.Add(typeof(byte), (obj) => (byte)obj);
        TypeUnboxer<int>.Add(typeof(sbyte), (obj) => (sbyte)obj);
    }

    public static IEnumerable<Type> GetAlternativeTypes(Type type) {
        if (_unboxFactories.TryGetValue(type, out var dictionary)) {
            return dictionary.Keys;
        }
        return Array.Empty<Type>();
    }
}

internal static class TypeUnboxer<T> {
    public static void Add(Type type, Func<object, T> unboxFactory) {
        (CollectionsMarshal.GetValueRefOrAddDefault(TypeUnboxer._unboxFactories, typeof(T), out _) ??= new())[type] = (obj) => unboxFactory(obj);
    }

    /// <exception cref="ArgumentNullException">When <paramref name="obj"/> is null.</exception>
    public static bool TryUnbox(object obj, out T value) {
        if (obj == null) {
            throw new ArgumentNullException(nameof(obj));
        }

        if (obj.GetType().Equals(typeof(T))) {
            value = (T)obj;
            return true;
        }

        if (TypeUnboxer._unboxFactories.TryGetValue(typeof(T), out var dictionary) && dictionary.TryGetValue(obj.GetType(), out var unboxAction)) {
            value = (T)unboxAction(obj);
            return true;
        }

        value = default;
        return false;
    }

    public static IEnumerable<Type> GetAlternativeTypes() {
        return TypeUnboxer.GetAlternativeTypes(typeof(T));
    }
}