using OfficeOpenXml;


namespace CoCApiDealer;
public class ExcelDealer
{

    public ExcelDealer()
    {
        // If you use EPPlus in a noncommercial context
        // according to the Polyform Noncommercial license:
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
        {

        }

    }

    public void CreateExcelFile()
    {



    }



}
