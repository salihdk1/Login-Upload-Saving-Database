using System.Collections.Generic;
using LDap.Models;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace LDap.Helpers
{
    public static class ExcelHelper
    {
        public static List<Model> ProcessExcel(IFormFile file)
        {
            List<Model> models = new List<Model>();

            using (var stream = new MemoryStream())
            {
                // Dosyayı belleğe kopyala
                file.CopyTo(stream);
                stream.Position = 0;

                // Excel dosyasını oku
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RowsUsed();

                    // Başlık satırını atla ve her bir satır için veri işle
                    foreach (var row in rows.Skip(1))
                    {
                        var model = new Model
                        {
                            // Excel dosyasındaki sütunlara göre alanları doldur
                            TcKimlikNo = row.Cell(1).GetValue<int>(),
                            AdıSoyadı = row.Cell(2).GetValue<string>(),
                            Ünvanı = row.Cell(3).GetValue<string>(),
                            ÖrgünEğitimSaat = row.Cell(4).GetValue<int>(),
                            ÖrgünEğitimTutar = row.Cell(5).GetValue<int>(),
                            GelirToplamı = row.Cell(6).GetValue<int>(),
                            DamgaVergisi = row.Cell(7).GetValue<int>(),
                            GelirVergisi = row.Cell(8).GetValue<int>(),
                            GelirVergisiİstisnası = row.Cell(9).GetValue<int>(),
                            DamgaVergisiİstisnası = row.Cell(10).GetValue<int>(),
                            KesintiToplamı = row.Cell(11).GetValue<int>(),
                            NetÖdenecekÜcret = row.Cell(12).GetValue<int>(),
                            Ay = row.Cell(13).GetValue<string>(),
                            Yıl = row.Cell(14).GetValue<int>()
                        };

                        models.Add(model);
                    }
                }
            }

            return models;
        }
    }
}
