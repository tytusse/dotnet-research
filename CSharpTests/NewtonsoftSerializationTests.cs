using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit.Abstractions;

namespace CSharpTests;

public class NewtonsoftSerializationTests(ITestOutputHelper testOutputHelper) {
    public record SimpleClass(string Name, string? Nickname);
    
    [Fact]
    public void TestNonNullablePropertiesRequiredResolver() {
        const string json = $"{{\"Nickname\":\"bubba\", \"Age\":30}}";
        
        var settings = new JsonSerializerSettings {
            //MissingMemberHandling = MissingMemberHandling.Error,
            TraceWriter =  new TraceWriter(testOutputHelper),
            ContractResolver = new NonNullablePropertiesRequiredResolver(testOutputHelper)
        };
        
        var exc = Assert.Throws<JsonSerializationException>(
            () => JsonConvert.DeserializeObject<SimpleClass>(json, settings));
        
        testOutputHelper.WriteLine(exc.ToString());
        
    }
    
    // Source - https://stackoverflow.com/a/57680436
    // Posted by Brian Rogers, modified by community. See post 'Timeline' for change history
    // Retrieved 2025-11-24, License - CC BY-SA 4.0
    private class NonNullablePropertiesRequiredResolver(ITestOutputHelper testOutputHelper) 
        : DefaultContractResolver {
        private readonly NullabilityInfoContext _nullabilityInfoContext = new(); 
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var prop = base.CreateProperty(member, memberSerialization);
            var isNullable = IsNullable(member);
            testOutputHelper.WriteLine(
                $"{prop.PropertyName}: {member.ReflectedType} ({member.GetType().Name}) {(isNullable ? "nullable":"")}");
            if (!isNullable) {
                prop.Required = Required.Always;
            }
            return prop;
        }

        private bool IsNullable(MemberInfo member) {
            return member switch {
                PropertyInfo prop => _nullabilityInfoContext.Create(prop).WriteState == NullabilityState.Nullable,
                FieldInfo field => _nullabilityInfoContext.Create(field).ReadState == NullabilityState.Nullable,
                _ => true // make nullable by default
            };
        }
    }
    
    private class TraceWriter(ITestOutputHelper helper) : ITraceWriter {
        public void Trace(TraceLevel level, string message, Exception? ex) {
            if (level > LevelFilter) {
                return;
            }

            helper.WriteLine(ex is not null ? $"{message}\n{ex}" : $"{message}");
        }

        public TraceLevel LevelFilter => TraceLevel.Verbose;
    }
}