//-----------------------------------------------------------------------
// <copyright file="FileTemplateModel.cs" company="NSwag">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/NSwag/NSwag/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Linq;

namespace NSwag.CodeGeneration.CodeGenerators.CSharp.Models
{
    /// <summary>The CSharp file template model.</summary>
    public class FileTemplateModel
    {
        private readonly string _clientCode;
        private readonly SwaggerService _service;
        private readonly SwaggerToCSharpGeneratorSettings _settings;
        private readonly SwaggerToCSharpTypeResolver _resolver;
        private readonly ClientGeneratorOutputType _outputType;

        /// <summary>Initializes a new instance of the <see cref="FileTemplateModel" /> class.</summary>
        /// <param name="clientCode"></param>
        /// <param name="outputType"></param>
        /// <param name="service">The service.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="resolver">The resolver.</param>
        public FileTemplateModel(string clientCode, ClientGeneratorOutputType outputType, SwaggerService service,
            SwaggerToCSharpGeneratorSettings settings, SwaggerToCSharpTypeResolver resolver)
        {
            _clientCode = clientCode;
            _outputType = outputType;
            _service = service;
            _settings = settings;
            _resolver = resolver;
        }

        /// <summary>Gets the namespace.</summary>
        public string Namespace => _settings.CSharpGeneratorSettings.Namespace ?? string.Empty;

        /// <summary>Gets the all the namespace usages.</summary>
        public string[] NamespaceUsages => _outputType == ClientGeneratorOutputType.Contracts || _settings.AdditionalNamespaceUsages == null ?
            new string[] { } : _settings.AdditionalNamespaceUsages;

        /// <summary>Gets a value indicating whether to generate contract code.</summary>
        public bool GenerateContracts =>
            _outputType == ClientGeneratorOutputType.Full ||
            _outputType == ClientGeneratorOutputType.Contracts;

        /// <summary>Gets a value indicating whether to generate implementation code.</summary>
        public bool GenerateImplementation =>
            _outputType == ClientGeneratorOutputType.Full ||
            _outputType == ClientGeneratorOutputType.Implementation;

        /// <summary>Gets the clients code.</summary>
        public string Clients => _settings.GenerateClientClasses ? _clientCode : string.Empty;

        /// <summary>Gets the classes code.</summary>
        public string Classes => _settings.GenerateDtoTypes ? _resolver.GenerateClasses() : string.Empty;

        /// <summary>Gets a value indicating whether missing HTTP methods are used.</summary>
        public bool HasMissingHttpMethods => _service.Operations.Any(o =>
            (o.Method == SwaggerOperationMethod.Delete && o.Operation.ActualParameters.Any(p => p.Kind == SwaggerParameterKind.Body)) ||
            o.Method == SwaggerOperationMethod.Options ||
            o.Method == SwaggerOperationMethod.Head ||
            o.Method == SwaggerOperationMethod.Patch);
    }
}