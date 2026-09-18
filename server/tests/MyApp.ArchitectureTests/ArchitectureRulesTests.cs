using System.Reflection;
using FluentAssertions;
using Xunit;

namespace MyApp.ArchitectureTests;

public sealed class ArchitectureRulesTests
{
    [Fact]
    public void HandlersShouldBeSealed()
    {
        var assembly = typeof(Program).Assembly;

        var handlers = assembly
            .GetTypes()
            .Where(type => type.Name.EndsWith("Handler", StringComparison.Ordinal))
            .ToList();

        handlers.Should().NotBeEmpty();
        handlers.Should().OnlyContain(type => type.IsSealed);
    }

    [Fact]
    public void FeaturesShouldNotReferenceOtherFeatureNamespaces()
    {
        var assembly = typeof(Program).Assembly;
        var featureTypes = assembly
            .GetTypes()
            .Where(type => type.Namespace is not null
                && type.Namespace.StartsWith("MyApp.Features.", StringComparison.Ordinal)
                && !type.Namespace.EndsWith(".Shared", StringComparison.Ordinal))
            .ToList();

        foreach (var type in featureTypes)
        {
            var referencedNamespaces = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(field => field.FieldType.Namespace)
                .Concat(type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .Select(property => property.PropertyType.Namespace))
                .Where(ns => ns is not null)
                .Select(ns => ns!)
                .ToHashSet();

            var featureRoot = type.Namespace!.Split('.', StringSplitOptions.RemoveEmptyEntries)[2];

            var forbiddenNamespaces = referencedNamespaces
                .Where(ns => ns.StartsWith("MyApp.Features.", StringComparison.Ordinal))
                .Where(ns =>
                {
                    var referencedFeatureRoot = ns.Split('.', StringSplitOptions.RemoveEmptyEntries)[2];
                    return referencedFeatureRoot != featureRoot;
                })
                .ToList();

            forbiddenNamespaces.Should().BeEmpty();
        }
    }
}
