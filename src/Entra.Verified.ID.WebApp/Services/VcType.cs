namespace Portal.VerifiableCredentials.API.Services;

public static class VcType
{
    public static bool IsBaseIssuance(string type)
    {
        return type != null && (type.Contains("basic") || type.Contains("b2c"));
    }
    public static bool IsB2C(string type)
    {
        return type != null && (type.Contains("b2c"));
    }
    
    public static bool IsProductIssuance(string type, string requestType)
    {
        return type != null && (type.Contains("product-generator")&& requestType.Contains("issuance"));
    }
    public static bool IsProductPresentation(string type, string requestType)
    {
        return type != null && (type.Contains("product-presentation")&& requestType.Contains("presentation"));
    }
    public static bool IsBankAccountIssuance(string type, string requestType)
    {
        return type != null && (type.Contains("bank-create"));
    }
    public static bool IsBankPay(string type, string requestType)
    {
        return type != null && (type.Contains("bank-pay"));
    }
    public static bool IsBankIdentity(string type, string requestType)
    {
        return type != null && (type.Contains("identity"));
    }
    public static bool IsIngredientIssuance(string type)
    {
        return type != null && type.Contains("ingr");
    }
    public static bool IsFace(string type)
    {
        return type != null && type.Contains("face");
    }

    public static string PresentationType => "presentation";
    public static string IssuanceType => "issuance";
}