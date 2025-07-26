namespace MovieVerse.Domain_Layer.Exceptions
{
    public class UserUnauthorizedException : Exception
    {
        public UserUnauthorizedException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
