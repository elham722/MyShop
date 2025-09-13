namespace MyShop.Domain.ValueObjects.Customer;
public class PhoneNumber : BaseValueObject
{
    public string Value { get; private set; } = null!;

    private PhoneNumber() { }

    public PhoneNumber(string value)
    {
        Guard.AgainstNullOrEmpty(value, nameof(value));

        if (!IsValidPhoneNumber(value))
            throw new CustomValidationException("Invalid phone number format");

        Value = NormalizePhoneNumber(value);
    }

    public static PhoneNumber Create(string value)
    {
        return new PhoneNumber(value);
    }

    private static bool IsValidPhoneNumber(string phoneNumber)
    {
        var patterns = new[]
        {
                @"^09\d{9}$",
                @"^0\d{10}$",
                @"^\+98\d{10}$",
                @"^0098\d{10}$"
            };

        return patterns.Any(pattern => Regex.IsMatch(phoneNumber, pattern));
    }

    private static string NormalizePhoneNumber(string phoneNumber)
    {

        var cleaned = Regex.Replace(phoneNumber, @"[^\d+]", "");

        if (cleaned.StartsWith("+98"))
            return cleaned;
        else if (cleaned.StartsWith("0098"))
            return "+98" + cleaned.Substring(4);
        else if (cleaned.StartsWith("09"))
            return "+98" + cleaned.Substring(1);
        else if (cleaned.StartsWith("0"))
            return "+98" + cleaned.Substring(1);
        else
            return "+98" + cleaned;
    }

    public bool IsMobile()
    {

        if (Value.StartsWith("+98"))
        {
            return Value.Length == 13 && Value[3] == '9';
        }
        else if (Value.StartsWith("09"))
        {
            return Value.Length == 11;
        }
        return false;
    }

    public bool IsLandline()
    {
        return !IsMobile();
    }

    public string GetLocalFormat()
    {
        if (Value.StartsWith("+98"))
        {
            var number = Value.Substring(3);
            return "0" + number;
        }
        return Value;
    }

    public string GetInternationalFormat()
    {
        return Value;
    }

    public string GetDisplayFormat()
    {
        if (Value.StartsWith("+98"))
        {
            var number = Value.Substring(3);
            if (number.StartsWith("9"))
            {

                return $"+98 {number.Substring(0, 4)}-{number.Substring(4, 3)}-{number.Substring(7)}";
            }
            else
            {

                return $"+98 {number.Substring(0, 3)}-{number.Substring(3)}";
            }
        }
        else if (Value.StartsWith("09"))
        {

            return $"{Value.Substring(0, 4)}-{Value.Substring(4, 3)}-{Value.Substring(7)}";
        }
        else if (Value.StartsWith("0"))
        {

            return $"{Value.Substring(0, 3)}-{Value.Substring(3)}";
        }
        return Value;
    }

    public string GetAreaCode()
    {
        if (Value.StartsWith("+98"))
        {
            var number = Value.Substring(3);
            if (number.StartsWith("9"))
            {
                return number.Substring(0, 4);
            }
            else
            {
                return number.Substring(0, 3);
            }
        }
        else if (Value.StartsWith("09"))
        {
            return Value.Substring(0, 4);
        }
        else if (Value.StartsWith("0"))
        {
            return Value.Substring(0, 3);
        }
        return string.Empty;
    }


    public bool IsSameAreaCode(PhoneNumber other)
    {
        return GetAreaCode() == other.GetAreaCode();
    }

    public PhoneNumber GetAlternativeFormat()
    {
        if (Value.StartsWith("+98"))
        {
            var number = Value.Substring(3);
            if (number.StartsWith("9"))
            {
                return new PhoneNumber("0098" + number);
            }
            else
            {
                return new PhoneNumber("0098" + number);
            }
        }
        return this;
    }


    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
}
