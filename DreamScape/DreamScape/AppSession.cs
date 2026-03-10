using DreamScape.Data;

namespace DreamScape
{
    internal static class AppSession
    {
        public static User? CurrentUser { get; set; }

        public static bool IsAdmin => CurrentUser?.Role?.Name == "Admin";
    }
}
