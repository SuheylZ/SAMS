using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DataProxy
{

    public enum enumPaymentMethod{
        pmUnknown=0,
        pmAMEX=1, 
        pmMaster=2,
        pmVisa=3,
        pmDiscover=4,
        pmCheque=5,
        pmCash=6
    };

    public enum enumEntityType
    {
        etCarpetNumber = 1, 
        etCarpetDesign=2, 
        etCarpetColor=3
    };

    public enum enumOrderState{
        orderNone=0,
        orderAll=1,
        orderFinished=2,
        orderCompleted=3
    };



    public class Order
    {
        public static int Add(int custid, DateTime dtpurchase, string reference, 
            DateTime dtfitting, DateTime dtmeasurement, 
            string request, string underlay, string qty, 
            enumPaymentMethod payment, double dDeposit, double dtotal, double dbalance)
        {
            Storage store = Storage.Get();
            store.ExecuteNonQuery(@"Insert Into PurchaseOrder(CustomerID, PurchaseDate, OrderState, 
            ReferenceNo, FittingDate, MeasurementDate, Underlaytype, underlayQty, Request, 
            PaymentMethod, Deposit, Total, Balance) Values
            (@customerID, @purchasedate, @Orderstate, 
            @reference, @fitting, @measurement, @underlaytype, @underlayqty, @request,
            @paymentMentod, @deposit, @total, @balance)",
            custid, dtpurchase, 0, 
            reference, dtfitting, dtmeasurement, underlay, qty, request,
            Convert.ToInt32(payment), dDeposit, dtotal, dbalance);
            return store.Identity;
        }

        public static void Update(int id, string reference,  DateTime dtPurchase, int custid, 
            DateTime dtfitting, DateTime dtmeasurement, string underlay, string qty, string request, 
            enumPaymentMethod payment, double dDeposit, double dtotal, double dbalance)
       {
            Storage store = Storage.Get();
            store.ExecuteNonQuery(@"update purchaseorder set 
            ReferenceNo=@reference, PurchaseDate=@purchase, Customerid=@custid, 
            fittingdate=@fitting, measurementdate=@measurement, UnderlayType=@underlay, UnderlayQty=@qty, 
            Request=@request, PaymentMethod=@payment, Deposit=@deposit, Total=@total, Balance=@balance  
            Where id=@id",
            reference, dtPurchase, custid, 
            dtfitting, dtmeasurement, underlay, qty, 
            request,Convert.ToInt32(payment), dDeposit, dtotal, dbalance,
            id);
        }

        public static void SaveDoorBars(int id, 
            string sstd, string sstp,
            string dstd, string dstp,
            string cstd, string cstp,
            string zstd, string zstp)
        {
            Storage s = Storage.Get();
            s.ExecuteNonQuery(@"Update PurchaseOrder Set DBSingleSTD=@sstd, DBSingleSTP=@sstp, 
            DBDoubleSTD=@dstd, DBDoubleSTP=@dstp, 
            DBCoverSTD=@cstd, DBCoverSTP=@cstp, 
            DBZSectionSTD=@zstd, DBZSectionSTP=@zstp
            Where id=@id", 
            sstd, sstp, dstd, dstp, cstd, cstp, zstd, zstp, id);
        }


        public static bool Exists(int id)
        {
            Storage st = Storage.Get();
            return st.ExecuteBool("Select count(*) from purchaseorder where id=@id", id);
        }

        public static void ClearDetail(int id)
        {
            Storage store = Storage.Get();
            store.ExecuteNonQuery(@"Delete PurchaseOrderDetail where PurchaseOrderID=@id", id);
        }

        public static void AddDetail(int orderId, int CarpetDetailid, double flength, double fbreadth, int iqty, double fcost, string notes)
        {
            Storage store = Storage.Get();
            store.ExecuteNonQuery(@"Insert into PurchaseOrderDetail(PurchaseOrderID, CarpetDetailID, Length, Breadth, Quantity, Cost, Notes)
                                VALUES(@purchaseid, @carpetdetailid, @len, @width, @qty, @cost, @notes)",
                orderId, CarpetDetailid, flength, fbreadth, iqty, fcost, notes);
        }

        public static IDataReader GetAllOrdersFor(int Custid)
        {
            Storage store = Storage.Get();
            return store.ExecuteReader("Select ID, referenceNo, PurchaseDate, PaymentMethod, Deposit, Total-Deposit as 'Remaining', total From PurchaseOrder where CustomerID =@custid", Custid);
        }

        public static IDataReader GetAllOrder(enumOrderState os)
        {
            Storage db = Storage.Get();
            return db.ExecuteReader(@"select C.id 'customerid', C.title+' '+C.firstname+' '+C.lastname as 'fullname', C.address, C.postalcode, 
                                C.phone, PO.id as 'orderid', PO.purchasedate, PO.orderstate, PO.balance, PO.deposit, PO.total
                                from customer C inner join Purchaseorder PO on C.id=PO.customerid 
                                where PO.orderstate=@os 
                                order by 'fullname'", (int)os);
        }

        public static IDataReader Get(int id)
        {
            Storage st = Storage.Get();
            return st.ExecuteReader("Select * from Purchaseorder where id = @id", id);
        }
        public static int GetCustomerIDOf(int id)
        {
            Storage s = Storage.Get();
            return s.ExecuteInt(@"select PO.customerid from PurchaseOrder PO where Po.id=@id", id);
        }

        public static IDataReader GetDetail(int id)
        {
            Storage st = Storage.Get();
            return st.ExecuteReader("select * from PurchaseOrderDetail where purchaseorderid=@id", id);
        }
        public static IDataReader GetDetail2(int id)
        {
            Storage s = Storage.Get();
            return s.ExecuteReader(@"select OD.id, C.CarpetCode 'carpet', CC.colorcode 'Color', CDN.designCode 'Backing', CD.CarpetNo 'No', 
                                    OD.Length, OD.Breadth, OD.Quantity, OD.Cost, OD.Notes
                                    from PurchaseOrderDetail OD 
                                    inner join CarpetDetail CD on OD.CarpetDetailID=CD.id
                                    inner join Carpet C on CD.CarpetID=C.id
                                    inner join CarpetColor CC on CD.CarpetColorID=CC.ID
                                    inner join CarpetDesign CDN on CD.CarpetDesignID=CDN.id
                                    Where OD.PurchaseOrderID=@id", id);
        }
        public static IDataReader GetAll()
        {
            Storage s = Storage.Get();
            return s.ExecuteReader(@"Select O.id, O.ReferenceNo, C.id 'customerid', C.title+' '+C.firstname+' '+C.lastname 'Customer Name', 
                C.address 'Address', O.PurchaseDate, O.UnderlayType, O.UnderlayQty 'Quantity', O.FittingDate, O.MeasurementDate, O.Deposit, O.total-O.Deposit 'Balance', O.Total  
                from purchaseorder O inner join Customer C on O.customerid=C.id");
        }

        public static void Delete(int id)
        {
            Storage s = Storage.Get();
            s.ExecuteNonQuery(@"Delete purchaseorderdetail where purchaseorderid=@id", id);
            s.ExecuteNonQuery(@"Delete purchaseorder where id=@id", id);
        }
    }

    public class Customer
    {
        public static int Add(string stitle, string sFirst, string sMid, string slast, 
            string sadress, string postcode, string phone, string cell)
        {
            string sql = @"Insert customer(
                        title, firstname, midname, lastname, 
                        address, postalcode, phone, cell 
                        ) VALUES (
                        @title, @first, @mid, @last, 
                        @address, @postcode, @phone, @cell)";
            Storage store = Storage.Get();
            store.ExecuteNonQuery(sql, stitle, sFirst, sMid, slast, sadress, postcode, phone, cell);
            return store.Identity;
        }

        public static void AddDetails(int id, string notes)
        {
            Storage s = Storage.Get();
            s.ExecuteNonQuery(@"update customer set notes=@notes where id=@id", notes, id);
        }

        public static int Exists(string sfirst, string slast)
        {
            int id;
            sfirst = sfirst.Trim();
            slast = slast.Trim();
            Storage store = Storage.Get();
            id = store.ExecuteInt("select id from customer where firstname like @first and lastname like @last", sfirst, slast);
            return id;
        }

        public static IDataReader GetAll()
        {
            StringBuilder sb = new StringBuilder(256);

            sb.Append("select id, title, firstname, midname, lastname, title + ' ' + firstname + ' ' + lastname + ' ' + midname as fullname, address, postalcode, phone, cell, notes from customer order by firstname");
            Storage store = Storage.Get();
            return store.ExecuteReader(sb.ToString());
        }

        public static bool Delete(int id)
        {
            bool bRet = true;
            Storage store = Storage.Get();
            try
            {
                store.ExecuteNonQuery("Delete from Customer where id = @id", id);
            }
            catch 
            {
                bRet = false;
            }
            return bRet;
        }

        public static IDataReader Get(int id)
        {
            Storage st = Storage.Get();
            return st.ExecuteReader("select * from customer where id=@id", id);
        }

        public static void Update(int id, string title, string first, string last, string family, string address, string postalcode, string phone, string cell)
        {
            Storage st = Storage.Get();
            st.ExecuteNonQuery(@"update customer set 
                            title=@title, firstname=@first, lastname=@last, midname=@family, 
                            address=@address, postalcode=@postalcode, phone=@phone, cell=@cell 
                            where id=@id", 
                            title, first, last, family, 
                            address, postalcode, phone, cell, 
                            id); 
        }
    };

    public class Carpet
    {
        public static int Add(string carpet, string Notes)
        {
            Storage store = Storage.Get();
            store.ExecuteNonQuery("Insert Into Carpet(CarpetCode, Notes)values(@code, @note)", carpet, Notes);
            return store.Identity;
        }

        public static IDataReader GetAll()
        {
            Storage s = Storage.Get();
            return s.ExecuteReader(@"select C.id, C.CarpetCode 'Carpet', (select count(*) from carpetdetail where carpetid=C.id) 'Types', C.Notes 'Details' from carpet C");
        }

//        public static IDataReader GetAll(enumEntityType et)
//        {
//            Storage store = Storage.Get();
//            IDataReader rd = null;

//            switch (et)
//            {
//                case enumEntityType.etCarpetNumber:
//                    rd = store.ExecuteReader(@"Select C.id 'CarpetID', C.CarpetCode 'Carpet Code', C.Notes, CN.id 'CarpetDesignID', Cn.DesignCode 'Design Code', 
//                        CC.id 'CarpetColorID', Cc.ColorCode 'Color Code' from  Carpet C inner join CarpetDetail CD on C.id=CD.carpetID 
//                        Inner join CarpetDesign CN on CD.CarpetDesignID = CN.id Inner join CarpetColor CC on CD.CarpetColorid=CC.id
//                        Order by C.id ");
//                    break;

//                case enumEntityType.etCarpetDesign:
//                    rd = store.ExecuteReader("Select * from CarpetDesign");
//                    break;

//                case enumEntityType.etCarpetColor:
//                    rd = store.ExecuteReader("Select * From CarpetColor");
//                    break;
//            }
//            return rd;
//        }

//        public static IDataReader GetCarpetDetail(int iCarpet)
//        {
//            Storage store = Storage.Get();
//            return store.ExecuteReader(@"Select C.id 'CarpetID', C.CarpetCode 'Carpet Code', C.Notes, CN.id 'CarpetDesignID', Cn.DesignCode 'Design Code', 
//                            CC.id 'CarpetColorID', Cc.ColorCode 'Color Code' from 
//                            Carpet C inner join CarpetDetail CD on C.id=CD.carpetID 
//                            Inner join CarpetDesign CN on CD.CarpetDesignID = CN.id
//                            Inner join CarpetColor CC on CD.CarpetColorid=CC.id
//                            where C.id = @id Order by C.id ", iCarpet);

//        }

        //public static int FindDetailORAdd(int icarpet, int idesign, int icolor)
        //{
        //    int id = Exists(icarpet, idesign, icolor);
        //    if (id == 0)
        //    {
        //        Storage store = Storage.Get();
        //        store.ExecuteNonQuery("insert into carpetdetail(carpetid, carpetdesignid, carpetcolorid) values( @a, @b, @c)", 
        //            icarpet, idesign, icolor);
        //        id = store.Identity;
        //    }
        //    return id;
        //}

        public static int Exists(int icarpet, int idesign, int icolor)
        {
            string sql = "select id from carpetdetail where carpetid =@a and carpetdesignid=@b and carpetcolorid=@c";
            Storage store = Storage.Get();
            int id = store.ExecuteInt(sql, icarpet, idesign, icolor);
            return id;
        }

        public static IDataReader Get(int id)
        {
            Storage st = Storage.Get();
            return st.ExecuteReader("Select * from Carpet where id=@id", id);
        }
        public static IDataReader GetDetail(int id)
        {
            Storage st = Storage.Get();
            return st.ExecuteReader(@"select CD.id 'id', CD.CarpetColorID 'ColorID',  CC.ColorCode 'Color', 
                    CD.CarpetDesignID 'DesignID', CDN.DesignCode 'Backing', Cd.Carpetno 'No', 
                    Cd.notes 'notes' 
                    from CarpetDetail CD 
                    inner join CarpetDesign CDN on CD.CarpetDesignid=CDN.id 
                    inner join CarpetColor CC on CD.CarpetColorID = CC.id
                    where CD.carpetid= @id", id);
        }
        public static IDataReader GetDetailByDetailID(int id)
        {
            Storage st = Storage.Get();
            return st.ExecuteReader(@"select CD.id 'id', CD.CarpetID 'carpetid', CD.CarpetColorID 'ColorID',  CC.ColorCode 'Color', 
                    CD.CarpetDesignID 'backingID', CDN.DesignCode 'Backing', Cd.Carpetno 'No', 
                    POD.length, POD.breadth, POD.Quantity, POD.Cost, POD.notes 
                    from purchaseorderdetail POD inner join CarpetDetail CD On PoD.carpetdetailid=Cd.id
                    inner join CarpetDesign CDN on CD.CarpetDesignid=CDN.id 
                    inner join CarpetColor CC on CD.CarpetColorID = CC.id
                    where CD.id= @id", id);
        }

        public static void Delete(int id)
        {
            Storage s = Storage.Get();
            s.ExecuteNonQuery("Delete CarpetDetail where Carpetid = @id", id);
            s.ExecuteNonQuery("Delete Carpet where id=@id", id);
        }
        public static void DeleteDetail(int id)
        {
            Storage s = Storage.Get();
            s.ExecuteNonQuery("Delete CarpetDetail where id = @id", id);
        }

        public static void Update(int id, string Carpet, string Notes)
        {
            Storage s = Storage.Get();
            s.ExecuteNonQuery(@"update carpet set carpetcode=@carpet, notes=@notes where id=@id", Carpet, Notes, id);
        }

        //public static void UpdateColor(int detailid, string color)
        //{
        //    int id = ExistsColor(color);
        //    if (id < 1)
        //        id = AddColor(color);
        //}

        public static int ExistsColor(string color)
        {
            Storage s = Storage.Get();
            color = color.ToLower().Trim();
            return s.ExecuteInt("Select id from carpetcolor where lower(colorcode) like @color", color);
        }
        public static int ExistsBacking(string backing)
        {
            Storage s = Storage.Get();
            backing = backing.ToLower();
            return s.ExecuteInt("Select id from carpetdesign where lower(designcode) like @backing", backing);
        }
        public static int ExistsNo(int icarpet, int icolor, int ibacking, string no)
        {
            int id = 0;
            Storage s = Storage.Get();
            return s.ExecuteInt(@"select id from CarpetDetail 
                            where (carpetid=@carpet) AND 
                            (carpetdesignid=@design) AND 
                            (carpetcolorid=@color) AND
                            (CarpetNo like @no)",
                            icarpet, ibacking, icolor, no);
        }

        public static int AddBacking(string Backing)
        {
            int id = ExistsBacking(Backing);
            if (id < 1)
            {
                Storage s = Storage.Get();
                s.ExecuteNonQuery("Insert into CarpetDesign(DesignCode)values(@design)", Backing);
                id = s.Identity;
            }
            return id;
        }
        public static int AddColor(string color)
        {
            int id = ExistsColor(color);
            if (id < 1)
            {
                Storage s = Storage.Get();
                s.ExecuteNonQuery("Insert into CarpetColor(ColorCode)Values (@color)", color);
                id = s.Identity;
            }
            return id;
        }
        public static int AddDetail(int icarpet, string color, string backing, string No, string notes)
        {
            int icolor, ibacking, idetail;
            
            icolor = ExistsColor(color.Trim());
            if (icolor < 1) icolor = AddColor(color);

            ibacking = ExistsBacking(backing);
            if (ibacking < 1) ibacking = AddBacking(backing);

            idetail = ExistsNo(icarpet, icolor, ibacking, No.Trim());
            if (idetail > 0)
                throw new Exception("Carpet detail already exists");
            else
            {


                Storage s = Storage.Get();
                s.ExecuteNonQuery(@"Insert Into CarpetDetail
                       (Carpetid, CarpetColorid, CarpetDesignid, CarpetNo, Notes) VALUES
                       (@carpet, @color, @design, @no, @notes)",
                        icarpet, icolor, ibacking, No, notes);
                idetail = s.Identity;
            }
            return idetail;
        }

//        private static int AddDetail(int carpetid, int colorid, int backingid, string no, string notes)
//        {
//            Storage s = Storage.Get();
//            s.ExecuteNonQuery(@"insert into CarpetDetail(carpetid, colorid, designid, no, notes) 
//                Values(@carpet, @color, @backing, @no, @notes)", carpetid, colorid, backingid, no, notes);
//            return s.Identity;
//        }
        
        public static void UpdateDetail(int id, int icarpet, string color, string backing, string no, string notes)
        {
            DeleteDetail(id);
            AddDetail(icarpet, color, backing, no, notes);
        }

        public static IDataReader GetColorsFor(int icarpet)
        {
            Storage s = Storage.Get();
            return s.ExecuteReader(@"select distinct CC.id 'id', CC.ColorCode 'color' 
                        from CarpetColor CC inner join CarpetDetail CD on Cd.CarpetColorID=CC.id
                        where CD.Carpetid=@id", icarpet); 
        }

        public static IDataReader GetBackingFor(int icarpet, int icolor)
        {
            Storage s = Storage.Get();
            return s.ExecuteReader(@"select distinct DN.id 'id', DN.DesignCode 'backing' from
                    CarpetDesign DN inner join CarpetDetail CD on DN.id=CD.CarpetDesignID
                    Where (CD.CarpetID=@carpet) AND (CD.CarpetColorID=@color)", icarpet, icolor);
        }

        public static IDataReader GetNosFor(int icarpet, int icolor, int ibacking)
        {
            Storage s = Storage.Get();
            return s.ExecuteReader(@"select distinct CD.CarpetNo 'no' from 
                    CarpetDetail CD Where 
                    CarpetID=@carpet AND CarpetColorID=@color AND CarpetDesignID=@backing", icarpet, icolor, ibacking);
        }
    }

}
