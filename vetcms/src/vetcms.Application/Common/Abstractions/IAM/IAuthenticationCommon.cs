using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Domain.Entity;

namespace vetcms.ServerApplication.Common.Abstractions.IAM
{
    /// <summary>
    /// Meghatározza a JWT tokenek generálására és érvényesítésére, valamint a tokenekből származó felhasználói információk lekérésére szolgáló metódusokat.
    /// </summary>
    public interface IAuthenticationCommon
    {
        /// <summary>
        /// JWT tokent generál egy adott felhasználó számára opcionális lejárati dátummal.
        /// </summary>
        /// <param name="user">A felhasználó, akinek a token generálva lesz.</param>
        /// <param name="expirationDate">A token opcionális lejárati dátuma. Alapértelmezés szerint 7 nap, ha nincs megadva.</param>
        /// <returns>A generált JWT token string formátumban.</returns>
        string GenerateAccessToken(User user, DateTime expirationDate = default);

        /// <summary>
        /// Érvényesíti a megadott JWT tokent.
        /// </summary>
        /// <param name="token">Az érvényesítendő JWT token.</param>
        /// <returns>Boolean érték, amely jelzi, hogy a token érvényes-e vagy sem.</returns>
        Task<bool> ValidateToken(string token);

        /// <summary>
        /// Lekéri a megadott JWT tokenhez társított felhasználót.
        /// </summary>
        /// <param name="token">A JWT token, amelyből a felhasználó lekérhető.</param>
        /// <returns>A megadott JWT tokenhez társított felhasználó.</returns>
        Task<User> GetUser(string token);
    }
}
