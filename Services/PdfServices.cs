using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Services
{
    public static class PDFService
    {
        public static void SavePDF()
        {
            var deserializedData = UserServices.GetAll();
            var filePath = Utils.GetAppUsersFilePath();
            var appPath = Utils.GetAppDirectoryPath();
            Document.Create(Container =>
            {
                Container.Page(page =>
                {
                    page.Header().Text("Users");
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(column =>
                        {
                            column.RelativeColumn();
                            column.RelativeColumn();
                        });
                        table.Header(header =>
                        {
                            header.Cell().Text("Username");
                            header.Cell().Text("Role");
                        });

                        foreach (var item in deserializedData)
                        {
                            table.Cell().Text(item.Username);
                            table.Cell().Text(item.Role.ToString());
                        }
                    });

                    page.Footer().Text(text =>
                    {
                        text.Span("Page");
                        text.CurrentPageNumber();
                    });
                });
            }).GeneratePdf(Path.Combine(appPath, "users.pdf"));
        }
    }
}
