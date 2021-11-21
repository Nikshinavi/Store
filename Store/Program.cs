using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace Store
{
    class Program
    {
        static void Main(string[] args)
        {
            var userBase = new UserBase();
            var generalMenu = new GeneralMenu(userBase); //новое
            generalMenu.Menu(); //новое
        }
    }

    public class GeneralMenu // новое kfkfkfkfkfkfkfkf
    {
        public UserBase UserBase;

        public GeneralMenu(UserBase userBase)
        {
            UserBase = userBase;
        }

        public virtual void Menu()
        {
            Console.WriteLine("Здравствуйте!");
            Console.WriteLine("Выберите одно из следующих действий:");
            Console.WriteLine();
            SelectOperation(UserBase);

        }
        public virtual void SelectOperation(UserBase baseOfUsers) //заменил static на virtual для переопределение в классах наследниках
        {
            var firstChoice = new Dictionary<int, string>()
            {
                {1, "Авторизоваться"},
                {2, "Зарегистрироваться"},
                {3, "Выход"},
            };
            
            foreach (var keyValuePair in firstChoice)
            {
                Console.WriteLine($"{keyValuePair.Key}. {keyValuePair.Value}");
            }

            var numberOfRequest = GetSelectedMenuItem();
            
            switch (numberOfRequest)
            {
                case 1:
                    baseOfUsers.LogIn(); 
                    // нужно сделать переход в AdminMenu или UserMenu в зависимости от введенных данных
                    //string role = UserBase.UserRole();
                    AdminMenu adminMenu = new AdminMenu(UserBase); //временно вызываем AdminMenu сразу
                    adminMenu.Menu();
                    break;
                case 2:
                    baseOfUsers.CreateAccount();
                    baseOfUsers.LogIn(); 
                    //как тут перейти в AdminMenU или UserMenu?
                    break;
                case 3:
                    Console.WriteLine("*** До свидания! ***");
                    break;
            }
            
        }
        public virtual int GetSelectedMenuItem() //заменил private static на public virtual для переопределение в классах наследниках
        {
            var consoleKey = Console.ReadLine();
            Console.WriteLine();
            switch (consoleKey)
            {
                case "1":
                    return 1;
                case "2":
                    return 2;
                case "3":
                    return 3;
                default:
                    Console.WriteLine("*** Ошибка ввода данных ***");
                    Console.WriteLine("Выберите один из предложенных выше вариантов ответа");
                    return GetSelectedMenuItem(); //добавил эту строку чтобы можно было снова ввести данные
            }
        }
        
    }
    
    public class AdminMenu : GeneralMenu //новое 
    {
        
        public AdminMenu(UserBase userBase) : base(userBase)
        {
        }

        public override void Menu()
        {
            Console.WriteLine("Выберите одно из следующих действий:");
            Console.WriteLine();
            SelectOperation(UserBase);
        }

        public override void SelectOperation(UserBase baseOfUsers)
        {
            var userChoice = new Dictionary<int, string>()
            {
                {1, "Распечатать список всех пользователей"},
                {2, "Удалить пользователя"},
                {3, "Распечатать список всех продуктов"},
                {4, "Добавить продукт"},
                {5, "Удалить продукт"},
                {6, "Выход"},
            };
            
            foreach (var keyValuePair in userChoice)
            {
                Console.WriteLine($"{keyValuePair.Key}. {keyValuePair.Value}");
            }
            
            Console.WriteLine();
            
            var numberOfRequest = GetSelectedMenuItem();

            switch (numberOfRequest)
            {
                case 1:
                    UserBase.PrintAllUsers();
                    Menu();
                    break;
                case 2:
                    UserBase.DeleteUser();
                    Menu();
                    break;
                case 3: //распечатать все продукты
                    break;
                case 4: //добавить продукт
                    break; 
                case 5: //удалить продукт
                    break;
                case 6:
                    Console.WriteLine("*** До свидания! ***");
                    base.Menu(); //вызывается AdminMenu.Menu, как вызвать GeneralMenu.Menu???
                    break;
            }
        }

        public override int GetSelectedMenuItem()
        {
            var choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    return 1;
                case "2":
                    return 2;
                case "3":
                    return 3;
                case "4":
                    return 4;
                case "5":
                    return 5;
                case "6":
                    return 6;
                default:
                    Console.WriteLine("*** Ошибка ввода данных ***");
                    Console.WriteLine("Выберите один из предложенных выше вариантов ответа");
                    return GetSelectedMenuItem(); //добавил эту строку чтобы можно было снова ввести данные
            }
        }
    }

    public class UserMenu : GeneralMenu //новое
    {
        public UserMenu(UserBase userBase) : base(userBase)
        {
        }
        
    }
    
    public class UserBase
    {
        public List<User> Users { get; } 

        public UserBase()
        {
            Users = new List<User>();
            Users.Add(new User("Admin", "123", "Admin")); //добавил роль
            Users.Add(new User("Max", "123")); //тут роли по умолчанию есть
            Users.Add(new User("Ted", "123"));
            Users.Add(new User("Ann", "123"));
        }

        public void CreateAccount() //добавить проверку на наличие логина в листе Users
        {
            Console.WriteLine("Придумайте логин");
            string login = Console.ReadLine();
            Console.WriteLine("Придумайте пароль");
            string password = Console.ReadLine();
            var user = new User(login, password);
            Users.Add(user);
            Console.WriteLine();
            Console.WriteLine("Вы успешно зарегестрировались");
            Console.WriteLine();
        }

        public void DeleteUser()
        {
            Console.WriteLine("Какого пользователя вы хотите удалить?");
            PrintAllUsers();
            int userNumber = Convert.ToInt32(Console.ReadLine());
            Users.RemoveAt(userNumber - 1);
        }

        public void PrintAllUsers()
        {
            for (var i = 0; i < Users.Count; i++)
            {
                Console.WriteLine($"{i + 1}.{Users[i]}"); // переопределил ToString в User
            }
            Console.WriteLine();
        }

        public void LogIn() // для того чтобы в зависимости от роли показывать AdminMenu или UserMenu - нужно возвращать string с ролью
        {
            Console.Write("Введите логин: ");
            var login = Console.ReadLine();
            CheckLogin(login);
            
            if (CheckLogin(login)) //сделал по новому.
            {
                Console.Write("Введите пароль: ");
                var password = Console.ReadLine();
                if (CheckPassword(password, login));
                {
                    Console.WriteLine();
                    Console.WriteLine($"Здравствуте, {login}");
                    Console.WriteLine();
                }
                // Нужна проверка пароля, но так не дает сделать:
                //else
                //{
                //    Console.WriteLine("Неправильный пароль");
                //    LogIn();
                //}
            }
            else
            {
                Console.WriteLine("Пользователя с таким логином не найдено");
                LogIn();
            }
        }

        private bool CheckLogin(string login)
        {
            foreach (var user in Users)
            {
                if (login == user.Login)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckPassword(string password, string login) //написал метод
        {
            int indexOfUserWithChoosedLogin = -1;

            foreach (var user in Users)
            {
                indexOfUserWithChoosedLogin++;
                if (login == user.Login)
                {
                    break;
                }
            }

            if (Users[indexOfUserWithChoosedLogin].Password == password)
            {
                return true;
            }
            return false;
        }

        public string UserRole(User user) // метод возвращающий роль
        {
            if (user.Role == "Admin")
            {
                return "Admin";
            }
            else
            {
                return "UsualUser";
            }
        }
        
    }

    public class User
    {
        public string Login { get; }
        public string Password { get; }
        public string Role { get; } // добавил роль
        public List<Product> Basket { get; }


        public User(string login, string password, string role = "UsualUser") // добавил роль по умолчанию
        {
            Login = login;
            Password = password;
            Role = role;
            Basket = new List<Product>(); 
        }

        public override string ToString()
        {
            return Login;
        }
    }

    public class Product 
    {
        public string Name { get; }
        public int Price { get; }
        
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return "Name: " + Name + " / Price: " + Price;
        }
    }
}