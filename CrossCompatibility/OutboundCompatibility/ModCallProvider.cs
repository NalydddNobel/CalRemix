using System.Collections.Generic;
using Terraria.ModLoader;

namespace CalRemix.CrossCompatibility.OutboundCompatibility;

internal abstract class ModCallProvider : ModType {
    public readonly List<string> AltNames = new();

    protected sealed override void Register() {
        ModCallManager.Register(this);
    }

    public sealed override void SetupContent() {
        SetStaticDefaults();
    }

    public abstract object Call(Caller caller, object[] args);
}