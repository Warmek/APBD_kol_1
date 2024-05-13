namespace APBD_Zadanie_6.Models
{
    public class PrescriptionDTO
    {
            public int MedicamentId { get; set; }
            public int PrescriptionId { get; set; }
            public int Dose { get; set; }
            public string Details { get; set; }
            public DateTime Date { get; set; }
    }
}
