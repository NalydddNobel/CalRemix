//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CalRemix.CrossCompatibility.OutboundCompatibility;

//[AttributeUsage(AttributeTargets.Class)]
//public class ArgumentsAttribute : Attribute {
//    private Type[] _validTypes;

//    public ArgumentsAttribute(params Type[] validTypes) { 
//        _validTypes = validTypes;
//    }

//    public IEnumerable<Type> GetValidTypes() {
//        return _validTypes;
//    }
//}