namespace Portal.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetRole(this ClaimsPrincipal User)
        {
            bool IsStudent = User.HasClaim("Student", "Student");
            bool IsCanteenEmployee = User.HasClaim("CanteenEmployee", "CanteenEmployee");

            if (IsStudent)
            {
                return "Student";
            } else if (IsCanteenEmployee)
            {
                return "CanteenEmployee";
            } else
            {
                return "None";
            }
        }
    }
}
