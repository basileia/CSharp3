public class FizzBuzz
{
    public void CountTo(int lastNumber)
    {
        for (int i = 1; i <= lastNumber; i++)
        {
            if (i % 3 == 0 && i % 5 == 0)
            {
                Console.WriteLine("FizzBuzz");
            }

            else if (i % 3 == 0)
            {
                Console.WriteLine("Fizz");
            }

            else if (i % 5 == 0)
            {
                Console.WriteLine("Buzz");
            }

            else
            {
                Console.WriteLine(i);
            }
        }
    }
}
