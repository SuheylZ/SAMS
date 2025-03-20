using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DataProxy
{
    public class OrderSearch
    {
        public static IDataReader CustomerName(string sname)
        {
            Storage store = Storage.Get();
            sname = "%" + sname + "%";
            return store.ExecuteReader(@"Select O.id, O.ReferenceNo, C.id 'customerid', C.title+' '+C.firstname+' '+C.lastname 'Customer Name', 
                        C.address 'Address', 
                        O.PurchaseDate, O.UnderlayType, O.UnderlayQty 'Quantity', O.FittingDate, O.MeasurementDate, O.Deposit, O.total-O.Deposit 'Balance', O.Total  
                        From PurchaseOrder O inner join Customer C on O.customerid=C.id Where (C.firstname like @name) OR (C.lastname like @name)", sname);
        }

        public static IDataReader PurchaseDate(DateTime dtpurchase)
        {
            Storage store = Storage.Get();
            return store.ExecuteReader(@"select O.id, O.ReferenceNo, C.id 'customerid', C.title+' '+C.firstname+' '+C.lastname 'Customer Name', 
                            C.address 'Address', 
                            O.PurchaseDate, O.UnderlayType, O.UnderlayQty 'Quantity', O.FittingDate, O.MeasurementDate, O.Deposit, O.total-O.Deposit 'Balance', O.Total  
                            from PurchaseOrder O inner join Customer C On O.customerid=C.id where DateDiff(day, O.purchasedate, @date) =0", dtpurchase);
        }

        public static IDataReader ReferenceNo(string sRef)
        {
            Storage store = Storage.Get();
            sRef = "%" + sRef + "%";
            return store.ExecuteReader(@"select O.id, O.ReferenceNo, C.id 'customerid', C.title+' '+C.firstname+' '+C.lastname 'Customer Name', 
                                C.address 'Address', 
                                O.PurchaseDate, O.UnderlayType, O.UnderlayQty 'Quantity', O.FittingDate, O.MeasurementDate, O.Deposit, O.total-O.Deposit 'Balance', O.Total  
                                from PurchaseOrder o inner join Customer C On o.customerid=C.id where (o.ReferenceNo like @ref)", sRef);
        }

        public static IDataReader PurchaseOrder(int id)
        {
            Storage store = Storage.Get();
            return store.ExecuteReader(@"select  O.id, O.ReferenceNo, C.id 'customerid', C.title+' '+C.firstname+' '+C.lastname 'Customer Name', 
                            C.address 'Address', 
                            O.PurchaseDate, O.UnderlayType, O.UnderlayQty 'Quantity', O.FittingDate, O.MeasurementDate, O.Deposit, O.total-O.Deposit 'Balance', O.Total  
                            from PurchaseOrder o inner join Customer C On O.customerid=C.id where O.id = @id", id);
        }
    }


    public class CustomerSearch
    {
        public static IDataReader ByName(string svalue)
        {
            Storage st = Storage.Get();
            svalue = "%" + svalue + "%";
            return st.ExecuteReader(@"Select C.title+' '+C.firstname+' '+C.midname+' '+C.lastname as fullname, C.address, C.postalcode, C.phone, C.cell from Customer C where (C.firstname like @name)OR(C.lastname like @name)OR(C.midname like @name)OR(C.title like @name)", svalue);
        }

        public static IDataReader ByAddress(string svalue)
        {
            Storage st = Storage.Get();
            svalue = "%" + svalue + "%";
            return st.ExecuteReader(@"Select C.title+' '+C.firstname+' '+C.midname+' '+C.lastname as 'FullName', C.address, C.postalcode, C.phone, C.cell from Customer C where (C.address like @name) OR (C.postalcode like @name)", svalue);
        }

        public static IDataReader ByPhone(string svalue)
        {
            Storage st = Storage.Get();
            svalue = "%" + svalue + "%";
            return st.ExecuteReader(@"Select C.title+' '+C.firstname+' '+C.midname+' '+C.lastname as 'FullName', C.address, C.postalcode, C.phone, C.cell from Customer C where (C.phone like @name) OR (C.cell like @name)", svalue);
        }


    }
}
