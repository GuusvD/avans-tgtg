namespace Core.Domain.Enumerations
{
    public enum MealTypeEnum
    {
        [Display(Name = "Brood")]
        Bread,
        [Display(Name = "Drank")]
        Drink,
        [Display(Name = "Fruit")]
        Fruit,
        [Display(Name = "Ontbijt")]
        Breakfast,
        [Display(Name = "Lunch")]
        Lunch,
        [Display(Name = "Avondmaaltijd")]
        Dinner,
        [Display(Name = "Warme avondmaaltijd")]
        WarmDinner
    }
}
