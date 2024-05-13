namespace APBD_Zadanie_6.Models
{
    public class MedicamentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<PrescriptionDTO> Prescriptions { get; set; }
    }
}
