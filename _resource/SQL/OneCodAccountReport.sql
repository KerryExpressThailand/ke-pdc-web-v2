select a.AccountCode,
a.AccountName,
a.AccountNo,
a.CitizenID,
a.BankName,
a.BankBranch,
a.Mobile1,
b.AccountCode,
b.AccountName,
b.AccountNo,
b.CitizenID,
b.BankName,
b.BankBranch,
b.Mobile1,
b.BranchID
from tb_Member_COD as a with (nolock)
inner join
(
		SELECT m.AccountCode,
		m.AccountName,
		m.AccountNo,
		m.CitizenID,
		m.BankName,
		m.BankBranch,
		m.Mobile1,
		n.BranchID
		FROM tb_Member as m (nolock)
		inner join
		(
			SELECT BranchID,
				AccountNo,
				MAX(AccountCode) as AccountCode
			FROM tb_Member as m with (nolock)
			GROUP BY BranchID,
				AccountNo
		) as n
		on m.BranchID = n.BranchID
		and m.AccountNo = n.AccountNo
		and m.AccountCode = n.AccountCode
) as b
on a.AccountNo = b.AccountNo
--where a.RegisterDate >= '2017-07-20 20:00:00.000'
order by a.AccountNo,b.AccountCode