// This isn't how I normally comment code, but I will comment this to explain my thought processes, and the decisions I make. 

// Because this is a small application simple is best. Let's do our best to be simple, and fast-ish. 
// Benchmarking this seems pointless so I'll just do my best.

// We really don't care about the numbers, so let's just use strings. It'll be what we're comparing anyway.
Random random = new Random();

char[] GetRandomNumbers(int length)
{
    const string possibleChars = "123456";

    var resultChars = new char[length];

    // We could make this generator using LINQ, but I don't think it's faster (I am not going to test that) and it's harder to read, and think about.
    for(int i = 0; i < length; i++)
    {
        resultChars[i] = possibleChars[random.Next(possibleChars.Length)];
    }

    return resultChars;
}

// I prefer enums for anything that has more than 2 states.
static MatchResult CheckDigit(char digit, int index, char[] answer)
{
    var hasMatch = false;
    for(int i = 0; i < answer.Length; i++)
    {
        if (digit == answer[i])
        {
            if(i == index)
            {
                return MatchResult.MatchAndIndex;
            }

            hasMatch = true;
        }

        // I can early exit here on has match (If I am beyond index), but honestly it's more code that might be slower. Again I'm not testing, but it seems better to keep it simple in this case
    }

    if(hasMatch)
    {
        return MatchResult.Match;
    }
    else
    {
        return MatchResult.Nothing;
    }
}

// Let's be nice and let the user replay without re-opening the program.
var playing = true;
Console.WriteLine("Welcome to mastermind!");

while(playing)
{
    Console.WriteLine("I'm thinking of a number...");
    var answer = GetRandomNumbers(4);

    Console.WriteLine("Take a guess!");

    // Let's use a while loop here, because it's easier to restart a loop without increasing a counter on an invalid attempt.
    var attempt = 0;
    while(attempt < 10)
    {
        var userGuess = Console.ReadLine();

        // Validate our inputs! If any character is not within (30, 37) ASCII then it's not valid!
        if (userGuess?.Length != 4 || userGuess.Any(x => x < '1' || x > '6'))
        {
            Console.WriteLine("Invalid guess! please make sure you guess 4 characters and that each character is between the numbers 1 and 6!");
            Console.WriteLine("Try again!");
            continue;
        }

        attempt++;

        var plusCount = 0;
        var minusCount = 0;
        // Ideally we only iterate over both userGuess and Answer once...
        // however I'm pretty sure using a map in this case is slower due to our array size being so small.
        for (int i = 0; i < userGuess.Length; i++)
        {
            // We use a function here because it's easier to handle than nested for loops.
            var result = CheckDigit(userGuess[i], i, answer);
            switch (result) {
                case MatchResult.MatchAndIndex:
                    plusCount++;
                    break;
                case MatchResult.Match: 
                    minusCount++; 
                    break;
                default:
                    break;
            }
        }

        if(plusCount == 4)
        {
            Console.WriteLine("++++");
            Console.WriteLine("You guessed it right!");
            break;
        }


        Console.WriteLine(new string('+', plusCount) + new string('-', minusCount));
        if(attempt < 10)
        {
            Console.WriteLine("Try again!");
        } 
        else
        {
            Console.WriteLine($"Better luck next time! The correct answer was {new string(answer)}");
        }
    }

    Console.WriteLine("Would you like to play again? (Y/N)");
    var playAgain = Console.ReadLine()?.ToLower().StartsWith("y");

    if(!(playAgain ?? false))
    {
        Console.WriteLine("Goodbye!");
        playing = false;
    }
}

// Keep the console open.
Console.ReadLine();

enum MatchResult
{
    MatchAndIndex,
    Match,
    Nothing
}