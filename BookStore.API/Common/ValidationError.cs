namespace BookStore.API.Common
{
    public record ValidationError
    (
        string Field,
        string[] Errors
    );
}
/*
 {
    "field": "email",
    "errors": [
        "The email field is required.",
        "The email field must be a valid email address."
    ]
}
 */