
namespace MVVMHelpersDemo
{
    public class AuthParameters
    {
        // Azure AD B2C Parametros.
        public static string Tenant = "{tenant}.onmicrosoft.com";
        public static string ClientID = "ApplicationID";
        public static string PolicySignIn = "B2C_1_SiIn"; // 
		public static string PolicySignUpSignIn = "B2C_1_SiUpIn";
        public static string PolicyEditProfile = "B2C_1_ProEdit";
        public static string PolicyResetPassword = "B2C_1_ResetPwd";

        public static string DefaultPolicy = PolicySignUpSignIn;

        // Web App Scope.
        public static string WebIdentifier = "https://{tenant}.onmicrosoft.com/auth";

		// API Scopes Mobile App.
        public static string ApiIdentifier = "https://{tenant}.onmicrosoft.com/mobile";
        public static string ReadScope = $"{ApiIdentifier}/read";
        public static string WriteScope = $"{ApiIdentifier}/write";
        public static string[] ApiScopes = { ReadScope, WriteScope };

		public static string[] Scopes = { ReadScope };

        public static string AuthorityBase = $"https://login.microsoftonline.com/tfp/{Tenant}/";
		public static string Authority = $"{AuthorityBase}{DefaultPolicy}";
        public static string AuthorityEditProfile = $"{AuthorityBase}{PolicyEditProfile}";
        public static string AuthorityPasswordReset = $"{AuthorityBase}{PolicyResetPassword}";
    }
}
