using Microsoft.CodeAnalysis;

namespace OhMyDearGpnu.Api.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class PayloadTypeEnumGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static context =>
        {
            var source = """
                         namespace OhMyDearGpnu.Api.Requests;

                         public enum PayloadTypeEnum
                         {
                             None,
                             Json,
                             FormUrlEncoded
                         }
                         """;

            context.AddSource("OhMyDearGpnu.Api.Requests.PayloadTypeEnum.g.cs", source);
        });
    }
}