using GenericHierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleOrganization.Infrastructure.Modules
{
    public class Module:IBaseHierarchyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentId { get; set; }

    }
    public enum ModuleType
    {
        Anasayfa = 1,
        Dashboard = 2,
        Islemler = 3,
        Urunler = 4,
        Raporlar = 5,
        Faturalar = 6,
        YillikRaporlar = 7,
        AylikRaporlar = 8,
        HaftalikRaporlar = 9,
        AylikRaporEkle = 10,
        AylikRaporGuncelle = 11,
        AylikRaporSil = 12,
        AylikRaporGoruntule = 13
    }
    public static class ModuleRepository 
    {
     private static List<Module> modules = new List<Module>
    {
        new Module { Id = (int)ModuleType.Anasayfa, Name = "Anasayfa", ParentId = null },
        new Module { Id = (int)ModuleType.Dashboard, Name = "Dashboard", ParentId = (int?)ModuleType.Anasayfa },
        new Module { Id = (int)ModuleType.Islemler, Name = "İşlemler", ParentId = (int?)ModuleType.Anasayfa },
        new Module { Id = (int)ModuleType.Urunler, Name = "Ürünler", ParentId = (int?)ModuleType.Islemler },
        new Module { Id = (int)ModuleType.Raporlar, Name = "Raporlar", ParentId = (int?)ModuleType.Islemler },
        new Module { Id = (int)ModuleType.Faturalar, Name = "Faturalar", ParentId = (int ?) ModuleType.Islemler },
        new Module { Id = (int)ModuleType.YillikRaporlar, Name = "Yıllık Raporlar", ParentId = (int ?) ModuleType.Raporlar },
        new Module { Id = (int)ModuleType.AylikRaporlar, Name = "Aylık Raporlar", ParentId = (int ?) ModuleType.Raporlar },
        new Module { Id = (int)ModuleType.HaftalikRaporlar, Name = "Haftalık Raporlar", ParentId = (int ?) ModuleType.Raporlar },
        new Module { Id = (int)ModuleType.AylikRaporEkle, Name = "Aylık Rapor Ekle", ParentId = (int ?) ModuleType.AylikRaporlar },
        new Module { Id = (int)ModuleType.AylikRaporGuncelle, Name = "Aylık Rapor Güncelle", ParentId = (int ?) ModuleType.AylikRaporlar },
        new Module { Id = (int)ModuleType.AylikRaporSil, Name = "Aylık Rapor Sil", ParentId = (int ?) ModuleType.AylikRaporlar },
        new Module { Id = (int)ModuleType.AylikRaporGoruntule, Name = "Aylık Rapor Görüntüle", ParentId = (int ?) ModuleType.AylikRaporlar },
    };
        public static List<Module> GetModules()
        {
            return modules;
        }
        public static Module AddModule(Module module)
        {
            module.Id = modules.Max(x => x.Id) + 1;
            modules.Add(module);
            return module;
        }
        public static string ModuleName(int moduleId)
        {
            return modules.FirstOrDefault(x => x.Id == moduleId)?.Name ?? "";
        }   
        public static Module RemoveModule(int moduleId) {
            var module = modules.FirstOrDefault(x => x.Id == moduleId);
            if (module != null)
            {
                modules.Remove(module);
                return module;
            }
            throw new Exception("Module not found");
        }
        public static Module UpdateModule(Module module)
        {
            var index = modules.FindIndex(x => x.Id == module.Id);
            modules[index] = module;
            return module;
        }   
    }
}
