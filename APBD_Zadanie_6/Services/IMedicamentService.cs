

using APBD_Zadanie_6.Models;

namespace Zadanie5.Services
{
    public interface IMedicamentService
    {
        public Task<MedicamentDTO> GetMedicament(int id);
        public void DeletePatient(int id);
    }
}
