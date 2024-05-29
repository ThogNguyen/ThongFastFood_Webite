using ThongFastFood_Api.Models;

namespace ThongFastFood_Api.Repositories.ComboService
{
    public interface IComboService
    {
        public List<ComboVM> GetCombo();
        ComboVM AddCombo(ComboVM model);
        ComboVM GetIdCombo(int id);
        ComboVM UpdateCombo(int id, ComboVM model);
        void DeleteCombo(int id);
    }
}
