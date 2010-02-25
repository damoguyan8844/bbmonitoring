/*
 * http://stackoverflow.com/questions/158706/how-to-properly-clean-up-excel-interop-objects-in-c
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace JOYFULL.CMPW.Report
{
    public class Exportor
    {
        Excel.Application _app;
        Excel.Workbook _book;
        Excel.Worksheet _sht;
        public Exportor()
        {
        }

        public void NewBook()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if( _app == null )
                _app = new Excel.Application();
            if (_book != null)
            {
                _book.Close(false, Type.Missing, Type.Missing);
                Marshal.FinalReleaseComObject( _book );
            }
            if( _sht != null )
            {
                Marshal.FinalReleaseComObject( _sht );
            }
            _book =
                _app.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            _sht = _book.ActiveSheet as Excel.Worksheet;
        }

        public void Write( int row, int col, object value )
        {
            Range range = _sht.Cells[ row, col ] as Range;
            if ( range == null ) return;
            object[] arg = new object[ 1 ];
            arg[ 0 ] = value;
            range.GetType().InvokeMember( "Value", BindingFlags.SetProperty, null, range, arg );
            Marshal.FinalReleaseComObject( range );
        }

        public void Save( string path, string password )
        {
            //_book.Password = password;
            //_book.SaveAs( path, Excel.XlFileFormat.xlExcel7,
            //    Type.Missing, Type.Missing, false, false,
            //    Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing,
            //    Type.Missing, Type.Missing, Type.Missing );
            _book.Password = password;
            Save( path );
        }

        public void Save( string path )
        {
            _book.SaveAs( path, Excel.XlFileFormat.xlWorkbookNormal,
                Type.Missing, Type.Missing, false, false,
                Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing );
        }

        public void Dispose()
        {

            Marshal.FinalReleaseComObject( _sht );
            if( _book != null )
            {
                _book.Close( false, Type.Missing, Type.Missing );
                Marshal.FinalReleaseComObject( _book );
                _book = null;
            }
            
            if (_app != null)
            {
                _app.Quit();
                Marshal.FinalReleaseComObject( _app );
                _app = null;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
    }
}
