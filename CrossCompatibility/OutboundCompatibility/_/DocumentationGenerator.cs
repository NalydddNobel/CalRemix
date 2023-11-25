//using Humanizer;
//using ReLogic.Utilities;
//using System.IO;
//using System.Text;
//using Terraria;
//using Terraria.ModLoader;

//namespace CalRemix.CrossCompatibility.OutboundCompatibility;

//#if DEBUG
//public class DocumentationGenerator : ModSystem {
//    public override void PostSetupContent() {
//        string createFile = $"{Main.SavePath.Replace("tModLoader-preview", "tModLoader")}/ModSources/{typeof(DocumentationGenerator).Namespace.Replace('.', '/')}/Debug/Documentation.txt";
//        using var file = File.Create(createFile);
//        var encoding = Encoding.UTF8;
//        string result = "{{mod sub-page}}";
//        foreach (var call in Mod.GetContent<ModCallProvider>()) {
//            WriteLine($"== {call.Name} ==");
//            if (call.AltNames.Count > 0) {
//                WriteLine($"<small>{string.Join(", ", call.AltNames)}</small>");
//            }
//            WriteLine("");
//            WriteLine("'''Parameters'''");

//            WriteUsage(call);
//            WriteParameters(call);
//        }

//        var b = encoding.GetBytes(result);
//        file.Write(b, 0, b.Length);

//        void WriteLine(string text) => result += text + "\n";
//        void WriteUsage(ModCallProvider call) {
//            var attr = call.GetType().GetAttribute<UsageAttribute>();
//            if (attr == null || attr.Flags == UseFlag.Anytime) {
//                WriteLine("* Usage: ''Can be called at any time.''");
//                return;
//            }

//            WriteLine($"* Usage: ''{attr.Flags.Humanize()}.''");
//        }
//        void WriteParameters(ModCallProvider call) {
//            var attr = call.GetType().GetAttribute<ArgumentsAttribute>();
//            if (attr == null) {
//                WriteLine("* This call has no parameters.");
//                return;
//            }
//        }
//    }
//}
//#endif