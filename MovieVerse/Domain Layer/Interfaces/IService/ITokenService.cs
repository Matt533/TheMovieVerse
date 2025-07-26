using MovieVerse.Domain_Layer.Models;

namespace MovieVerse.Domain_Layer.Interfaces.IService
{
    public interface ITokenService
    {
        public string CreateToken(User user);
    }
}
