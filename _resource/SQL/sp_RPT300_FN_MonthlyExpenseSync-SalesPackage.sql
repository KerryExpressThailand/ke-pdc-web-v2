DECLARE @Month int = 3;
DECLARE @Year int = 2017;

--============================================================
DECLARE @DateStart Date = CAST(CAST(@Year AS varchar) + '-' + CAST(@Month AS varchar) + '-' + CAST(26 AS varchar) AS DATE);
DECLARE @DateEnd Date = CAST(CAST(@Year AS varchar) + '-' + CAST(@Month AS varchar) + '-' + CAST(25 AS varchar) AS DATE);

IF(@Month = 2 AND @Year <= 2017) BEGIN
	SET @DateStart = CAST('2017-2-1' AS DATE);
END
ELSE
BEGIN
	SET @DateStart = DateAdd(MONTH, -1, @DateStart)
END
--============================================================

SELECT @DateStart,@DateEnd

SELECT	 h.branch_id AS BranchID
		,i.package_id AS PackageID
		,p.PackageType
		,i.unit_price AS UnitPrice
		,SUM(i.packing_qty) AS Quantity
		,SUM(i.packing_qty * i.unit_price) AS TotalPrice
FROM tb_stock_order_item AS i WITH(NOLOCK)
INNER JOIN tb_stock_order_head AS h WITH(NOLOCK) ON i.order_no = h.order_no
INNER JOIN tb_Package AS p WITH(NOLOCK) ON i.package_id = p.PackageID
LEFT JOIN tb_monthly_commission AS c WITH(NOLOCK)
	ON h.branch_id = c.branch_id
	AND @Month = c.[month]
	AND @Year = c.[year]
WHERE h.branch_id = 'BSTO'
	AND (
		(
			p.PackageType = 1	/*Sales Package*/
			AND c.sales_package_verify_date IS NULL
			AND (CONVERT(varchar(8), h.delivery_date, 112) BETWEEN @DateStart AND @DateEnd)
		)
		OR
		(
			p.PackageType = 5	/*Supply*/
			AND c.fee_supply_verify_date IS NULL
			AND MONTH(h.delivery_date) = @Month
			AND YEAR(h.delivery_date) = @Year
		)
	)
	AND h.delivery_date IS NOT NULL
	AND h.[status] IN (3, 4)
	AND (
		(
			p.PackageType = 1
			AND i.package_id IN ('PKG03','PKG04','PKG05',/*'PKG06',*/'PKG07','PKG08','PKG18'/*,'PKG01','PKG02'*/)
		)
		OR
		p.PackageType = 5
	)
GROUP BY i.package_id, p.PackageType, h.branch_id, i.unit_price