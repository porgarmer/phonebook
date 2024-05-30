using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using phonebook.Models;
using System.Data;
using System.Data.SqlClient;

namespace phonebook.Controllers
{
    public class PhoneBook : Controller
    {
        string conn = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=phonebook-db;Integrated Security=True;Pooling=False;Encrypt=True;TrustServerCertificate=True";
        // GET: PhoneBook
        public ActionResult Index()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM phonebook_record";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
            }
            return View(dataTable);
        }
    

        // GET: PhoneBook/Create
        public ActionResult Create()
        {
            return View(new PhonebookRecord());
        }

        // POST: PhoneBook/Create
        [HttpPost]
        public ActionResult Create(PhonebookRecord phonebookRecord)
        {
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                sqlConnection.Open();
                string query = "INSERT INTO phonebook_record (first_name, last_name, area_code, phone_number, mobile_number, house_number, street_name, city, province, zip_code, status, email) " +
                    "           VALUES(@fname, @lname, @area_code, @phone, @mobile, @house, @street, @city, @province, @zip, @status, @email)";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@fname", phonebookRecord.firstName);
                command.Parameters.AddWithValue("@lname", phonebookRecord.lastName);
                command.Parameters.AddWithValue("@area_code", phonebookRecord.areaCode);
                command.Parameters.AddWithValue("@phone", phonebookRecord.phoneNumber);
                command.Parameters.AddWithValue("@mobile", phonebookRecord.mobileNumber);
                command.Parameters.AddWithValue("@house", phonebookRecord.houseNumber);
                command.Parameters.AddWithValue("@street", phonebookRecord.streetName);
                command.Parameters.AddWithValue("@city", phonebookRecord.city);
                command.Parameters.AddWithValue("@province", phonebookRecord.province);
                command.Parameters.AddWithValue("@zip", phonebookRecord.zipCode);
                command.Parameters.AddWithValue("@status", phonebookRecord.status);
                command.Parameters.AddWithValue("@email", phonebookRecord.email);
                command.ExecuteNonQuery();
                sqlConnection.Close();

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: PhoneBook/Edit/5
        public ActionResult Edit(int id)
        {
            PhonebookRecord phonebookRecord = new PhonebookRecord();
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                sqlConnection.Open();
                string query = "SELECT * FROM phonebook_record WHERE id = @id";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("id", id);
                sqlDataAdapter.Fill(dataTable);
                sqlConnection.Close();
            }

            if (dataTable.Rows.Count == 1)
            {
                phonebookRecord.firstName = dataTable.Rows[0][1].ToString();
                phonebookRecord.lastName = dataTable.Rows[0][2].ToString();
                phonebookRecord.areaCode = dataTable.Rows[0][3].ToString();
                phonebookRecord.phoneNumber = dataTable.Rows[0][4].ToString();
                phonebookRecord.mobileNumber = dataTable.Rows[0][5].ToString();
                phonebookRecord.houseNumber = dataTable.Rows[0][6].ToString();
                phonebookRecord.streetName = dataTable.Rows[0][7].ToString();
                phonebookRecord.city = dataTable.Rows[0][8].ToString();
                phonebookRecord.province = dataTable.Rows[0][9].ToString();
                phonebookRecord.zipCode = dataTable.Rows[0][10].ToString();
                phonebookRecord.status = dataTable.Rows[0][11].ToString();
                phonebookRecord.email = dataTable.Rows[0][12].ToString();
                return View(phonebookRecord);

            }
            return RedirectToAction(nameof(Index));
        }

        // POST: PhoneBook/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhonebookRecord phoneBookRecord)
        {
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                sqlConnection.Open();
                string query = "UPDATE phonebook_record SET mobile_number = @mobile_number WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@mobile_number", phoneBookRecord.mobileNumber);
                cmd.Parameters.AddWithValue("@id", phoneBookRecord.id);
                cmd.ExecuteNonQuery();

            }
            return RedirectToAction(nameof(Index));
           
        }

        public ActionResult DeleteAll()
        {
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                sqlConnection.Open();
                string query = "DELETE FROM phonebook_record";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            return RedirectToAction(nameof(Index));
        }
        
        public ActionResult Delete()
        {
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                sqlConnection.Open();
                string query = "UPDATE phonebook_record SET status = 'inactive' WHERE status = 'active'";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            return RedirectToAction(nameof(Index));
           
        }
    }
}
