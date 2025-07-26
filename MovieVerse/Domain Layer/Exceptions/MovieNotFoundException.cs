namespace MovieVerse.Infrastructional_Layer.Exceptions
{
    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException(string message) 
        {
            Console.WriteLine(message);
        }
    }
}
