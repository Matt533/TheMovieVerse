namespace MovieVerse.Domain_Layer.Exceptions
{
    public class UsernameExistsException : Exception
    {
        public UsernameExistsException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
