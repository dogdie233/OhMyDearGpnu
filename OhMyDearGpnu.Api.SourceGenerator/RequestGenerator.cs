using System;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace OhMyDearGpnu.Api.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class RequestGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName("OhMyDearGpnu.Api.Common.RequestAttribute",
            static (syntaxNode, ct) => syntaxNode is ClassDeclarationSyntax,
            static (context, ct) =>
            {
                var ns = context.TargetSymbol.ContainingNamespace.ToDisplayString();
                var symbol = (INamedTypeSymbol)context.TargetSymbol;
                var cls = symbol.Name;

                var generic = symbol.TypeParameters.Length == 0 ? "" : $"<{string.Join(", ", symbol.TypeParameters.Select(v => v.ToString()))}>";

                var payloadTypeStr = context.Attributes[0].ConstructorArguments[0].Value!.ToString();
                if (!Enum.TryParse<PayloadTypeEnum>(payloadTypeStr.Contains('.') ? payloadTypeStr.Substring(payloadTypeStr.LastIndexOf('.')) : payloadTypeStr, out var payloadType))
                    payloadType = PayloadTypeEnum.None;

                if (payloadType != PayloadTypeEnum.FormUrlEncoded)
                    return (ns, cls, generic, payloadType, null!);

                var attributeSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName("OhMyDearGpnu.Api.Common.FormItemAttribute");
                var members = symbol.GetMembers();
                var properties = members.OfType<IPropertySymbol>()
                    .Select(symbol => (symbol, ad: symbol.GetAttributes().FirstOrDefault(attr => attr.AttributeClass?.Equals(attributeSymbol, SymbolEqualityComparer.Default) ?? false)))
                    .Where(info => info.ad != null);
                return (ns, cls, generic, payloadType, properties: properties.ToArray());
            });

        context.RegisterSourceOutput(pipeline, static (context, data) =>
        {
            string createHttpContentBody;
            switch (data.payloadType)
            {
                case PayloadTypeEnum.None:
                    createHttpContentBody = "return null;";
                    break;
                case PayloadTypeEnum.Json:
                    createHttpContentBody = "return JsonContent.Create(this);";
                    break;
                case PayloadTypeEnum.FormUrlEncoded:
                    createHttpContentBody = "KeyValuePair<string, string?>[] formItems = [";
                    var wrote = false;
                    foreach (var property in data.properties!)
                    {
                        var keyName = property.ad!.ConstructorArguments[0].Value!.ToString();
                        if (wrote)
                            createHttpContentBody += ",";
                        else
                            wrote = true;
                        createHttpContentBody += $"\n        new KeyValuePair<string, string?>(\"{keyName}\", FormItemSerializeHelper.Serialize(this.{property.symbol.Name}))";
                    }

                    if (wrote)
                        createHttpContentBody += "\n        ];\n";
                    else
                        createHttpContentBody += "];\n";
                    createHttpContentBody += "        return new FormUrlEncodedContent(formItems.Where(kvp => kvp.Value != null));";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(data.payloadType));
            }

            var source = SourceText.From($$"""
                                           using OhMyDearGpnu.Api.Common;
                                           using OhMyDearGpnu.Api.Utility;

                                           using System.Net;
                                           using System.Net.Http;
                                           using System.Net.Http.Json;

                                           namespace {{data.ns}};

                                           partial class {{data.cls}}{{data.generic}}
                                           {
                                           #nullable enable
                                               public override HttpContent? CreateHttpContent(SimpleServiceContainer serviceContainer)
                                               {
                                                   {{createHttpContentBody}}
                                               }
                                           #nullable disable
                                           }
                                           """, Encoding.UTF8);

            context.AddSource($"{data.ns}.{data.cls}.g.cs", source);
        });
    }
}