using APBD_Zadanie_6.Models;
using System.Data.SqlClient;

namespace Zadanie5.Services
{
    public class MedicamentService : IMedicamentService
    {
        private readonly IConfiguration _configuration;

        public MedicamentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<MedicamentDTO> GetMedicament(int id)
        {
            var connectionString = _configuration.GetConnectionString("Database");
            using var connection = new SqlConnection(connectionString);
            using var cmd = new SqlCommand();

            cmd.Connection = connection;
            await connection.OpenAsync();

            cmd.CommandText = "SELECT * FROM Medicament WHERE IdMedicament = @Id";
            cmd.Parameters.AddWithValue("Id", id);

            var reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows) throw new Exception("No medicament with id: " + id);

            await reader.ReadAsync();

            int Id = int.Parse(reader["IdMedicament"].ToString());
            string name = reader["Name"].ToString();
            string description = reader["Description"].ToString();
            string type = reader["Type"].ToString();


            await reader.CloseAsync();
            cmd.Parameters.Clear();

            MedicamentDTO toReturn = new MedicamentDTO();


            cmd.CommandText = "SELECT * FROM Prescription_Medicament WHERE IdMedicament = @Id";
            cmd.Parameters.AddWithValue("Id", id);

            reader = await cmd.ExecuteReaderAsync();


            List<PrescriptionMedicament> prescriptionMedicaments = new List<PrescriptionMedicament>();


            while (reader.Read())
            {
                int IdPrescription = int.Parse(reader["IdPrescription"].ToString());
                int IdMedicament = int.Parse(reader["IdMedicament"].ToString());
                int dose = int.Parse(reader["Dose"].ToString());
                string details = reader["Details"].ToString();

                PrescriptionMedicament prescriptionMedicament = new PrescriptionMedicament();

                prescriptionMedicament.IdPrescription = IdPrescription;
                prescriptionMedicament.IdMedicament = IdMedicament;
                prescriptionMedicament.Dose = dose;
                prescriptionMedicament.Details = details;

                prescriptionMedicaments.Add(prescriptionMedicament);
            }

            await reader.CloseAsync();
            cmd.Parameters.Clear();

            List<PrescriptionDTO> prescriptions = new List<PrescriptionDTO>();

            foreach (PrescriptionMedicament prescriptionMedicament in prescriptionMedicaments) {
                cmd.CommandText = "SELECT * FROM Prescription WHERE IdPrescription = @Id";
                cmd.Parameters.AddWithValue("Id", prescriptionMedicament.IdPrescription);

                reader = await cmd.ExecuteReaderAsync();
                if (!reader.HasRows) throw new Exception("Prescription with id: " + prescriptionMedicament.IdPrescription + " does not exist");

                await reader.ReadAsync();

                int MedId = prescriptionMedicament.IdMedicament;
                int PreId = prescriptionMedicament.IdPrescription;
                int Dose = prescriptionMedicament.Dose;
                string details = prescriptionMedicament.Details;
                DateTime Date = DateTime.Parse(reader["Date"].ToString());

                PrescriptionDTO prescription = new PrescriptionDTO();

                prescription.MedicamentId = MedId;
                prescription.PrescriptionId = PreId;
                prescription.Dose = Dose;   
                prescription.Details = details;
                prescription.Date = Date;
                prescriptions.Add(prescription);


                await reader.CloseAsync();
                cmd.Parameters.Clear();
            }



            

            toReturn.Id = Id;
            toReturn.Name = name;
            toReturn.Description = description;
            toReturn.Type = type;

            toReturn.Prescriptions = prescriptions.OrderByDescending(p => p.Date).ToList();
            
            return toReturn;
        }
        public async void DeletePatient(int PatientId)
        {
            var connectionString = _configuration.GetConnectionString("Database");
            using var connection = new SqlConnection(connectionString);
            using var cmd = new SqlCommand();

            cmd.Connection = connection;
            await connection.OpenAsync();

            cmd.CommandText = "SELECT IdPrescription FROM Prescription WHERE IdPatient = @Id";
            cmd.Parameters.AddWithValue("Id", PatientId);

            var reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows) throw new Exception("No patient with id: " + PatientId);



            List<int> prescriptions = new List<int>();

            while (reader.Read())
            {
                int IdPrescription = int.Parse(reader["IdPrescription"].ToString());
                prescriptions.Add(IdPrescription);
            }

            await reader.CloseAsync();
            cmd.Parameters.Clear();

            foreach (int PreId in prescriptions)
            {
                cmd.CommandText = "DELETE FROM Prescription_Medicament WHERE IdPrescription = @Id";
                cmd.Parameters.AddWithValue("Id", PreId);

                await cmd.ExecuteNonQueryAsync();
                cmd.Parameters.Clear();
            }

            cmd.CommandText = "DELETE FROM Prescription WHERE IdPatient = @Id";
            cmd.Parameters.AddWithValue("Id", PatientId);

            await cmd.ExecuteNonQueryAsync();
            cmd.Parameters.Clear();

            cmd.CommandText = "DELETE FROM Patient WHERE IdPatient = @Id";
            cmd.Parameters.AddWithValue("Id", PatientId);

            await cmd.ExecuteNonQueryAsync();

            cmd.Parameters.Clear();
        }
    }
}
