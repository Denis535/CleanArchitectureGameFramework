#nullable enable
namespace System.Diagnostics.CodeAnalysis {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    // https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis

    // Input:  field (set), property (set), argument
    // Output: field (get), property (get), argument (out), result

    // AllowNull            - Input allows null
    // DisallowNull         - Input disallows null

    // MaybeNull            - Output may be null
    // MaybeNull/When       - Output may be null (when result == true/false)

    // NotNull              - Output is non-null
    // NotNull/When         - Output is non-null (when result == true/false)
    // NotNull/If/NotNull   - Output is non-null (if argument == non-null)

    // MemberNotNull        - Method/property will ensure that the output is non-null
    // MemberNotNull/When   - Method/property will ensure that the output is non-null (when result == true/false)

    // DoesNotReturn        - Method will never return
    // DoesNotReturn/If     - Method will never return (if argument == true/false)

    // Input
    //[AttributeUsage( AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false )]
    //internal sealed class AllowNullAttribute : Attribute {
    //}
    //[AttributeUsage( AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false )]
    //internal sealed class DisallowNullAttribute : Attribute {
    //}

    // Output/MaybeNull
    //[AttributeUsage( AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false )]
    //internal sealed class MaybeNullAttribute : Attribute {
    //}
    //[AttributeUsage( AttributeTargets.Parameter, Inherited = false )]
    //internal sealed class MaybeNullWhenAttribute : Attribute {
    //    public bool ReturnValue { get; }
    //    public MaybeNullWhenAttribute(bool returnValue) {
    //        ReturnValue = returnValue;
    //    }
    //}

    // Output/NotNull
    //[AttributeUsage( AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false )]
    //internal sealed class NotNullAttribute : Attribute {
    //}
    //[AttributeUsage( AttributeTargets.Parameter, Inherited = false )]
    //internal sealed class NotNullWhenAttribute : Attribute {
    //    public bool ReturnValue { get; }
    //    public NotNullWhenAttribute(bool returnValue) {
    //        ReturnValue = returnValue;
    //    }
    //}
    //[AttributeUsage( AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false )]
    //internal sealed class NotNullIfNotNullAttribute : Attribute {
    //    public string ParameterName { get; }
    //    public NotNullIfNotNullAttribute(string parameterName) {
    //        ParameterName = parameterName;
    //    }
    //}

    // Ensure
    [EditorBrowsable( EditorBrowsableState.Never )]
    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true )]
    internal sealed class MemberNotNullAttribute : Attribute {
        public string[] Members { get; }
        public MemberNotNullAttribute(string member) {
            Members = new[] { member };
        }
        public MemberNotNullAttribute(params string[] members) {
            Members = members;
        }
    }
    [EditorBrowsable( EditorBrowsableState.Never )]
    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true )]
    internal sealed class MemberNotNullWhenAttribute : Attribute {
        public bool ReturnValue { get; }
        public string[] Members { get; }
        public MemberNotNullWhenAttribute(bool returnValue, string member) {
            ReturnValue = returnValue;
            Members = new[] { member };
        }
        public MemberNotNullWhenAttribute(bool returnValue, params string[] members) {
            ReturnValue = returnValue;
            Members = members;
        }
    }

    // Ensure
    //[AttributeUsage( AttributeTargets.Method, Inherited = false )]
    //internal sealed class DoesNotReturnAttribute : Attribute {
    //}
    //[AttributeUsage( AttributeTargets.Parameter, Inherited = false )]
    //internal sealed class DoesNotReturnIfAttribute : Attribute {
    //    public bool ParameterValue { get; }
    //    public DoesNotReturnIfAttribute(bool parameterValue) {
    //        ParameterValue = parameterValue;
    //    }
    //}

}
