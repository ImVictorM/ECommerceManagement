using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection {

  public static IServiceCollection AddApplication(this IServiceCollection services) {
    var assembly = typeof(DependencyInjection).Assembly;

    services
      .AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly))
      .AddValidatorsFromAssembly(assembly);

    return services;
  }
}