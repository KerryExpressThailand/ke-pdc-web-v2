declare @ReportDate varchar(8);
declare @BranchID varchar(10);
set @ReportDate='20161208';
set @BranchID='ASK'

declare @shop_type varchar(2);
declare @code varchar(6);
declare @receipt_date varchar(10);
declare @id varchar(20);
declare @bid varchar(10);
declare @report_date varchar(10);
declare @freight money;
declare @transport money;
declare @am money;
declare @pup money;
declare @sat money;
declare @ras money;
declare @cod money;
declare @insur money;
declare @pkg money;
declare @sale_package money;
declare @line_topup money;
declare @cash money;
declare @rabbit money;
declare @credit money;
declare @line_pay money;
declare @transaportation money;
declare @vas_surcharge money;
declare @discount money;
declare @vat money;
declare @total money;
declare @city money;
declare @cityn money;
declare @grab money;
declare @bsd_linepay money;
declare @bsd_cash money;
declare @bsd_line_topup money;
declare @total_transfer money;
declare @bonus_commission money;
declare @adj_other money;
declare @return_charge money;
declare @suspense money;
declare @suspense_remark nvarchar(150);
declare @with_holding_tax money;
declare @promotion money;
declare @total_con int;
declare @total_boxes int;
declare @verify_date varchar(20);
declare @captured varchar(5);
declare @captured_date varchar(10);
declare @captured_by nvarchar(20);


SELECT 
@shop_type = cast(case when upper(b.branch_type)='KE-SHOP' then 'KE'
when upper(b.branch_type)='FC-SHOP' then 'FC'
when upper(b.branch_type)='DC-SHOP' then 'KE'
when upper(b.branch_type)='DCSP-SHOP' then 'KE'
end  as nvarchar(100)),
@code = cast(ISNULL(b.ERP_ID,'-') as nvarchar(100)),
@receipt_date = cast('' as nvarchar(100)),
@id = cast(d.BranchID+'-'+convert(varchar,ReportDate,112) as nvarchar(100)),
@bid = cast(ISNULL(b.DMSID,'-') as nvarchar(100)),
@report_date= cast(format(dateadd(year,0,d.ReportDate),'d/M/yyyy') as nvarchar(10)),
@freight = cast(ISNULL(d.FreightSurcharge,0)+
cast(ISNULL(d.AMSurcharge,0)+ISNULL(d.PUPSurcharge,0)+
ISNULL(d.SatDelSurcharge,0)+ISNULL(d.RemoteAreaSurcharge,0) as nvarchar(100)) as nvarchar(100)),
@transport= cast(ISNULL(d.FreightSurcharge,0) as nvarchar(100)),
@am = cast(ISNULL(d.AMSurcharge,0) as nvarchar(100)),
@pup = cast(ISNULL(d.PUPSurcharge,0) as nvarchar(100)),
@sat = cast(ISNULL(d.SatDelSurcharge,0) as nvarchar(100)),
@ras = cast(ISNULL(d.RemoteAreaSurcharge,0) as nvarchar(100)),
@cod = cast(ISNULL(d.CODSurcharge,0) as nvarchar(100)),
@insur = cast(ISNULL(d.InsurSurcharge,0) as nvarchar(100)),
@pkg = cast(ISNULL(d.PkgSurcharge,0) as nvarchar(100)),
@sale_package = cast(ISNULL(d.PkgService,0) as nvarchar(100)),
@line_topup = cast(ISNULL(d.LineTopUpService,0) as nvarchar(100)),
@cash = cast(ISNULL(d.Cash,0) + ISNULL(d.CashForService,0) + ISNULL(d.LineTopUpService,0) as nvarchar(100)),
@rabbit = cast(ISNULL(d.Rabbit,0) + ISNULL(d.RabbitForService,0) as nvarchar(100)),
@credit = cast(ISNULL(d.Credit,0) + ISNULL(d.CreditForService,0) as nvarchar(100)),
@line_pay = cast(ISNULL(d.LinePay,0) + ISNULL(d.LinePayForService,0) as nvarchar(100)),
@transaportation = cast(ISNULL(d.FreightSurcharge,0) as nvarchar(100)),
@vas_surcharge = cast(ISNULL(d.AMSurcharge,0)+ ISNULL(d.PUPSurcharge,0)+ ISNULL(d.SatDelSurcharge,0)+ ISNULL(d.RemoteAreaSurcharge,0)+ ISNULL(d.CODSurcharge,0)+ ISNULL(d.InsurSurcharge,0)+ ISNULL(d.PkgSurcharge,0)+ ISNULL(d.PkgService,0)+ ISNULL(d.LineTopUpService,0) - ISNULL(d.VatSurcharge,0) - ISNULL(d.VatService,0) as nvarchar(100)),
@discount = cast(ISNULL(d.DiscountSurcharge,0) as nvarchar(100)),
@vat = cast(ISNULL(d.VatSurcharge,0) + ISNULL(d.VatService,0) as nvarchar(100)),
@total = cast(ISNULL(d.FreightSurcharge,0) + (ISNULL(d.AMSurcharge,0) + ISNULL(d.PUPSurcharge,0) + ISNULL(d.SatDelSurcharge,0) + ISNULL(d.RemoteAreaSurcharge,0) + ISNULL(d.CODSurcharge,0) + ISNULL(d.InsurSurcharge,0) + ISNULL(d.PkgSurcharge,0) + ISNULL(d.PkgService,0) + ISNULL(d.LineTopUpService,0) - ISNULL(d.VatSurcharge,0) - ISNULL(d.VatService,0)) - isnull(d.DiscountSurcharge,0) + (ISNULL(d.VatSurcharge,0) + ISNULL(d.VatService,0)) as nvarchar(100)),
@city = cast(ISNULL(d.CITYSurcharge,0) as nvarchar(100)),
@cityn = cast(ISNULL(d.CITYNSurcharge,0) as nvarchar(100)),
@grab = cast(ISNULL(d.GRABEXSurcharge,0) as nvarchar(100)),
@bsd_linepay =cast(ISNULL(d.BSDLinePay,0) as nvarchar(100)),
@bsd_cash =ISNULL(cast(d.BSDCash as nvarchar(100)), '0'),
@bsd_line_topup = cast(ISNULL(d.BSDLineTopUp,0) as nvarchar(100)),
@total_transfer = cast(CASE WHEN b.branch_type = 'KE-SHOP' THEN ISNULL(d.Cash,0) + ISNULL(d.CashForService,0) + ISNULL(d.LineTopUpService,0) + ISNULL(d.BSDCash,0) + ISNULL(d.BSDLineTopUp,0) ELSE ISNULL(d.Cash,0) + ISNULL(d.CashForService,0) + ISNULL(d.LineTopUpService,0) + ISNULL(d.BSDCash,0) + ISNULL(d.BSDLineTopUp,0) - ISNULL(d.CODSurcharge,0) - ISNULL(d.InsurSurcharge,0) - ISNULL(d.PkgSurcharge,0) - ISNULL(d.PkgService,0) END as nvarchar(100)),
@bonus_commission = cast(ISNULL(d.BonusCommission,0) as nvarchar(100)),
@adj_other = cast(ISNULL(d.AdjOther,0) as nvarchar(100)),
@return_charge = cast(ISNULL(d.ReturnCharge,0) as nvarchar(100)),
@suspense = cast(ISNULL(d.Suspense,0) as nvarchar(100)),
@suspense_remark = isnull(d.SuspenseRemark,'-'),
@with_holding_tax = cast(ISNULL(d.WithHoldingTax,0) as nvarchar(100)),
@promotion = cast(ISNULL(d.Promotion,0) as nvarchar(100)),
@total_con = cast(ISNULL(d.Con,0) as nvarchar(100)),
@total_boxes = cast(ISNULL(d.Boxes,0) as nvarchar(100)),
@verify_date = cast(format(dateadd(year,0,d.VerifyDate),'d/M/yyyy H:mm:ss') as nvarchar(10)),
@captured = cast(case when isnull(d.Captured,0)=1 then 'TRUE' else 'FALSE' end as nvarchar(10)),
@captured_date = cast(format(dateadd(year,0,d.CapturedDate),'d/M/yyyy')  as nvarchar(10)),
@captured_by = cast(d.CapturedBy as nvarchar(100))
FROM tb_daily_revenue as d
inner join tb_branch as b
on d.BranchID = b.BranchID
where d.BranchID = @BranchID
and d.ReportDate  = @ReportDate


declare @tempTable table (ShopType varchar(2), OracleDC varchar(6), Sale_Date varchar(20),
Receipt_Date varchar(20),Oracle_Account varchar(100),Item_Name varchar(100),Debit money,
Credit money,Total money,Remark nvarchar(100),SortBy varchar(1),Company varchar(2),Account varchar(6),
BU varchar(2),Cost_center varchar(6),DC_Del varchar(4),Truck varchar(2),reserve1 varchar(2),
reserve2 varchar(3),reserve3 varchar(3))

IF   
@shop_type ='KE'
BEGIN  
insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.111282.01.000000.0000.00.00.000.000',
'Cheque In Transit -BBL HQ', @cash+@suspense+@adj_other ,0.00, @cash+@suspense+@adj_other  ,'Cash',
'1','01','111282','01','000000','0000','00','00','000','000')
END  
ELSE   
BEGIN
insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.111282.01.000000.0000.00.00.000.000',
'Cheque In Transit -BBL HQ', @cash-(@cod+@insur+@pkg+@sale_package)+@suspense+@adj_other ,0.00,  @cash-(@cod+@insur+@pkg+@sale_package)+@suspense+@adj_other  ,'Cash',
'1','01','111282','01','000000','0000','00','00','000','000')
END ;

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.111282.01.000000.0000.00.00.000.000',
'Cheque In Transit -BBL HQ', @rabbit ,0.00, @rabbit  ,'Rabbit',
'1','01','111282','01','000000','0000','00','00','000','000')

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.111282.01.000000.0000.00.00.000.000',
'Cheque In Transit -BBL HQ', @credit+@bonus_commission ,0.00, @credit+@bonus_commission  ,'Credit',
'1','01','111282','01','000000','0000','00','00','000','000')

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.112190.01.000000.0000.00.00.000.000',
'AR Suspense Account', @line_pay ,0.00, @line_pay ,'LinePay',
'1','01','112190','01','000000','0000','00','00','000','000')

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.112190.01.000000.0000.00.00.000.000',
'AR Suspense Account',0.00 ,@line_topup, -1*@line_topup ,'TOPUP',
'1','01','112190','01','000000','0000','00','00','000','000')

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.112190.01.000000.0000.00.00.000.000',
'AR Suspense Account', 0.00 ,@suspense, -1*@suspense  ,@suspense_remark,
'1','01','112190','01','000000','0000','00','00','000','000')

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.770120.01.'+ @code +'.0000.00.00.000.000',
'Bank Commission',-1*@bonus_commission ,0.00, -1*@bonus_commission  ,'Commission Credit',
'1','01','770120','01',@code,'0000','00','00','000','000')

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.760230.01.'+ @code +'.0000.00.00.000.000',
'Shortage/Overrage', 0.00 ,@adj_other, -1*@adj_other ,'Shortage/Overrage',
'1','01','760230','01',@code,'0000','00','00','000','000')

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.113401.01.000000.0000.00.00.000.000',
'Withholding Tax - Current Year',@with_holding_tax ,0.00, @with_holding_tax  ,'Withholding Tax',
'1','01','113401','01','000000','0000','00','00','000','000')

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.401101.01.'+@code+'.0000.00.00.000.000',
'Freight Revenue', @city ,0.00,@city ,'City',
'1','01','401101','01',@code,'0000','00','00','000','000')

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.401101.01.'+@code+'.0000.00.00.000.000',
'Freight Revenue', @cityn ,0.00, @cityn ,'CityN',
'1','01','401101','01',@code,'0000',
'00','00','000','000')

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.111282.01.'+@code+'.0000.00.00.000.000',
'Freight Revenue', @grab,0.00, @grab ,'Grab',
'1','01','401101','01',@code,'0000','00','00','000','000')

IF   
@shop_type ='KE'
BEGIN  
insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.213401.01.000000.0000.00.00.000.000',
'Output Vat', 0.00 ,@vat, -1*@vat ,'Output Vat',
'1','01','213401','01','000000','0000','00','00','000','000')
END  
ELSE   
BEGIN  
insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.213401.01.000000.0000.00.00.000.000',
'Output Vat', 0.00 ,0.00, 0.00 ,'Output Vat',
'1','01','213401','01','000000','0000','00','00','000','000')
END ; 

IF   
@shop_type ='KE'
BEGIN  
insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.409103.01.'+@code+'.0000.00.00.000.000',
'Other income', 0.00 ,@pkg+@sale_package-round(((@pkg+@sale_package)*7/107),2),-1*@pkg+@sale_package-round(((@pkg+@sale_package)*7/107),2) ,'Package+Sale Package',
'1','01','409103','01',@code,'0000','00','00','000','000')
END  
ELSE   
BEGIN  
insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.409103.01.'+@code+'.0000.00.00.000.000',
'Other income', 0.00 ,0.00,0.00 ,'Package+Sale Package',
'1','01','409103','01',@code,'0000','00','00','000','000')
END ;

IF   
@shop_type ='KE'
BEGIN  
insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.401202.01.'+@code+'.0000.00.00.000.000',
'Insurance', 0.00 ,@insur-round((@insur*7/107),2), -1*@insur-round((@insur*7/107),2) ,'Insurance',
'1','01','401202','01',@code,'0000','00','00','000','000')
END  
ELSE   
BEGIN  
insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.401202.01.'+@code+'.0000.00.00.000.000',
'Insurance', 0.00 ,0.00, 0.00 ,'Insurance',
'1','01','401202','01',@code,'0000','00','00','000','000')
END ;

IF   
@shop_type ='KE'
BEGIN  
insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.401201.01.'+@code+'.0000.00.00.000.000',
'COD Surcharge',0.00 ,@cod-round((@cod*7/107),2),-1*@cod-round((@cod*7/107),2) ,'COD Surcharge',
'1','01','401201','01',@code,'0000','00','00','000','000')
END  
ELSE   
BEGIN  
insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.401201.01.'+@code+'.0000.00.00.000.000',
'COD Surcharge',0.00 ,0.00,0.00,'COD Surcharge',
'1','01','401201','01',@code,'0000','00','00','000','000')
END ;

insert into @tempTable ( ShopType, OracleDC,Sale_Date,Receipt_Date,Oracle_Account,Item_Name,
Debit,Credit,Total,Remark,SortBy,Company,Account,BU,Cost_center,DC_Del,Truck,reserve1,
reserve2,reserve3) values(@shop_type,@code,@report_date,@verify_date,
'01.401101.01.'+@code+'.0000.00.00.000.000',
'Freight Revenue', 0.00 ,@freight, -1*@freight ,'Freight Revenue',
'1','01','401101','01',@code,'0000','00','00','000','000')


select * from @tempTable as t