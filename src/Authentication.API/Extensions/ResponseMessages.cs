namespace Authentication.API.Extensions
{
    public static class ResponseMessages
    {
        public const string INVALID_USER_CREDENTIALS = "Invalid user credentials.";
        public const string INTERNAL_SERVER_ERROR = "An internal server error has occurred, please contact our support.";
        public const string CAN_NOT_LOGIN_NOW = "You can not login now, try again later.";
        public const string PASSWORD_CAN_NOT_BE_EMPTY = "The password field can not be empty.";
        public const string INVALID_EMAIL_FORMAT = "Invalid email format.";
        public const string PASSWORD_MIN_LENGTH = "The password must be at least 8 characters long.";
        public const string PASSWORD_UPPERCASE_REQUIRED = "The password must contain at least one uppercase letter.";
        public const string PASSWORD_LOWERCASE_REQUIRED = "The password must contain at least one lowercase letter.";
        public const string PASSWORD_DIGIT_REQUIRED = "The password must contain at least one digit.";
        public const string PASSWORD_SPECIAL_REQUIRED = "The password must contain at least one special character (!@#$%^&* etc.).";
        public const string FIRSTNAME_CANNOT_BE_EMPTY = "The First Name field cannot be empty.";
        public const string EMAIL_CANNOT_BE_EMPTY = "The Email field cannot be empty.";
        public const string BIRTH_DATE_CANNOT_BE_EMPTY = "The Birth Date field cannot be empty.";
        public const string LASTNAME_CANNOT_BE_EMPTY = "The Last Name field cannot be empty.";
        public const string FIRSTNAME_LENGTH_2_TO_50 = "The First Name must be between 2 and 50 characters.";
        public const string LASTNAME_LENGTH_2_TO_50 = "The Last Name must be between 2 and 50 characters.";
        public const string INVALID_DOCUMENT = "Invalid Document.";
        public const string PASSWORDS_DO_NOT_MATCH = "The passwords do not match.";
        public const string AGE_MUST_BE_OVER_16 = "You need to be over 16 years old.";
        public const string FAIL_TO_CREATE_USER = "Failed to create user.";
        public const string USER_NOT_FOUND = "User not found.";
        public const string FAILED_TO_DELETE_USER = "Failed to delete user.";
        public const string FAILED_TO_REGISTER_CUSTOMER = "Failed to register customer.";
        public const string NAME_MUST_BE_JUST_LETTERS = "The First and Last Name must contain only letters.";
        public const string INVALID_ROLE_NAME = "Invalid Role name.";
        public const string INEXISTENT_ROLE = "Invalid role name. Must be a valid role from enum UserRoles.";
        public const string ROLE_NAME_CANNOT_BE_EMPTY = "Role name cannot be empty.";
        public const string INVALID_USER_ID = "Invalid user id.";
        public const string CLIENT_URL_TO_RESET_PASSWORD_CANNOT_BE_EMPTY = "Client Url to reset password cannot be empty.";
        public const string CLIENT_URL_TO_RESET_PASSWORD_INVALID = "Client Url to reset password is invalid.";
        public const string TOKEN_CANNOT_BE_EMPTY = "Token cannot be empty.";
        public const string FIRST_AND_LAST_NAME_MUST_BE_DIFERENT = "First and Last Name can not be equal.";
    }
}