using System.Reflection;
using Terraria;

namespace CalRemix.CrossCompatibility.OutboundCompatibility; 

public readonly record struct Caller(string Name) {
    public Caller(Assembly assembly) : this(assembly.GetName().Name) {
    }

    public void LogError(object message) {
        CalRemix.instance.Logger.Error($"Error from {Name}: {message}");
        if (!Main.dedServ && !Main.gameMenu) {
            Main.NewText($"Error from {Name}: {message}", Main.errorColor);
        }
    }
}