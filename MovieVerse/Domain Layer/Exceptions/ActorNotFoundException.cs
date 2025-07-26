namespace MovieVerse.Domain_Layer.Exceptions
{
    public class ActorNotFoundException : Exception
    {
        public ActorNotFoundException(string message)
        {
            Console.WriteLine(message);
        }
    }
}
