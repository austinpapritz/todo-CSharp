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
        private static List<TodoItem> todos = new List<TodoItem>();

        static void Main(string[] args)
        {
            LoadTodos(); // Load existing todos from file

            string input;

            do
            {
                Console.Clear();
                DisplayTodos(); // Show the list of todos
                Console.WriteLine("Choose an option:"); // Writes the list of user commands
                Console.WriteLine("1. Add Todo");
                Console.WriteLine("2. Toggle Todo Completion");
                Console.WriteLine("3. Delete Todo");
                Console.WriteLine("4. Exit");
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
                }
            } while (input != "4");

            SaveTodos(); // Save todos to file before exiting
        }

        // Load todos from the JSON file
        private static void LoadTodos()
        {
            if (File.Exists(FILE_NAME))
            {
                string json = File.ReadAllText(FILE_NAME);
                todos = JsonConvert.DeserializeObject<List<TodoItem>>(json);
            }
        }

        // Save todos to the JSON file
        private static void SaveTodos()
        {
            string json = JsonConvert.SerializeObject(todos, Formatting.Indented);
            File.WriteAllText(FILE_NAME, json);
        }

        // Display the list of todos in the console
        private static void DisplayTodos()
        {
            Console.WriteLine("Todos:");
            foreach (var todo in todos)
            {
                Console.WriteLine($"{todo.Id}. {(todo.Completed ? "[x]" : "[ ]")} {todo.Title}");
            }
            Console.WriteLine();
        }

        // Add a new todo with the given title
        private static void AddTodo(string title)
        {
            int newId = todos.Count > 0 ? todos.Max(t => t.Id) + 1 : 1;

            todos.Add(new TodoItem { Id = newId, Title = title, Completed = false });
            SaveTodos();
        }

        // Toggle the completion status of the todo with the given ID
        private static void ToggleTodoCompletion(string idInput)
        {
            int id;
            if (int.TryParse(idInput, out id))
            {
                var todo = todos.Find(t => t.Id == id);
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
                var todo = todos.Find(t => t.Id == id);
                if (todo != null)
                {
                    todos.Remove(todo);
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
}

