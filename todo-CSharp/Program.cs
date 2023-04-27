using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace todo_CSharp
{
    class Program
    {
        private const string FILE_NAME = "todos.json";
        private static List<TodoItem> todos = new List<TodoItem>();

        static void Main(string[] args)
        {
            LoadTodos();
            string input;

            do
            {
                Console.Clear();
                DisplayTodos();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add Todo");
                Console.WriteLine("2. Toggle Todo Completion");
                Console.WriteLine("3. Delete Todo");
                Console.WriteLine("4. Exit");
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddTodo();
                        break;
                    case "2":
                        ToggleTodoCompletion();
                        break;
                    case "3":
                        DeleteTodo();
                        break;
                }
            } while (input != "4");

            SaveTodos();
        }

        private static void LoadTodos()
        {
            if (File.Exists(FILE_NAME))
            {
                string json = File.ReadAllText(FILE_NAME);
                todos = JsonConvert.DeserializeObject<List<TodoItem>>(json);
            }
        }

        private static void SaveTodos()
        {
            string json = JsonConvert.SerializeObject(todos, Formatting.Indented);
            File.WriteAllText(FILE_NAME, json);
        }

        private static void DisplayTodos()
        {
            Console.WriteLine("Todos:");
            foreach (var todo in todos)
            {
                Console.WriteLine($"{todo.Id}. {(todo.Completed ? "[x]" : "[ ]")} {todo.Title}");
            }
            Console.WriteLine();
        }

        private static void AddTodo()
        {
            Console.Write("Enter the title of the new todo: ");
            string title = Console.ReadLine();

            int newId = todos.Count > 0 ? todos[todos.Count - 1].Id + 1 : 1;

            todos.Add(new TodoItem { Id = newId, Title = title, Completed = false });
            SaveTodos();
        }

        private static void ToggleTodoCompletion()
        {
            Console.Write("Enter the ID of the todo
