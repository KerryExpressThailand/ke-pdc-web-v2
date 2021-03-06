DECLARE @BranchID varchar(20) = 'AMTA';
DECLARE @Month int = 3;
DECLARE @Year int = 2017;
-------------------------------------------------------

DECLARE @DateStart Date = CAST(CAST(@Year AS varchar) + '-' + CAST(@Month AS varchar) + '-' + CAST(26 AS varchar) AS DATE);
DECLARE @DateEnd Date = CAST(CAST(@Year AS varchar) + '-' + CAST(@Month AS varchar) + '-' + CAST(25 AS varchar) AS DATE);

IF(@Month = 2 AND @Year <= 2017) BEGIN
	SET @DateStart = CAST('2017-2-1' AS DATE);
END
ELSE
BEGIN
	SET @DateStart = DateAdd(MONTH, -1, @DateStart)
END

SELECT i.order_no
      ,i.package_id
	  ,PackageType
	  ,p.PackageDesc
      ,i.balance
      ,i.qty
      ,i.unit_price
      ,i.[status]
      ,i.packing_qty
	  --,SUM(ISNULL(i.qty,0)) AS TotalQuantity
	  ,ISNULL(i.packing_qty,0) * i.unit_price AS TotalPrice
  FROM [POS].[dbo].[tb_stock_order_item] AS i
	  INNER JOIN tb_stock_order_head as h ON i.order_no = h.order_no
	  LEFT JOIN tb_Package as p ON i.package_id = p.PackageID
  WHERE (h.branch_id = @BranchID)
		AND (CONVERT(DATE, h.created_date) BETWEEN @DateStart AND @DateEnd)
		AND (h.[status] = 4)
		AND (
			PackageType = 5
			OR (p.PackageID IN ('PKG03','PKG04','PKG05',/*'PKG06',*/'PKG07','PKG08','PKG18'/*,'PKG01','PKG02'*/))
		)
  ORDER BY PackageType, i.unit_price