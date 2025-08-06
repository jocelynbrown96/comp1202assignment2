namespace Assignment_2;

class Program
{
    static string FileName = "VideoGames.txt";
    static List<Game> VideoGames = GetGameList();

    static void Main()
    {
        //Show all of the current games.
        for (int i = 0; i < VideoGames.Count; i++)
        {
            Console.WriteLine(VideoGames[i].ListGame());
        }

        //Add a new game.
        AddGame();

        //Search for games by item number.
        FindGameByItemNumber();

        //Search for games by maximum price.
        FindGameByMaximumPrice();

        //Perform statistical analysis of the price of the items in stock.
        PerformStatisticalAnalysis();
    }

    static List<Game> GetGameList()
    {
        List<Game> videoGames = new List<Game>();
        try
        {
            StreamReader videoGamesFile = new StreamReader(FileName);
            string? gameString = videoGamesFile.ReadLine();
            while (gameString != null)
            {
                string[] gameArray = gameString.Split(',');
                videoGames.Add(new Game(
                    Convert.ToInt32(gameArray[0]),
                    gameArray[1],
                    Convert.ToInt32(gameArray[2]),
                    Convert.ToInt32(gameArray[3]),
                    Convert.ToInt32(gameArray[4])
                ));
                gameString = videoGamesFile.ReadLine();
            }
            videoGamesFile.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return videoGames;
    }

    static List<int> GetItemNumbers()
    {
        List<int> itemNumbers = new List<int>();
        for (int i = 0; i < VideoGames.Count; i++)
        {
            itemNumbers.Add(VideoGames[i].GetItemNumber());
        }
        return itemNumbers;
    }

    static void AddGame()
    {
        int itemNumber = 0;
        Console.WriteLine("Please enter an item number or leave blank to generate one");
        string? input = Console.ReadLine();
        while (itemNumber == 0)
        {
            List<int> itemNumbers = GetItemNumbers();
            if (input == "")
            {
                itemNumber = 1000;
                while (itemNumbers.Contains(itemNumber))
                {
                    itemNumber++;
                }
            }
            else if (!Int32.TryParse(input, out itemNumber) || itemNumber < 1 || itemNumber > 9999)
            {
                Console.WriteLine("Invalid input, please enter a number between 1 and 9999");
                input = Console.ReadLine();
                itemNumber = 0;
            }
            else if (itemNumbers.Contains(Convert.ToInt32(input)))
            {
                Console.WriteLine("Item number already exists, please enter another item number");
                input = Console.ReadLine();
                itemNumber = 0;
            }
        }

        Console.WriteLine("Please enter the game name");
        string? itemName = Console.ReadLine();
        while (String.IsNullOrEmpty(itemName))
        {
            Console.WriteLine("Input is blank. Please enter the game name");
            itemName = Console.ReadLine();
        }

        int price;
        Console.WriteLine("Please enter the game price");
        input = Console.ReadLine();
        while (!Int32.TryParse(input, out price) || price < 1)
        {
            Console.WriteLine("Invalid input, please enter a positive integer for the game price");
            input = Console.ReadLine();
        }

        int userRating;
        Console.WriteLine("Please enter the game rating (1-5)");
        input = Console.ReadLine();
        while (!Int32.TryParse(input, out userRating) || userRating < 1 || userRating > 5)
        {
            Console.WriteLine("Invalid input, please enter a number between 1 and 5");
            input = Console.ReadLine();
        }

        int quantity;
        Console.WriteLine("Please enter the quantity in stock");
        input = Console.ReadLine();
        while (!Int32.TryParse(input, out quantity) || quantity < 1)
        {
            Console.WriteLine("Invalid input, please enter a positive integer for the quantity");
            input = Console.ReadLine();
        }
        Game newGame = new Game(itemNumber, itemName, price, userRating, quantity);
        VideoGames.Add(newGame);
        try
        {
            File.AppendAllText(FileName, "\n" + newGame.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    //This method allows the user to search for a specified game by its item number.
    static void FindGameByItemNumber()
    {
        //Prompt the user to enter an item number.
        Console.WriteLine("Please enter the item number of the game you are looking for: ");
        //Read user input from the console. A nullable string is returned, meaning it may return null if no input is received.
        string? input = Console.ReadLine();

        //Attempt to convert the input to an integer (item number).
        if (!int.TryParse(input, out int searchItemNumber))
        {
            //If the conversion fails, display an error message and exit the method.
            Console.WriteLine("Invalid input! Please enter a valid number: ");
            return;
        }

        //Create a variable to store the game that is found (initially a null value).
        Game? foundGame = null;

        //Loop through all of the games in the inventory.
        for (int i = 0; i < VideoGames.Count; i++)
        {
            //Obtain the current game.
            Game currentGame = VideoGames[i];

            //Check if the current game's item number matches the item number entered by the user.
            if (currentGame.GetItemNumber() == searchItemNumber)
            {
                foundGame = currentGame; //Store the matching game.
                break; //End the loop since the match has been found.
            }
        }
        //If a matching game was found, display its details.
        if (foundGame != null)
        {
            Console.WriteLine("Game found!: ");
            Console.WriteLine(foundGame.ListGame()); //Print the game's information.
        }
        else
        {
            //If no game was found, inform the user.
            Console.WriteLine($"No game has been found with the item number {searchItemNumber}.");
        }
    }

    //This method allows the user to search for games based on their maximum price.
    static void FindGameByMaximumPrice()
    {
        //Prompt the user to enter a maximum price.
        Console.WriteLine("Please enter the maximum price to search for: ");
        //Read user input from the console. A nullable string is returned, meaning it may return null if no input is received.
        string? input = Console.ReadLine();

        //Attempt converting the input to an integer (maximum price).
        if (!int.TryParse(input, out int maxPrice) || maxPrice < 0)
        {
            //If the conversion fails or the price is negative, display an error message and exit the method.
            Console.WriteLine("Invalid input! Please enter a positive number.");
            return;
        }

        bool gameFound = false; //Variable to check if at least one game matches the search condition.

        //Loop through all of the games present in the inventory.
        for (int i = 0; i < VideoGames.Count; i++)
        {
            Game game = VideoGames[i];

            //If the game's price is less than or equal to the entered max price.
            if (game.GetPrice() <= maxPrice)
            {
                Console.WriteLine(game.ListGame()); //Display the game to the user.
                gameFound = true;
            }
        }
        //If no matching games were found, inform the user of this.
        if (!gameFound)
        {
            Console.WriteLine($"No games were found with the price equal to or less than ${maxPrice}.");
        }
    }

    //This method will allow the user to perform a statistical analysis of the price of items currently in stock.
    static void PerformStatisticalAnalysis() 
    {
        //Check if the inventory is currently empty.
        if (VideoGames.Count == 0)
        {
            //If so, display an error message and exit the method.
            Console.WriteLine("There are no games in the inventory to analayze!");
            return;
        }

        int totalPrice = 0; //Variable to keep track of the total price of all of the games present in the inventory.
        int minimumPrice = VideoGames[0].GetPrice(); //Variable to store the minimum price available; begin by assuming the first game's price is the lowest.
        int maximumPrice = VideoGames[0].GetPrice(); //Variable to store the maximum price available; begin by assuming the first game's price is the highest.
        Game lowestPriceGame = VideoGames[0]; //Variable to keep reference of the game with the lowest price.
        Game highestPriceGame = VideoGames[0]; //Variable to keep reference of the game with the highest price.

        //Loop through all of the available games in the inventory to calculate the statistics.
        for (int i = 0; i < VideoGames.Count; i++)
        {
            Game game = VideoGames[i]; //Obtain the current game.
            int price = game.GetPrice(); //Get the price of the current game.
            totalPrice += price; //Add this price to the total.

            //Check if this game's price is lower than the current minimum.
            if (price < minimumPrice) 
            { 
                minimumPrice = price; //Update the minimum price.
                lowestPriceGame = game; //Store this as the game with the new lowest price.
            }

            //Check if this game's price is higher than the current maximum.
            if (price > maximumPrice) 
            {
                maximumPrice = price; //Update the maximum price.
                highestPriceGame = game; //Store this as the game with the new highest price.
            }
        }

        //Calculate the mean (average) price of all games by dividing the total by the number of games present.
        double meanPrice = (double)totalPrice/VideoGames.Count;
        //Calculate the price range by subtracting the game with the lowest price from the game with the highest price.
        int priceRange = maximumPrice - minimumPrice;

        //Display the calculated statistics to the user.
        Console.WriteLine("\n----- Price Statistics: -----");
        Console.WriteLine($"Mean Price: ${meanPrice:F2}"); //Show the average price rounded to two decimal places.
        Console.WriteLine($"Price Range: ${priceRange}"); //Show the difference between the highest priced game and the lowest price game.
        Console.WriteLine($"Highest Price: ${highestPriceGame.GetItemName()} - ${maximumPrice}"); //Show the game with the highest price.
        Console.WriteLine($"Lowest Price: ${lowestPriceGame.GetItemName()} - ${minimumPrice}"); //Show the game with the lowest price.
    }

    internal class Game
    {
        private int ItemNumber;
        private string ItemName;
        private int Price;
        private int UserRating;
        private int Quantity;

        public Game(int itemNumber, string itemName, int price, int userRating, int quantity)
        {
            ItemNumber = itemNumber;
            ItemName = itemName;
            Price = price;
            UserRating = userRating;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return ItemNumber + "," + ItemName + "," + Price + "," + UserRating + "," + Quantity;
        }
        public string ListGame()
        {
            return ItemNumber + ": " + ItemName + " - $" + Price + ", " + UserRating + " Stars, " + Quantity + " In Stock";
        }

        public int GetItemNumber()
        {
            return ItemNumber;
        }
        public string GetItemName()
        {
            return ItemName;
        }
        public int GetPrice()
        {
            return Price;
        }
        public int GetUserRating()
        {
            return UserRating;
        }
        public int GetQuantity()
        {
            return Quantity;
        }
    }
}