using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace CalRemix.CrossCompatibility.OutboundCompatibility; 

internal sealed class ModCallManager : ModSystem {
    private static readonly Dictionary<string, ModCallProvider> _modCallsByName = new();

    public static void Register(ModCallProvider modCallProvider) {
        _modCallsByName.Add(modCallProvider.Name, modCallProvider);
        foreach (var legacyName in LegacyNameAttribute.GetLegacyNamesOfType(modCallProvider.GetType())) {
            _modCallsByName.Add(legacyName, modCallProvider);
            modCallProvider.AltNames.Add(legacyName);
        }
    }

    public static object Call(Caller caller, params object[] args) {
        if (!args.Any()) {
            throw new ArgumentException("There must be at least one argument in order to use mod calls.");
        }

        if (args[0] is not string command) {
            throw new ArgumentException("The first argument must supply a string that specifies the mod call's type.");
        }

        if (_modCallsByName.TryGetValue(command, out var provider)) {
            try {
                return provider.Call(caller, args);
            }
            catch (Exception ex) {
                caller.LogError($"{ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        caller.LogError($"Mod Call '{command}' was not found. Did you mean: {StringMatching.GetClosestMatch(command, _modCallsByName.Keys)}?");
        return null;
    }
}
