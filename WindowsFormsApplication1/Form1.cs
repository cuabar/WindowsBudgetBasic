using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Int16 userID = 0;
        Int16 maxUser = 0;

        public Form1()
        {
            InitializeComponent();
            //Perform SQL count query
            MySqlConnection conn = new MySqlConnection("Server=tootalentlesshacks.com;Port=3306;Database=tthacks_budgeting;Uid=tthacks_cbudget;Pwd=pract1ce;");
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM users", conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                maxUser = Int16.Parse(reader.GetValue(0).ToString());
            }
            reader.Close();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            try
            { 
                if (userID == maxUser)
                {
                    MessageBox.Show("End of User List. No More Users.");
                    nextButton.Enabled = false;
                }
                else
                {
                    String firstName = "";
                    String secondName = "";
                    Int16 income = 0;
                    String expenses = "";
                    Int16 tempExpense = 0;
                    Int16 totalExpenses = 0;
                    String regionName = "";
                    Int16 rentPrice = 0;
                    Int16 balance = 0;

                    userID++;

                    if(userID > 0)
                    {
                        prevButton.Enabled = true;
                    }

                    //Get User info from SQL
                    MySqlConnection conn = new MySqlConnection("Server=tootalentlesshacks.com;Port=3306;Database=tthacks_budgeting;Uid=tthacks_cbudget;Pwd=pract1ce;");
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("SELECT u.first, u.last, u.income, e.name, e.cost, r.region, r.rent FROM users u, expenses e, user_expense l, renting r WHERE u.userid = "+ userID +" AND l.userid = "+ userID + " AND e.expenseid = l.expenseid AND r.userid = " + userID, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        firstName = reader.GetValue(0).ToString();
                        secondName = reader.GetValue(1).ToString();
                        income = Int16.Parse(reader.GetValue(2).ToString());
                        rentPrice = Int16.Parse(reader.GetValue(6).ToString());
                        regionName = reader.GetValue(5).ToString();
                        tempExpense = Int16.Parse(reader.GetValue(4).ToString());
                        totalExpenses = (Int16)(totalExpenses + tempExpense);

                        //Create String of Expenses
                        expenses = expenses + reader.GetValue(3).ToString() + "(" + reader.GetValue(4).ToString() + ")\n"
;                    }
                    reader.Close();

                    expenses = expenses.Replace("\n", System.Environment.NewLine);

                    incomeBox.Text = "Income Per Month: " + income;
                    expensesBox.Text = expenses;
                    firstNameBox.Text = firstName;
                    lastNameBox.Text = secondName;
                    rentBox.Text = "" + rentPrice;
                    houseBox.Text = regionName;

                    totalExpenses = (Int16)(totalExpenses + rentPrice);
                    balance = (Int16)(income - totalExpenses);
                    balanceBox.Text = "Balance For Month: " + balance;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (userID == 1)
                {
                    MessageBox.Show("End of User List. No More Users.");
                }
                else
                {
                    String firstName = "";
                    String secondName = "";
                    Int16 income = 0;
                    String expenses = "";
                    Int16 tempExpense = 0;
                    Int16 totalExpenses = 0;
                    String regionName = "";
                    Int16 rentPrice = 0;
                    Int16 balance = 0;

                    userID--;
                    nextButton.Enabled = true;

                    if (userID == 0)
                    {
                        userID = 1;
                    }
                    if(userID == 1)
                    {
                        prevButton.Enabled = false;
                    }

                    //Get User info from SQL
                    MySqlConnection conn = new MySqlConnection("Server=tootalentlesshacks.com;Port=3306;Database=tthacks_budgeting;Uid=tthacks_cbudget;Pwd=pract1ce;");
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("SELECT u.first, u.last, u.income, e.name, e.cost, r.region, r.rent FROM users u, expenses e, user_expense l, renting r WHERE u.userid = " + userID + " AND l.userid = " + userID + " AND e.expenseid = l.expenseid AND r.userid = " + userID, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        firstName = reader.GetValue(0).ToString();
                        secondName = reader.GetValue(1).ToString();
                        income = Int16.Parse(reader.GetValue(2).ToString());
                        rentPrice = Int16.Parse(reader.GetValue(6).ToString());
                        regionName = reader.GetValue(5).ToString();
                        tempExpense = Int16.Parse(reader.GetValue(4).ToString());
                        totalExpenses = (Int16)(totalExpenses + tempExpense);

                        //Create String of Expenses
                        expenses = expenses + reader.GetValue(3).ToString() + "(" + reader.GetValue(4).ToString() + ")\n"
;
                    }
                    reader.Close();

                    expenses = expenses.Replace("\n", System.Environment.NewLine);

                    incomeBox.Text = "Income Per Month: " + income;
                    expensesBox.Text = expenses;
                    firstNameBox.Text = firstName;
                    lastNameBox.Text = secondName;
                    rentBox.Text = "" + rentPrice;
                    houseBox.Text = regionName;

                    totalExpenses = (Int16)(totalExpenses + rentPrice);
                    balance = (Int16)(income - totalExpenses);
                    balanceBox.Text = "Balance For Month: " + balance;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                String firstName = "";
                String lastName = "";
                String region = "";
                Int16 rentCost = 0;
                Int16 income = 0;
                Int16 temp = 0;
                Int16 currentUser = 0;

                if (string.IsNullOrWhiteSpace(nameInput.Text) || string.IsNullOrWhiteSpace(lastInput.Text) || string.IsNullOrWhiteSpace(regionInput.Text) || string.IsNullOrWhiteSpace(incomeInput.Text) || string.IsNullOrWhiteSpace(rentInput.Text))
                {
                    MessageBox.Show("All Input Fields Must Be Completed!");
                }
                else
                {
                    if (Int16.TryParse(rentInput.Text, out temp))
                    {
                        if (temp <= 0)
                        {
                            MessageBox.Show("Rent Must be a number above 0");
                        }
                        else
                        {
                            if (Int16.TryParse(incomeInput.Text, out temp))
                            {
                                if (temp <= 0)
                                {
                                    MessageBox.Show("Income Must be a number above 0");
                                }
                                else
                                {
                                    firstName = nameInput.Text;
                                    lastName = lastInput.Text;
                                    region = regionInput.Text;
                                    rentCost = Int16.Parse(rentInput.Text);
                                    income = Int16.Parse(incomeInput.Text);

                                    MySqlConnection conn = new MySqlConnection("Server=tootalentlesshacks.com;Port=3306;Database=tthacks_budgeting;Uid=tthacks_cbudget;Pwd=pract1ce;");
                                    conn.Open();
                                    MySqlCommand countCommand = new MySqlCommand("SELECT COUNT(*) FROM users", conn);
                                    MySqlDataReader reader = countCommand.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        currentUser = Int16.Parse(reader.GetValue(0).ToString());
                                    }
                                    reader.Close();
                                    currentUser++;

                                    MySqlCommand command = new MySqlCommand("INSERT INTO users(first, last, income) VALUES ('"+firstName+"', '"+lastName+"', "+income+")", conn);
                                    MySqlCommand rentCommand = new MySqlCommand("INSERT INTO renting(userid, region, type, rent) VALUES ("+currentUser+", '"+region+"', 'Apartment', "+rentCost+")", conn);
                                    command.ExecuteNonQuery();
                                    rentCommand.ExecuteNonQuery();

                                    //Handle Expenses
                                    for(int i = 0; i < expensesList.Items.Count; i++)
                                    {
                                        if(expensesList.GetItemChecked(i))
                                        {
                                            int x = i + 1;
                                            MySqlCommand expensesCommand = new MySqlCommand("INSERT INTO user_expense(userid, expenseid) VALUES(" + currentUser + ", " + x + ")", conn);
                                            expensesCommand.ExecuteNonQuery();
                                        }
                                    }
                                    maxUser++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }
    }
}
