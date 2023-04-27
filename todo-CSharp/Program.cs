using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace todo_CSharp
{
    class Program
    {
        private const string FILE_NAME = "todos.json";
        private static Dictionary<string, List<TodoItem>> userTodos = new Dictionary<string, List<TodoItem>>();
        private static string currentUser;

        static void Main(string[] args)
        {
            LoadTodos(); // Load existing todos from file

            do
            {
                Console.Write("Enter your username: ");
                currentUser = Console.ReadLine();

                if (!userTodos.ContainsKey(currentUser))
                {
                    userTodos[currentUser] = new List<TodoItem>();
                }

                string input;

                do
                {
                    Console.Clear();
                    DisplayTodos(); // Show the list of todos

                    // Show user commands
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1. Add Todo");
                    Console.WriteLine("2. Toggle Todo Completion");
                    Console.WriteLine("3. Delete Todo");
                    Console.WriteLine("4. Exit");
                    Console.WriteLine("5. Sign Out");

                    Console.Write("Enter your command: ");
                    input = Console.ReadLine();

                    // Split the input into command and arguments
                    string[] inputParts = input.Split(' ', 2);
                    string command = inputParts[0];

                    switch (command)
                    {
                        case "1": // Add Todo
                            if (inputParts.Length == 2)
                            {
                                AddTodo(inputParts[1]);
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Press Enter to continue.");
                                Console.ReadLine();
                            }
                            break;
                        case "2": // Toggle Todo Completion
                            if (inputParts.Length == 2)
                            {
                                ToggleTodoCompletion(inputParts[1]);
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Press Enter to continue.");
                                Console.ReadLine();
                            }
                            break;
                        case "3": // Delete Todo
                            if (inputParts.Length == 2)
                            {
                                DeleteTodo(inputParts[1]);
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Press Enter to continue.");
                                Console.ReadLine();
                            }
                            break;
                        case "4": // Sign Out
                            currentUser = null;
                            break;
                    }
                } while (input != "4" && input != "5");

            } while (currentUser != null);

            SaveTodos(); // Save todos to file before exiting
        }

        // Load todos from the JSON file
        private static void LoadTodos()
        {
            if (File.Exists(FILE_NAME))
            {
                string json = File.ReadAllText(FILE_NAME);
                userTodos = JsonConvert.DeserializeObject<Dictionary<string, List<TodoItem>>>(json);
            }
        }

        // Save todos to the JSON file
        private static void SaveTodos()
        {
            string json = JsonConvert.SerializeObject(userTodos, Formatting.Indented);
            File.WriteAllText(FILE_NAME, json);
        }

        // Display the list of todos in the console
        private static void DisplayTodos()
        {
            Console.WriteLine($"Todos for {currentUser}:");
            foreach (var todo in userTodos[currentUser])
            {
                Console.WriteLine($"{todo.Id}. {(todo.Completed ? "[x]" : "[ ]")} {todo.Title}");
            }
            Console.WriteLine();
        }

        // Add a new todo with the given title
        private static void AddTodo(string title)
        {
            int newId = userTodos[currentUser].Count > 0 ? userTodos[currentUser].Max(t => t.Id) + 1 : 1;

            userTodos[currentUser].Add(new TodoItem { Id = newId, Title = title, Completed = false });

            SaveTodos();
        }

        // Toggle the completion status of the todo with the given ID
        private static void ToggleTodoCompletion(string idInput)
        {
            int id;
            if (int.TryParse(idInput, out id))
            {
                var todo = userTodos[currentUser].Find(t => t.Id == id);
                if (todo != null)
                {
                    todo.Completed = !todo.Completed;
                    SaveTodos();
                }
                else
                {
                    Console.WriteLine("Invalid ID. Press Enter to continue.");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Press Enter to continue.");
                Console.ReadLine();
            }
        }

        // Delete the todo with the given ID
        private static void DeleteTodo(string idInput)
        {
            int id;
            if (int.TryParse(idInput, out id))
            {
                var todo = userTodos[currentUser].Find(t => t.Id == id);
                if (todo != null)
                {
                    userTodos[currentUser].Remove(todo);
                    SaveTodos();
                }
                else
                {
                    Console.WriteLine("Invalid ID. Press Enter to continue.");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Press Enter to continue.");
                Console.ReadLine();
            }
        }
    }

    class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }
}
