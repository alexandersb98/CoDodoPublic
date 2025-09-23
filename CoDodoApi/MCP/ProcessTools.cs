using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using CoDodoApi.Data.Repositories;
using CoDodoApi.Entities;

namespace CoDodoApi.MCP;

[McpServerToolType, 
 Description("""
             A set of tools for querying and manipulating
             the CoDodo database containing opportunities (potential assignments) 
             and processes (applications from consultants for opportunities).
             """)]
public class ProcessTools
{
    [McpServerTool, Description("Returns the schema of processes, i.e. the names and data types of attributes that all processes have, in JSON.")]
    public static string GetProcessSchema()
    {
        return GetRecordSchemaJson(typeof(Process));
    }

    [McpServerTool, Description("Queries for all processes in the CoDodo database.")]
    public static async Task<string> GetAllProcesses(IProcessRepository processRepository, string name, string opportunityUriForAssignment)
    {
        var processes = await processRepository.GetProcesses();
        return JsonSerializer.Serialize(processes);
    }

    [McpServerTool, Description("Queries for the process with a specific name and opportunity URI.")]
    public static async Task<string?> GetProcess(IProcessRepository processRepository, string name, string opportunityUriForAssignment)
    {
        var process = await processRepository.GetProcessByKey(key: (name, opportunityUriForAssignment));
        return process != null ? JsonSerializer.Serialize(process) : null;
    }

    private static string GetRecordSchemaJson(Type recordType)
    {
        var properties = recordType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var schema = new Dictionary<string, object?>
        {
            ["type"] = "object",
            ["properties"] = properties.ToDictionary(
                prop => prop.Name,
                prop => new Dictionary<string, object?>
                {
                    ["type"] = GetJsonType(prop.PropertyType),
                    ["nullable"] = IsNullable(prop)
                }
            )
        };
        return JsonSerializer.Serialize(schema, new JsonSerializerOptions { WriteIndented = true });
    }

    private static string GetJsonType(Type type)
    {
        if (type == typeof(string)) return "string";
        if (type == typeof(int) || type == typeof(long) || type == typeof(short)) return "integer";
        if (type == typeof(float) || type == typeof(double) || type == typeof(decimal)) return "number";
        if (type == typeof(bool)) return "boolean";
        if (type.IsEnum) return "string";
        if (type.IsArray || (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition())))
            return "array";
        if (type.IsClass || type.IsValueType) return "object";
        return "undetermined";
    }

    private static bool IsNullable(PropertyInfo prop)
    {
        var type = prop.PropertyType;
        if (!type.IsValueType) return true; // Reference types are nullable
        return Nullable.GetUnderlyingType(type) != null;
    }
}