using Duende.IdentityServer.Models;

namespace FlottaIdentity
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
new List<IdentityResource>
{
    new IdentityResources.OpenId(),
    new IdentityResources.Profile()
};
        // Milyen API hatókörök (jogosultságok) léteznek
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
        new ApiScope("FlottaApi", "Saját Web API elérése", new[] { "role", "name" })
            };

        // Milyen kliens alkalmazások kérhetnek tokent
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {

                new Client
                {
                    ClientId = "wpf_kliens",
                    // Engedélyezzük a felhasználónév + jelszó alapú belépést:
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Duende.IdentityServer.Models.Secret("wpf_titkos_kulcs".Sha256()) },
        
                    // Milyen jogosultságokat kérhet a kliens:
                    AllowedScopes = { "openid", "profile", "FlottaApi" },
        
                    // Opcionális: Refresh token engedélyezése, hogy ne kelljen újra beírni a jelszót lejáratkor
                    AllowOfflineAccess = true
                }
            };

    }
}
