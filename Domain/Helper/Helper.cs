using Domain.Common;
using Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using SysEnum = System.Enum;

namespace Domain.Helper;

public class Helper
{
    public static IQueryable<EnumDto> GetEnumDisplayNames<T>() where T : Enum
    {
        try
        {
            var data = SysEnum.GetValues(typeof(T)).Cast<T>()
                .Select(x => new EnumDto
                {
                    Id = Convert.ToInt32(x),
                    Name = x.ToString(),
                    DisplayName = GetEnumDisplayName(x)
                })
                .AsQueryable();

            return data;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static string GetEnumDisplayName<T>(int value) where T : Enum
    {
        try
        {
            var enumValue = SysEnum.GetValues(typeof(T)).Cast<T>()
                .FirstOrDefault(x => Convert.ToInt32(x) == value);

            if (enumValue is not null)
                return GetEnumDisplayName(enumValue);
            return string.Empty;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private static string GetEnumDisplayName(Enum value)
    {
        FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());
        DisplayAttribute? displayAttribute;
        if (fieldInfo is not null)
        {
            displayAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DisplayAttribute)) as DisplayAttribute;
            return displayAttribute?.Name ?? value.ToString();
        }
        else
            return string.Empty;
    }

    public static T CreateErrorResult<T>(HttpStatusCode statusCode, string errorMessage) where T : OperationResult, new()
    {
        return new T
        {
            StatusCode = statusCode,
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}
