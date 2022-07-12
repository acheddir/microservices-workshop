// Global using directives

global using System.Diagnostics;
global using FluentValidation;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;
global using Serilog.Context;
global using SharedKernel.Application.Common;
global using SharedKernel.Application.Common.Services;
global using SharedKernel.EventBus.Events;
global using SharedKernel.Extensions;
global using SharedKernel.Infrastructure.Common;
global using ValidationException = SharedKernel.Domain.Exceptions.ValidationException;