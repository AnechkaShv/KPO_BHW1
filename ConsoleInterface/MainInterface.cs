

using System.Threading.Channels;
using Domain.Entities;
using Domain.Patterns.DataExporter;
using Domain.Patterns.DataImporter;
using Domain.Patterns.Facades;

namespace Interface
{
    public class MainInterface
    {
        /// <summary>
        /// This method implements the main menu.
        /// </summary>
        /// <param name="zoo"></param>

        public void ShowMenu(FinanceFacade financeFacade, JsonExporter jsonExporter, CsvExporter csvExporter, CsvImporter csvImporter, JsonImporter jsonImporter)
        {
            SayHello();
            Console.WriteLine("Welcome to Bank System!\n");
            
            bool exit = false;
            while (!exit)
            {
                // Main menu.
                Menu menu = new Menu("Use up/down keys to choose menu item.",
                    new string[]
                    {
                        "1. Create account", "2. Create category", "3. Create operation", "4. Print accounts",
                        "5. Print categories", "6. Print operations", "7. Import CSV data", "8. Import Json data",
                        "9. Export CSV data", "10. Export Json data", "11. Exit"
                    });
                
                // Getting user's choice.
                int num = menu.ActMenu();
                switch (num)
                {
                    // Creating account.
                    case 1:
                        Console.WriteLine("Enter account's name: ");
                        string accountName;
                        while ((accountName = Console.ReadLine()) == null || accountName.Length == 0)
                        {
                            Console.WriteLine("Please try again");
                        }
                        Console.WriteLine("Enter initial balance: ");
                        decimal balance;
                        while (!decimal.TryParse(Console.ReadLine(), out balance))
                        {
                            Console.WriteLine("Please try again");
                        }
                        var newBankAccount = financeFacade.CreateBankAccount(accountName, balance);

                        Console.WriteLine($"The account with id {newBankAccount.Id} was successfully created.");

                        break;
                    // Creating category.
                    case 2:
                        Console.WriteLine("Enter category's name: ");
                        string categoryName;
                        while ((categoryName = Console.ReadLine()) == null || categoryName.Length == 0)
                        {
                            Console.WriteLine("Please try again");
                        }
                        
                        EntityType categoryType;
                        Menu typeMenu = new Menu("Choose category type",
                            new string[] { "1. Income", "2. Expense" });
                        int type = typeMenu.ActMenu();

                        if (type == 1)
                        {
                            categoryType = EntityType.Income;
                        }
                        else
                        {
                            categoryType = EntityType.Expense;
                        }
                        var newCategory = financeFacade.CreateCategory(categoryType, categoryName);
                        Console.WriteLine($"The category with id {newCategory.Id} was successfully created.");
                        break;
                    // Creating operation.
                    case 3:
                        EntityType operationType;
                        Menu opMenu = new Menu("Choose category type",
                            new string[] { "1. Income", "2. Expense" });
                        int opType = opMenu.ActMenu();
                        
                        if (opType == 1)
                        {
                            operationType = EntityType.Income;
                        }
                        else
                        {
                            operationType = EntityType.Expense;
                        }

                        Console.WriteLine("Enter operation's sum: ");

                        decimal sum;
                        while (!decimal.TryParse(Console.ReadLine(), out sum))
                        {
                            Console.WriteLine("Please try again");
                        }

                        Console.WriteLine("Enter description: "); 
                        string description = Console.ReadLine();

                        var accounts = financeFacade.GetBankAccounts();
                        if (accounts == null || accounts.Count == 0)
                        {
                            Console.WriteLine("There are no accounts");
                            break;
                        }

                        string[] existedAccounts = new string[accounts.Count];
                        for (int i = 0; i < accounts.Count; i++)
                        {
                            existedAccounts[i] = $"{i + 1}. {accounts[i].Name} - Balance: {accounts[i].Balance}";
                        }
                        
                        Menu accountMenu = new Menu("Choose account", existedAccounts);
                        int accNum = accountMenu.ActMenu();
                        var account = accounts[accNum - 1];
                        
                        var categories = financeFacade.GetCategories().FindAll(c => c.EntityType == operationType);
                        if (categories == null || categories.Count == 0)
                        {
                            Console.WriteLine("There are no categories");
                            break;
                        } 
                        string[] existedCat = new string[categories.Count];
                        for (int i = 0; i < categories.Count; i++)
                        {
                            existedCat[i] = $"{i + 1}. {categories[i].Name}";
                        }
                        
                        Menu catMenu = new Menu("Choose category", existedCat);
                        int catNum = catMenu.ActMenu();
                        var category = categories[catNum - 1];
                        
                        financeFacade.CreateOperation(operationType, account.Id, sum, description, category);
                        Console.WriteLine($"The operation was successfully created.");
                        break;
                    
                    // Printing accounts.
                    case 4:
                        foreach (var acc in financeFacade.GetBankAccounts())
                        {
                            Console.WriteLine($"{acc.Id} - {acc.Name}: Balance - {acc.Balance}");
                        }
                        break;
                    
                    // Printing categories.
                    case 5:
                        foreach (var cat in financeFacade.GetCategories())
                        {
                            
                            Console.WriteLine($"{cat.Id} - {cat.Name}: Type - {cat.EntityType}");
                        }
                        break;
                    // Printing operations
                    case 6:
                        foreach (var op in financeFacade.GetOperations())
                        {
                            Console.WriteLine($"{op.Id}: Type - {op.EntityType}; Amount - {op.Amount}; Date - {op.Date}");
                        }
                        break;
                    // Import csv data
                    case 7:
                        Console.WriteLine("Enter file path: ");
                        string csvfilePath = Console.ReadLine();
                        csvImporter.Import(csvfilePath, financeFacade);
                        break;
                    // Import json
                    case 8:
                        Console.WriteLine("Enter file path: ");
                        string jsonfilePath = Console.ReadLine();
                        jsonImporter.Import(jsonfilePath, financeFacade);
                        break;
                    
                    // Export Csv
                    case 9:
                        Console.WriteLine("Enter file path: ");
                        string csvExportFilePath = Console.ReadLine();
                        csvExporter.Export(financeFacade, csvExportFilePath);
                        break;
                    
                    //Export json
                    case 10:
                        Console.WriteLine("Enter file path: ");
                        string jsonExportFilePath = Console.ReadLine();
                        jsonExporter.Export(financeFacade, jsonExportFilePath);
                        break;
                    case 11:
                        exit = true;
                        break;
                }

                if (num != 11)
                {
                    Menu isContinue = new Menu("Do you want to exit?", new string[] { "1. Yes", "2. No" });
                    if (isContinue.ActMenu() == 1)
                    {
                        
                        exit = true;

                    }
                }
            }
        }
        /// <summary>
        /// This method prints greeting according to date.
        /// </summary>
        private void SayHello()
        {
            if (DateTime.Now.Hour < 12 && DateTime.Now.Hour >= 5)
                PrintColor("Good morning!", ConsoleColor.Yellow, ConsoleColor.Yellow);

            else if (DateTime.Now.Hour < 17 && DateTime.Now.Hour >= 12)
                PrintColor("Good afternoon!", ConsoleColor.Magenta, ConsoleColor.Magenta);

            else if (DateTime.Now.Hour < 22 && DateTime.Now.Hour >= 17)
                PrintColor("Good evening!", ConsoleColor.Blue, ConsoleColor.Blue);

            else
                PrintColor("Good night!", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue);
        }

        /// <summary>
        /// This method prints colorful text
        /// </summary>
        /// <param name="str"></param>
        /// <param name="colorDay"></param>
        /// <param name="colorNight"></param>
        public static void PrintColor(string str, ConsoleColor colorDay, ConsoleColor colorNight)
        {
            // Day time colors.
            if (DateTime.Now.Hour <= 20 && DateTime.Now.Hour >= 7)
            {
                Console.ForegroundColor = colorDay;
                Console.WriteLine(str);
                Console.ResetColor();
            }
            // Nighttime colors.
            else
            {
                Console.ForegroundColor = colorNight;
                Console.WriteLine(str);
                Console.ResetColor();
            }
        }
    }
}