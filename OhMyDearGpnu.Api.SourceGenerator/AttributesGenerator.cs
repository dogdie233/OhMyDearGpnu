using Microsoft.CodeAnalysis;

namespace OhMyDearGpnu.Api.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class AttributesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(GenerateAttributes);
    }

    private static void GenerateAttributes(IncrementalGeneratorPostInitializationContext context)
    {
        /* lang=c#-test */
        var requestAttribute = """
                               using System;

                               namespace OhMyDearGpnu.Api.Requests;

                               [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
                               internal sealed class RequestAttribute(PayloadTypeEnum PayloadType) : Attribute
                               {
                               }

                               [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
                               internal sealed class FormItemAttribute(string Name) : Attribute
                               {
                               }
                               """;

        context.AddSource("OhMyDearGpnu.Api.Requests.Attributes.g.cs", requestAttribute);
    }
}