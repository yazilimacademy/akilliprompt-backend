using System;

namespace AkilliPrompt.WebApi.Helpers;

public static class CacheKeysHelper
{
    public static string GetAllCategoriesKey => "GetAllCategories";
    public static string GetByIdCategoryKey(Guid id) => $"GetByIdCategory:{id}";
}
