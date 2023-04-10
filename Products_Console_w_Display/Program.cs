// See https://aka.ms/new-console-template for more information


using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Npgsql;

class Sample
{
    static void Main(string[] args)
    {
        // Connect to a PostgreSQL database
        NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1:5432;User Id=postgres; " +
           "Password=kobe7220;Database=prods;");
        conn.Open();



        // Define a query returning a single row result set
        NpgsqlCommand productTable = new NpgsqlCommand("SELECT * FROM product;", conn);
        NpgsqlCommand customerTable = new NpgsqlCommand("SELECT * FROM customer;", conn);
        

    
        number_7(productTable);
        Console.WriteLine();
        number_20(customerTable);

        conn.Close();
    }
 


    static void number_20(NpgsqlCommand command)
    {
        NpgsqlDataReader reader = command.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(reader);

        Dictionary<string, int> colWidths = new Dictionary<string, int>();
        //Obtaining column data
        foreach (DataColumn col in dt.Columns)
        {
            if ((col.ColumnName).ToString() == "rep_id")
            {
                //Displaying rep_id
                Console.Write(col.ColumnName);
                var maxLabelSize = dt.Rows.OfType<DataRow>()
                        .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                        .OrderByDescending(m => m).FirstOrDefault();

                colWidths.Add(col.ColumnName, maxLabelSize);
                for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 14; i++)
                {
                    Console.Write(" ");
                }


                //Displaying balance_sum
                Console.Write("balance_sum");
                colWidths.Add("balance_sum", maxLabelSize);
                for (int i = 0; i < maxLabelSize - "balance_sum".Length + 14; i++)
                {
                    Console.Write(" ");
                }
            }

            Console.Write("");
        }
        Console.WriteLine();

        //Creating a dicitonary of rep_id > cust_balance values so that we can create a sum of values
        Dictionary<int, int> rep_balances = new Dictionary<int, int>();



        //Obtaining row data
        foreach (DataRow dataRow in dt.Rows)
        {
            int rep_id = Convert.ToInt16(dataRow.ItemArray[8]);
            int currCustBalance = Convert.ToInt16(dataRow.ItemArray[6]);

            if (rep_balances != null && rep_balances.ContainsKey(rep_id))
            {
                int previous_value = rep_balances[rep_id];
                rep_balances[rep_id] = previous_value + currCustBalance;
            }
            else if (rep_balances != null && !rep_balances.ContainsKey(rep_id))
            {
                rep_balances.Add(rep_id, currCustBalance);
            }

        }

        foreach(KeyValuePair<int, int> pair in rep_balances)
        {
            if (pair.Value > 12000)
            {
                Console.Write(pair.Key.ToString() + "              " + pair.Value);
                Console.WriteLine();
            }
        }
    }
    static void number_7(NpgsqlCommand command)
    {
        NpgsqlDataReader reader = command.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(reader);

        Dictionary<string, int> colWidths = new Dictionary<string, int>();

        //Obtaining column data
        foreach (DataColumn col in dt.Columns)
        {
            if ((col.ColumnName).ToString() == "prod_id" || (col.ColumnName).ToString() == "prod_desc" || (col.ColumnName).ToString() == "prod_quantity")
            { 
                Console.Write(col.ColumnName);
                var maxLabelSize = dt.Rows.OfType<DataRow>()
                        .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                        .OrderByDescending(m => m).FirstOrDefault();
            
                colWidths.Add(col.ColumnName, maxLabelSize);
                for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 14; i++)
                {
                    Console.Write(" ");
                }
            }
            Console.Write("");
            
        }

        Console.WriteLine();
        //Obtaining row data

        int max_spaces = 34;

        foreach (DataRow dataRow in dt.Rows)
        {
            
            for (int i = 0; i < 1; i++)
            {
                if (Convert.ToInt16(dataRow.ItemArray[2]) > 12 && Convert.ToInt16(dataRow.ItemArray[2]) < 30)
                {
                    int spaces = max_spaces - Convert.ToString(dataRow.ItemArray[1]).Length;
                 
                    Console.Write(Convert.ToString(dataRow.ItemArray[0]) + "              " + Convert.ToString(dataRow.ItemArray[1]));
                    for (int j = 0; j < spaces; j++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(Convert.ToString(dataRow.ItemArray[2]));
                    Console.WriteLine();
                }
            }
            
        }

    }

    static void print_results(DataTable data)
    {
        Console.WriteLine();
        Dictionary<string, int> colWidths = new Dictionary<string, int>();

        foreach (DataColumn col in data.Columns)
        {
            Console.Write(col.ColumnName);
            var maxLabelSize = data.Rows.OfType<DataRow>()
                    .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                    .OrderByDescending(m => m).FirstOrDefault();

            colWidths.Add(col.ColumnName, maxLabelSize);
            for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 14; i++) Console.Write(" ");
        }

        Console.WriteLine();

        foreach (DataRow dataRow in data.Rows)
        {
            for (int j = 0; j < dataRow.ItemArray.Length; j++)
            {
                Console.Write(dataRow.ItemArray[j]);
                for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 14; i++) Console.Write(" ");
            }
            Console.WriteLine();
        }
    }

    static void executeAndPrintCommand(NpgsqlCommand command)
    {
        NpgsqlDataReader reader = command.ExecuteReader();
        DataTable dt = new DataTable();
        dt.Load(reader);
        print_results(dt);
    }
}

