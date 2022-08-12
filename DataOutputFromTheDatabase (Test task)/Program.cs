using Microsoft.Data.Sqlite;

    // Объект подключения необходимой базы данных

  using (SqliteConnection connectionDB = new SqliteConnection("Data Source =DataBase.db;"))
  {
      SqliteCommand command;
      SqliteCommand commandSelect;

      #region Открытие базы данных и создание необходимых таблиц со значениями
      try
      {
          Console.WriteLine("Идет открытие базы даннных.");
          connectionDB.Open();

      }
      catch
      {
          Console.WriteLine("Открытие базы даннных вызвало ошибку!");
          return;
      }

      command = connectionDB.CreateCommand();

      //---------------
      command.CommandText = "CREATE TABLE Customers ( Id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR (50) NOT NULL);" +
          "\n\nCREATE TABLE Orders (Id INTEGER PRIMARY KEY AUTOINCREMENT, CustomerId INT, FOREIGN KEY (CustomerId)  REFERENCES Customers (Id));" +
          "\n\nINSERT INTO Customers (Name) VALUES ('Max'), ('Pavel'), ('Ivan'), ('Leonid');" +
          "\n\nINSERT INTO Orders (CustomerId) VALUES (2),(4);";

      Console.WriteLine("\nПроисходит выполнение запроса: \n\n" +
          "-------------------\n" + command.CommandText);

      Console.WriteLine("-------------------\n");
      command.ExecuteNonQuery();

      #endregion

      #region Выполнение Select запроса, направленного на поиск лиц, не совершивших ни одной покупки

      try
      {
          commandSelect = connectionDB.CreateCommand();

          commandSelect.CommandText = "SELECT Name AS Customer FROM Customers WHERE NOT EXISTS (SELECT 1 FROM Orders WHERE Orders.CustomerId = Customers.Id);";

          Console.WriteLine("\nПроисходит выполнение запроса: \n\n" +
         "-------------------\n" + commandSelect.CommandText);

          Console.WriteLine("-------------------\n");

          SqliteDataReader SQL = commandSelect.ExecuteReader();

          if (SQL.HasRows)
          {
              while (SQL.Read())
              {
                  Console.WriteLine("Customer: " + SQL["Customer"]);
              }
          }
          else
          {
              Console.WriteLine("\nТаких записей в БД не обнаружено!\n");
          }
      }
      catch
      {
          Console.WriteLine("Ошибка выполнения SELECT запроса!");
      }
      
      finally
      {
          #endregion

      #region Блок завершения работы программы

          Console.WriteLine("\nДля завершения работы программы нажмите клавишу \"ESC\".");

          ConsoleKeyInfo cons = Console.ReadKey();
          while (cons.Key != ConsoleKey.Escape)
          {
              cons = Console.ReadKey();
          }


          //---------------

          command.CommandText = "DROP TABLE Orders;" +
              "DROP TABLE Customers;";
          command.ExecuteNonQuery();

          //---------------

          try
          {
              connectionDB.Close();
          }
          catch
          {
              Console.WriteLine("Закрытие базы даннных вызвало ошибку!");
          }
      }
          #endregion
      
  }


