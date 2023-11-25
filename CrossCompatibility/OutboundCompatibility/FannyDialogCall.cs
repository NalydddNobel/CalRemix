using CalamityMod;
using CalRemix.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CalRemix.CrossCompatibility.OutboundCompatibility;

[LegacyName("TestName", "Tesst", "Hihi")]
internal class FannyDialogCall : ModCallProvider {
    // Required arguments are:
    // string - Identifier. Imagine this as an entry into one of Terraria's various string Dictionaries, suggested to put your mod's name in the front.

    // string - Message. I personally wish this was a LocalizedText, but this is very self explanatory.

    // string - Portrait. There are various types of Portraits to choose from. The options include:
    //  -Idle
    //  -Awooga
    //  -Cryptid
    //  -Sob
    //  -Nuhuh
    //
    // Evil:
    //  -EvilIdle
    //
    // Wonder Flower:
    //  -TalkingFlower

    // Anonymous Type - Optional arguments. Create an anonymous type by using new().
    // new
    // {
    //     Evil = true,
    //     InventoryOnly = true,
    //     Saves = true,
    // }
    // Optional arguments are detailed in the below comments, inside of the "AnonymousEvents" constructor.


    private static readonly Dictionary<string, Action<object, FannyMessage>> AnonymousEvents = new() {
        // Func<IEnumerable<NPC>, bool> - Condition for making this message appear.
        // Example:
        //  Condition = (npcs) => foreach (NPC npc in npcs) 
        //              {
        //                  if (npc.type == NPCID.Guide)
        //                  {
        //                      return true;
        //                  }
        //              }
        ["Condition"] = (obj, message) => {
            if (TypeUnboxer<Func<IEnumerable<NPC>, bool>>.TryUnbox(obj, out var condition)) {
                message.Condition = metrics => condition(metrics.onscreenNPCs);
                return;
            }
        },
        // (int, float, Vector2) - Adds an item display to this Fanny message.
        // Be sure to specify the type as Int32 if the Type is not an Int32 exactly.
        // Example:
        //  ItemDisplay = ((int)ItemID.ITEMNAME, 1f, Vector2.Zero)
        ["ItemDisplay"] = (obj, message) => {
            if (TypeUnboxer<(int, float, Vector2)>.TryUnbox(obj, out var tuple)) {
                message.AddItemDisplay(tuple.Item1, tuple.Item2, tuple.Item3);
                return;
            }
        },
        // string - Hover text
        // Example:
        //  HoverText = "Thank you Fanny!!! My bestie!!!"
        ["HoverText"] = (obj, message) => {
            if (TypeUnboxer<string>.TryUnbox(obj, out var text)) {
                message.SetHoverTextOverride(text);
                return;
            }
        },
        // float - How long the message's cooldown lasts, in seconds.
        // Default/Example:
        //  Cooldown = 1f
        ["Cooldown"] = (obj, message) => {
            if (TypeUnboxer<float>.TryUnbox(obj, out var value)) {
                message.CooldownTime = CalamityUtils.SecondsToFrames(value);
                return;
            }
        },
        // float - How long the message's cooldown lasts, in seconds.
        // Default/Example:
        //  Duration = 1f
        ["Duration"] = (obj, message) => {
            if (TypeUnboxer<float>.TryUnbox(obj, out var value)) {
                message.messageDuration = CalamityUtils.SecondsToFrames(value);
                return;
            }
        },
        // (int, float) - Adjusts the box's maximum width, and text's size.
        // int = text box width
        // float = text size
        // Default/Example:
        //  TextSize = (380, 1f)
        ["TextSize"] = (obj, message) => {
            if (TypeUnboxer<(int, float)>.TryUnbox(obj, out var value)) {
                message.maxTextWidth = value.Item1;
                message.textSize = value.Item2;
                message.FormatText(FontAssets.MouseText.Value, value.Item1);
                return;
            }
        },
        // bool - Makes the message spoken by Evil Fanny.
        // Default/Example:
        //  Evil = false
        ["Evil"] = (obj, message) => {
            if (TypeUnboxer<bool>.TryUnbox(obj, out var value) && value) {
                message.SpokenByEvilFanny();
                return;
            }
        },
        // bool - Makes the message only appear while the inventory is opened.
        // Default/Example:
        //  InventoryOnly = false
        ["InventoryOnly"] = (obj, message) => {
            if (TypeUnboxer<bool>.TryUnbox(obj, out var value)) {
                message.DisplayOutsideInventory = !value;
                return;
            }
        },
        // bool - Whether or not this Fanny message can be clicked off.
        // Default/Example:
        //  Unclickable = false
        ["Unclickable"] = (obj, message) => {
            if (TypeUnboxer<bool>.TryUnbox(obj, out var value)) {
                message.NeedsToBeClickedOff = value;
                return;
            }
        },
        // bool - Whether or not this Fanny message should be saved as read. 
        // (Prevents it from being seen again after reloading the world.)
        // Default/Example:
        //  Saves = true
        ["Saves"] = (obj, message) => {
            if (TypeUnboxer<bool>.TryUnbox(obj, out var value)) {
                message.PersistsThroughSaves = value;
                return;
            }
        },
        // bool - Whether or not this Fanny message can be repeated without reloading the world.
        // Default/Example:
        //  Repeats = false
        ["Repeats"] = (obj, message) => {
            if (TypeUnboxer<bool>.TryUnbox(obj, out var value)) {
                message.OnlyPlayOnce = !value;
                return;
            }
        },
    };

    public override object Call(Caller caller, object[] args) {
        var message = new FannyMessage((string)args[1], (string)args[2], (string)args[3]);
        if (args.Length > 4) {
            var obj = args[4];
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj)) {
                if (AnonymousEvents.TryGetValue(property.Name, out var events)) {
                    events(property.GetValue(obj), message);
                    continue;
                }

                caller.LogError($"Property {property.Name} was not found. Did you mean {StringMatching.GetClosestMatch(property.Name, AnonymousEvents.Keys)}?");
            }
        }
        FannyManager.LoadFannyMessage(message);
        return message;
    }
}

#region Old Calls

#endregion