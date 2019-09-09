	DECLARE @BranchID varchar(20) = 'NLCH';
	DECLARE @DateStart Date = CAST('2017-2-1' AS DATE)
	DECLARE @DateEnd Date = CAST(CAST(2017 AS varchar) + '-' + CAST(2 AS varchar) + '-' + CAST(25 AS varchar) AS DATE);

	DECLARE @PKG03ItemAmount int = 0;
	DECLARE @PKG03ItemExpense int = 0;

	DECLARE @PKG04ItemAmount int = 0;
	DECLARE @PKG04ItemExpense int = 0;

	DECLARE @PKG05ItemAmount int = 0;
	DECLARE @PKG05ItemExpense int = 0;

	DECLARE @PKG07ItemAmount int = 0;
	DECLARE @PKG07ItemExpense int = 0;

	DECLARE @PKG08ItemAmount int = 0;
	DECLARE @PKG08ItemExpense int = 0;

	DECLARE @PKG18ItemAmount int = 0;
	DECLARE @PKG18ItemExpense int = 0;

	SELECT @PKG03ItemAmount = SUM(ISNULL(i.qty,0)),@PKG03ItemExpense = SUM(ISNULL(i.packing_qty,0) * i.unit_price)
	FROM tb_stock_order_item AS i INNER JOIN tb_stock_order_head AS h ON i.order_no = h.order_no LEFT JOIN tb_Package AS p ON i.package_id = p.PackageID
	WHERE (h.branch_id = @BranchID) AND (p.PackageID = 'PKG03') AND (h.created_date BETWEEN @DateStart AND @DateEnd) AND (h.[status] = 4 ) AND PackageType =1
	GROUP BY p.PackageID,p.PackageDesc,p.unit
     
	SELECT @PKG04ItemAmount = SUM(ISNULL(i.qty,0)),@PKG04ItemExpense = SUM(ISNULL(i.packing_qty,0) * i.unit_price)
	FROM tb_stock_order_item AS i INNER JOIN tb_stock_order_head AS h ON i.order_no = h.order_no LEFT JOIN tb_Package AS p ON i.package_id = p.PackageID
	WHERE (h.branch_id = @BranchID) AND (p.PackageID = 'PKG04') AND (h.created_date BETWEEN @DateStart AND @DateEnd) AND (h.[status] = 4 ) AND PackageType =1
	GROUP BY p.PackageID,p.PackageDesc,p.unit
     
	SELECT @PKG05ItemAmount = SUM(ISNULL(i.qty,0)),@PKG05ItemExpense = SUM(ISNULL(i.packing_qty,0) * i.unit_price)
	FROM tb_stock_order_item AS i INNER JOIN tb_stock_order_head AS h ON i.order_no = h.order_no LEFT JOIN tb_Package AS p ON i.package_id = p.PackageID
	WHERE (h.branch_id = @BranchID) AND (p.PackageID = 'PKG05') AND (h.created_date BETWEEN @DateStart AND @DateEnd) AND (h.[status] = 4 ) AND PackageType =1
	GROUP BY p.PackageID,p.PackageDesc,p.unit
     
	SELECT @PKG07ItemAmount = SUM(ISNULL(i.qty,0)),@PKG07ItemExpense = SUM(ISNULL(i.packing_qty,0) * i.unit_price)
	FROM tb_stock_order_item AS i INNER JOIN tb_stock_order_head AS h ON i.order_no = h.order_no LEFT JOIN tb_Package AS p ON i.package_id = p.PackageID
	WHERE (h.branch_id = @BranchID) AND (p.PackageID = 'PKG07') AND (h.created_date BETWEEN @DateStart AND @DateEnd) AND (h.[status] = 4 ) AND PackageType =1
	GROUP BY p.PackageID,p.PackageDesc,p.unit
	
	SELECT @PKG08ItemAmount = SUM(ISNULL(i.qty,0)),@PKG08ItemExpense = SUM(ISNULL(i.packing_qty,0) * i.unit_price)
	FROM tb_stock_order_item AS i INNER JOIN tb_stock_order_head AS h ON i.order_no = h.order_no LEFT JOIN tb_Package AS p ON i.package_id = p.PackageID
	WHERE (h.branch_id = @BranchID) AND (p.PackageID = 'PKG08') AND (h.created_date BETWEEN @DateStart AND @DateEnd) AND (h.[status] = 4 ) AND PackageType =1
	GROUP BY p.PackageID,p.PackageDesc,p.unit

	SELECT @PKG18ItemAmount = SUM(ISNULL(i.qty,0)),@PKG18ItemExpense = SUM(ISNULL(i.packing_qty,0) * i.unit_price)
	FROM tb_stock_order_item AS i INNER JOIN tb_stock_order_head AS h ON i.order_no = h.order_no LEFT JOIN tb_Package AS p ON i.package_id = p.PackageID
	WHERE (h.branch_id = @BranchID) AND (p.PackageID = 'PKG18') AND (h.created_date BETWEEN @DateStart AND @DateEnd) AND (h.[status] = 4 ) AND PackageType =1
	GROUP BY p.PackageID,p.PackageDesc,p.unit

SELECT
	 @PKG03ItemAmount AS 'Box S'
	,@PKG03ItemExpense AS 'Box S'

	,@PKG04ItemAmount AS 'Box S'
	,@PKG04ItemExpense AS 'Box S'

	,@PKG05ItemAmount AS 'Box S'
	,@PKG05ItemExpense AS 'Box S'

	,@PKG07ItemAmount AS 'Box S'
	,@PKG07ItemExpense AS 'Box S'

	,@PKG08ItemAmount AS 'Box S'
	,@PKG08ItemExpense AS 'Box S'

	,@PKG18ItemAmount AS 'Box S'
	,@PKG18ItemExpense AS 'Box S'