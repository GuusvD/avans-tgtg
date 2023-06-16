namespace Portal.Extensions
{
    public static class MealTypeEnumExtension
    {
        public static TAttribute GetAttribute<TAttribute>(this MealTypeEnum MealTypeEnum) where TAttribute : Attribute
        {
            return MealTypeEnum.GetType().GetMember(MealTypeEnum.ToString()).First().GetCustomAttribute<TAttribute>()!;
        }
    }
}
