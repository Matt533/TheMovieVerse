namespace MovieVerse.Domain_Layer.Exceptions
{
    public class PasswordDontMatchException : Exception
    {
        public PasswordDontMatchException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
