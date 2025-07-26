namespace MovieVerse.Domain_Layer.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
