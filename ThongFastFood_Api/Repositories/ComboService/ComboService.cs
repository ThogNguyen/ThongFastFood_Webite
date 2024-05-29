using System.Security.AccessControl;
using ThongFastFood_Api.Data;
using ThongFastFood_Api.Models;

namespace ThongFastFood_Api.Repositories.ComboService
{
    public class ComboService : IComboService
    {
        private readonly FoodStoreDbContext db;

        public ComboService(FoodStoreDbContext context)
        {
            db = context;
        }

        public ComboVM AddCombo(ComboVM model)
        {
            // Tạo đối tượng Product từ ProductModel
            var combo = new Combo
            {
                ComboName = model.ComboName,
                ComboImage = model.ComboImage,
                ComboPrice = model.ComboPrice
            };

            // Thêm sản phẩm vào cơ sở dữ liệu
            db.Combos.Add(combo);
            db.SaveChanges();

            // Trả về model của sản phẩm đã được thêm
            return model;
        }

        public void DeleteCombo(int id)
        {
            // Xóa sản phẩm từ cơ sở dữ liệu
            var combo = db.Combos.Find(id);
            if (combo != null)
            {
                // Remove the product from the database
                db.Combos.Remove(combo);
                db.SaveChanges();
            }
        }

        public List<ComboVM> GetCombo()
        {
            // Lấy danh sách sản phẩm từ cơ sở dữ liệu
            var comboVM = db.Combos.Select(p => new ComboVM
            {
                ComboId = p.ComboId,
                ComboName = p.ComboName,
                ComboImage = p.ComboImage,
                ComboPrice = p.ComboPrice
            }).ToList();

            return comboVM;
        }

        public ComboVM GetIdCombo(int id)
        {
            // Lấy thông tin của sản phẩm theo ID từ cơ sở dữ liệu
            var combo = db.Combos.Find(id);

            if (combo != null)
            {
                return new ComboVM
                {
                    ComboId = combo.ComboId,
                    ComboName = combo.ComboName,
                    ComboImage = combo.ComboImage,
                    ComboPrice = combo.ComboPrice,
                };
            }
            return null;
        }

        public ComboVM UpdateCombo(int id, ComboVM model)
        {
            var combo = db.Combos.Find(id);
            if (combo != null)
            {
                combo.ComboName = model.ComboName;
                combo.ComboImage = model.ComboImage;
                combo.ComboPrice = model.ComboPrice;

                db.SaveChanges();

                return model;
            }
            return null;
        }
    }
}
