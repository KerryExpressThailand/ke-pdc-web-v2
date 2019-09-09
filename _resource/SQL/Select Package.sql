DECLARE @branch_id varchar(10) = 'BSTO';
DECLARE @Month varchar(2) = 3
DECLARE @Year varchar(4) = 2017

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

SELECT p.PackageID,
  p.PackageDesc as p_desc,
  p.unit as unit,
  i.unit_price,
  SUM(ISNULL(i.qty,0)) as ord_qty,

  SUM(ISNULL(i.packing_qty,0)) as pkg_qty,
  SUM(ISNULL(i.packing_qty,0) * i.unit_price) As amt
FROM tb_stock_order_item as i 
   INNER JOIN tb_stock_order_head as h ON i.order_no = h.order_no
   LEFT JOIN tb_Package as p ON i.package_id = p.PackageID
WHERE (h.branch_id = @branch_id)
  AND (p.PackageID in ('PKG03','PKG04','PKG05',/*'PKG06',*/'PKG07','PKG08','PKG18'/*,'PKG01','PKG02'*/))
  and (convert(varchar(8),h.delivery_date,112) BETWEEN @DateStart and @DateEnd)
  AND (h.[status] IN (3, 4) )
  AND PackageType = 1

GROUP BY p.PackageID,p.PackageDesc,p.unit,i.unit_price
ORDER BY i.unit_price ASC