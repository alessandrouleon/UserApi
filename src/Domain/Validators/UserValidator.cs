namespace UserApin.Validators;

public static class UserValidator
{
    public static IReadOnlyList<string> ValidateForCreation(string name, string email)
    {
        var errors = new List<string>();

        ValidateName(name, errors);
        ValidateEmail(email, errors);

        return errors;
    }

    public static IReadOnlyList<string> ValidateForUpdate(string name, string email)
        => ValidateForCreation(name, email);

    private static void ValidateName(string name, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("Name is required.");
            return;
        }

        if (name.Trim().Length < 2)
            errors.Add("Name must be at least 2 characters long.");

        if (name.Trim().Length > 100)
            errors.Add("Name must not exceed 100 characters.");
    }

    private static void ValidateEmail(string email, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add("Email is required.");
            return;
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email.Trim())
                errors.Add("Email format is invalid.");
        }
        catch
        {
            errors.Add("Email format is invalid.");
        }
    }
}
