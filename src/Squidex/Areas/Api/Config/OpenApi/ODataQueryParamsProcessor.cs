﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschränkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using Squidex.Pipeline.OpenApi;

namespace Squidex.Areas.Api.Config.OpenApi
{
    public sealed class ODataQueryParamsProcessor : IOperationProcessor
    {
        private readonly string supportedPath;
        private readonly string entity;
        private readonly bool supportSearch;

        public ODataQueryParamsProcessor(string supportedPath, string entity, bool supportSearch)
        {
            this.entity = entity;

            this.supportSearch = supportSearch;
            this.supportedPath = supportedPath;
        }

        public bool Process(OperationProcessorContext context)
        {
            if (context.OperationDescription.Path == supportedPath)
            {
                var operation = context.OperationDescription.Operation;

                operation.AddOData(entity, supportSearch);
            }

            return true;
        }
    }
}
