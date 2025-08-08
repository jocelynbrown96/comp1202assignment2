namespace Assignment_2;

class Program
{
    // Declare filename for videogame data and get list from existing file
    static string FileName = "VideoGames.txt";
    static List<Game> VideoGames = GetGameList();

    static void Main() // Main menu logic 
    {
        string? input;
        do
        {
            Console.WriteLine("\n====== Video Game Inventory Menu ======");
            Console.WriteLine();
            Console.WriteLine("1. Show All Games");
            Console.WriteLine("2. Add New Game");
            Console.WriteLine("3. Search by Item Number");
            Console.WriteLine("4. Search by Maximum Price");
            Console.WriteLine("5. Perform Price Analysis");
            Console.WriteLine("6. Exit");
            Console.WriteLine();
            Console.Write("Enter your choice: ");

            input = Console.ReadLine();

            if (input == "1")
            {
                Console.Clear();
                Console.WriteLine("\n====== ALL GAMES ======");
                Console.WriteLine();
                foreach (var game in VideoGames)
                    Console.WriteLine(game.ListGame());
                Console.WriteLine();
                Console.Write("Press any key to go back to Main menu ");
                Console.ReadLine();
                Console.Clear();
                
            }
            else if (input == "2")
            {
                AddGame();
            }
            else if (input == "3")
            {
                FindGameByItemNumber();
            }
            else if (input == "4")
            {
                FindGameByMaximumPrice();
            }
            else if (input == "5")
            {
                PerformStatisticalAnalysis();
            }
            else if (input != "6")
            {
                Console.WriteLine("Invalid choice, please try again.");
            }

        } while (input != "6");

        Console.WriteLine("Goodbye!");
    }
    // Function to get game list from file
    static List<Game> GetGameList()
    {
        List<Game> videoGames = new List<Game>();
        try
        {
            StreamReader videoGamesFile = new StreamReader(FileName);
            // Iterate through lines in file to map games to game objects
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
    
    // Function to get list of item numbers for all games
    static List<int> GetItemNumbers()
    {
        List<int> itemNumbers = new List<int>();
        for (int i = 0; i < VideoGames.Count; i++)
        {
            itemNumbers.Add(VideoGames[i].GetItemNumber());
        }
        return itemNumbers;
    }

    // Function to prompt user for new game info and add game to file and list
    static void AddGame()
    {
        // Determine item number to use, provided by user or first unique number starting at 1000
        int itemNumber = 0;
        Console.Clear();
        Console.WriteLine("\n====== Add New Game ======");
        Console.WriteLine();
        Console.WriteLine("Please create a 4 digit item number");
        Console.WriteLine();
        Console.WriteLine("OR");
        Console.WriteLine();
        Console.WriteLine("Leave blank to auto generate a number");
        Console.WriteLine();
        Console.Write("Item Number: ");
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
            // Input validation
            else if (!Int32.TryParse(input, out itemNumber) || itemNumber < 1 || itemNumber > 9999)
            {
                Console.WriteLine("Invalid input, please enter a number between 1 and 9999");
                input = Console.ReadLine();
                itemNumber = 0;
            }
            else if (itemNumbers.Contains(itemNumber))
            {
                Console.WriteLine("Item number already exists, please enter another item number");
                input = Console.ReadLine();
                itemNumber = 0;
            }
        }
        Console.Clear();
        
        // Prompts for game name, price, rating and quantity with input validation
        Console.WriteLine("\n====== Add New Game ======");
        Console.WriteLine();
        Console.WriteLine("Please enter the game name");
        Console.WriteLine();
        Console.Write("Name: ");
        
        string? itemName = Console.ReadLine();
        while (String.IsNullOrEmpty(itemName))
        {
            Console.WriteLine("Input is blank. Please enter the game name");
            itemName = Console.ReadLine();
        }

        int price;
        Console.Clear();
        Console.WriteLine("\n====== Add New Game ======");
        Console.WriteLine();
        Console.WriteLine("Please enter the game price");
        Console.WriteLine();
        Console.Write("Price: ");
        input = Console.ReadLine();
        while (!Int32.TryParse(input, out price) || price < 1)
        {
            Console.WriteLine("Invalid input, please enter a positive integer for the game price");
            input = Console.ReadLine();
        }

        int userRating;
        Console.Clear();
        Console.WriteLine("\n====== Add New Game ======");
        Console.WriteLine();
        Console.WriteLine("Please enter the game rating (1-5)");
        Console.WriteLine();
        Console.Write("rating: ");
        input = Console.ReadLine();
        while (!Int32.TryParse(input, out userRating) || userRating < 1 || userRating > 5)
        {
            Console.WriteLine("Invalid input, please enter a number between 1 and 5");
            input = Console.ReadLine();
        }

        int quantity;
        Console.Clear();
        Console.WriteLine("\n====== Add New Game ======");
        Console.WriteLine();
        Console.WriteLine("Please enter the quantity in stock");
        Console.WriteLine();
        Console.Write("Stock: ");
        input = Console.ReadLine();
        Console.Clear();
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
        Console.Clear();
        Console.WriteLine("\n====== Search by Item Number ======");
        Console.WriteLine();
        Console.WriteLine("Please enter the item number of the game you are looking for: ");
        Console.WriteLine();
        Console.Write("Number: ");
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
            //Obtain the game at the current index in the array or list.
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
            Console.Clear();
            Console.WriteLine("\n====== Search by Item Number ======");
            Console.WriteLine();
            Console.WriteLine("Game found!");
            Console.WriteLine();
            Console.WriteLine(foundGame.ListGame()); //Display the game's information.
            Console.WriteLine();
            Console.Write("Press any key to go back to Main menu ");
            Console.ReadLine();
            Console.Clear();
            
        }
        else
        {
            //If no game was found, inform the user of this.
            Console.Clear();
            Console.WriteLine("\n====== Search by Item Number ======");
            Console.WriteLine();
            Console.WriteLine($"No game has been found with the item number {searchItemNumber}.");
            Console.WriteLine();
            Console.Write("Press any key to go back to main menu ");
            Console.ReadLine();
            Console.Clear();
            
            
        }
    }

    //This method allows the user to search for games based on their maximum price.
    static void FindGameByMaximumPrice()
    {
        //Prompt the user to enter a maximum price.
        Console.Clear();
        Console.WriteLine("\n====== Search by max price ======");
        Console.WriteLine();
        Console.WriteLine("Please enter the maximum price to search for: ");
        Console.WriteLine();
        Console.Write("Price: ");
        //Read user input from the console. A nullable string is returned, meaning it may return null if no input is received.
        string? input = Console.ReadLine();
        Console.Clear();

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
            Console.Clear();
            Console.WriteLine("\n====== Search by max price ======");
            Console.WriteLine();
            Game game = VideoGames[i]; //Obtain the game at the current index in the array or list.
            Console.WriteLine();
            Console.Write("Press any key to go back to main menu ");
            Console.ReadLine();
            Console.Clear();
            

            //If the game's price is less than or equal to the entered max price.
            if (game.GetPrice() <= maxPrice)
            {
                Console.Clear();
                Console.WriteLine("\n====== Search by max price ======");
                Console.WriteLine();
                Console.WriteLine(game.ListGame()); //Display the game to the user.
                gameFound = true; //Variable to indicate that a matching game has been found.
                Console.WriteLine();
                Console.Write("Press any key to go back to main menu ");
                Console.ReadLine();
                Console.Clear();
            }
        }
        //If no matching games were found, inform the user of this.
        if (!gameFound)
        {
            Console.Clear();
            Console.WriteLine("\n====== Search by max price ======");
            Console.WriteLine();
            Console.WriteLine($"No games were found with the price equal to or less than ${maxPrice}.");
            Console.WriteLine();
            Console.Write("Press any key to go back to main menu ");
            Console.ReadLine();
            Console.Clear();
        }
    }

    //This method will allow the user to perform a statistical analysis of the price of items currently in stock.
    static void PerformStatisticalAnalysis() 
    {
        //Check if the inventory is currently empty.
        if (VideoGames.Count == 0)
        {
            //If so, display an error message and exit the method.
            Console.WriteLine("There are no games in the inventory to analyze!");
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
        Console.Clear();
        Console.WriteLine("\n====== Price Statistics ======");
        Console.WriteLine();
        Console.WriteLine($"Mean Price: ${meanPrice:F2}"); //Show the average price rounded to two decimal places.
        Console.WriteLine($"Price Range: ${priceRange}"); //Show the difference between the highest priced game and the lowest price game.
        Console.WriteLine($"Highest Price: ${highestPriceGame.GetItemName()} - ${maximumPrice}"); //Show the game with the highest price.
        Console.WriteLine($"Lowest Price: ${lowestPriceGame.GetItemName()} - ${minimumPrice}"); //Show the game with the lowest price.
        Console.WriteLine();
        Console.Write("Press any key to go back to main menu ");
        Console.ReadLine();
        Console.Clear();
    }

    // Create video game class
    internal class Game
    {
        private int ItemNumber;
        private string ItemName;
        private int Price;
        private int UserRating;
        private int Quantity;
        
        // Constructor
        public Game(int itemNumber, string itemName, int price, int userRating, int quantity)
        {
            ItemNumber = itemNumber;
            ItemName = itemName;
            Price = price;
            UserRating = userRating;
            Quantity = quantity;
        }

        // Get data for video game to add to file
        public override string ToString()
        {
            return ItemNumber + "," + ItemName + "," + Price + "," + UserRating + "," + Quantity;
        }
        // Get data for video game in human readable form
        public string ListGame()
        {
            return ItemNumber + ": " + ItemName + " - $" + Price + ", " + UserRating + " Stars, " + Quantity + " In Stock";
        }

        // Get values from each variable
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

