﻿// ==========================================================================
//  EventConsumersController.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Orleans;
using Squidex.Areas.Api.Controllers.EventConsumers.Models;
using Squidex.Infrastructure.CQRS.Commands;
using Squidex.Infrastructure.CQRS.Events.Orleans.Grains;
using Squidex.Infrastructure.Reflection;
using Squidex.Pipeline;

namespace Squidex.Areas.Api.Controllers.EventConsumers
{
    [ApiAuthorize]
    [ApiExceptionFilter]
    [MustBeAdministrator]
    [SwaggerIgnore]
    public sealed class EventConsumersController : ApiController
    {
        private readonly IEventConsumerRegistryGrain eventConsumerRegistryGrain;

        public EventConsumersController(ICommandBus commandBus, IClusterClient orleans)
            : base(commandBus)
        {
            eventConsumerRegistryGrain = orleans.GetGrain<IEventConsumerRegistryGrain>("Default");
        }

        [HttpGet]
        [Route("event-consumers/")]
        [ApiCosts(0)]
        public async Task<IActionResult> GetEventConsumers()
        {
            var entities = await eventConsumerRegistryGrain.GetConsumersAsync();

            var models = entities.Value.Select(x => SimpleMapper.Map(x, new EventConsumerDto())).ToList();

            return Ok(models);
        }

        [HttpPut]
        [Route("event-consumers/{name}/start/")]
        [ApiCosts(0)]
        public async Task<IActionResult> Start(string name)
        {
            await eventConsumerRegistryGrain.StartAsync(name);

            return NoContent();
        }

        [HttpPut]
        [Route("event-consumers/{name}/stop/")]
        [ApiCosts(0)]
        public async Task<IActionResult> Stop(string name)
        {
            await eventConsumerRegistryGrain.StopAsync(name);

            return NoContent();
        }

        [HttpPut]
        [Route("event-consumers/{name}/reset/")]
        [ApiCosts(0)]
        public async Task<IActionResult> Reset(string name)
        {
            await eventConsumerRegistryGrain.ResetAsync(name);

            return NoContent();
        }
    }
}
