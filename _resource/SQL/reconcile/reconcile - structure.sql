USE [POS]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_reconcile_payment_type]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_verified_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_confirm_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_daily_revenue]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] DROP CONSTRAINT [FK_tb_reconcile_summary_master_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] DROP CONSTRAINT [FK_tb_reconcile_summary_history_tb_reconcile_summary_master]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] DROP CONSTRAINT [FK_tb_reconcile_summary_history_tb_reconcile_payment_type]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] DROP CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_verified_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] DROP CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] DROP CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] DROP CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] DROP CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_confirm_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header] DROP CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header] DROP CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header] DROP CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header] DROP CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header] DROP CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail] DROP CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_reconcile_qr_payment_transaction_header]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail] DROP CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail] DROP CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail] DROP CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction] DROP CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction] DROP CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction] DROP CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction] DROP CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction] DROP CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction] DROP CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction] DROP CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction] DROP CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction] DROP CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction] DROP CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction] DROP CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction] DROP CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction] DROP CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction] DROP CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction] DROP CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_reconcile_card_transaction_header]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_reconcile_card_transaction_header]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card] DROP CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] DROP CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_reconcile_reference_master]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] DROP CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] DROP CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] DROP CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] DROP CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] DROP CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_batch_master] DROP CONSTRAINT [FK_tb_reconcile_batch_master_tb_reconcile_payment_type]
GO
ALTER TABLE [dbo].[tb_reconcile_batch_master] DROP CONSTRAINT [FK_tb_reconcile_batch_master_tb_master_user_created_by]
GO
/****** Object:  Table [dbo].[tb_reconcile_summary_master]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_summary_master]
GO
/****** Object:  Table [dbo].[tb_reconcile_summary_history]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_summary_history]
GO
/****** Object:  Table [dbo].[tb_reconcile_reference_master]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_reference_master]
GO
/****** Object:  Table [dbo].[tb_reconcile_qr_payment_transaction_header]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_qr_payment_transaction_header]
GO
/****** Object:  Table [dbo].[tb_reconcile_qr_payment_transaction_detail]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail]
GO
/****** Object:  Table [dbo].[tb_reconcile_payment_type]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_payment_type]
GO
/****** Object:  Table [dbo].[tb_reconcile_no_bill_transaction]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_no_bill_transaction]
GO
/****** Object:  Table [dbo].[tb_reconcile_linepay_transaction]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_linepay_transaction]
GO
/****** Object:  Table [dbo].[tb_reconcile_ke_pay_transaction]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_ke_pay_transaction]
GO
/****** Object:  Table [dbo].[tb_reconcile_card_transaction_header]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_card_transaction_header]
GO
/****** Object:  Table [dbo].[tb_reconcile_card_transaction_detail]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_card_transaction_detail]
GO
/****** Object:  Table [dbo].[tb_reconcile_card_transaction_by_card]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_card_transaction_by_card]
GO
/****** Object:  Table [dbo].[tb_reconcile_bill_payment_transaction]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_bill_payment_transaction]
GO
/****** Object:  Table [dbo].[tb_reconcile_batch_master]    Script Date: 2/8/2019 12:24:47 PM ******/
DROP TABLE [dbo].[tb_reconcile_batch_master]
GO
/****** Object:  Table [dbo].[tb_reconcile_batch_master]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_batch_master](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[type_id] [varchar](10) NOT NULL,
	[records] [int] NULL,
	[success] [int] NULL,
	[failure] [int] NULL,
	[created_by] [varchar](20) NOT NULL,
	[created_date] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_reconcile_batch_master] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_bill_payment_transaction]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_bill_payment_transaction](
	[id] [varchar](32) NOT NULL,
	[batch_id] [bigint] NOT NULL,
	[branch_id] [varchar](10) NULL,
	[customer_code] [nvarchar](20) NULL,
	[customer_name] [nvarchar](100) NULL,
	[transaction_date] [date] NULL,
	[pay_date] [date] NULL,
	[pay_time] [time](7) NULL,
	[pay_by] [varchar](5) NULL,
	[reference_no] [varchar](20) NULL,
	[fr_br] [varchar](10) NULL,
	[amount] [money] NULL,
	[chq_no] [varchar](20) NULL,
	[bc] [varchar](5) NULL,
	[rc] [varchar](5) NULL,
	[created_by] [varchar](20) NOT NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_bill_payment_transaction] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_card_transaction_by_card]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_card_transaction_by_card](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[header_id] [varchar](32) NOT NULL,
	[card_type] [varchar](20) NOT NULL,
	[card_item] [int] NULL,
	[amount] [money] NULL,
	[commission] [money] NULL,
	[tax] [money] NULL,
	[cr_amt] [money] NULL,
	[tr_amt] [money] NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_card_transaction_by_card] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_card_transaction_detail]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_card_transaction_detail](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[header_id] [varchar](32) NOT NULL,
	[inv_no] [varchar](10) NOT NULL,
	[terminal] [varchar](10) NULL,
	[card_no] [varchar](20) NULL,
	[amount] [money] NULL,
	[transaction_type] [varchar](2) NULL,
	[transaction_date] [date] NULL,
	[auth] [varchar](10) NULL,
	[batch] [varchar](10) NULL,
	[trace_no] [varchar](10) NULL,
	[card_type] [varchar](2) NULL,
	[reference0] [varchar](10) NULL,
	[reference1] [varchar](50) NULL,
	[reference2] [varchar](50) NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_card_transaction_detail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_card_transaction_header]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_card_transaction_header](
	[id] [varchar](32) NOT NULL,
	[batch_id] [bigint] NOT NULL,
	[branch_id] [varchar](10) NULL,
	[branch_name] [nvarchar](100) NULL,
	[merchant_id] [varchar](20) NOT NULL,
	[merchant_idn] [varchar](15) NULL,
	[report_date] [date] NULL,
	[value_date] [date] NULL,
	[account_no] [varchar](10) NULL,
	[branch_no] [varchar](10) NULL,
	[vat_no] [varchar](15) NULL,
	[amount] [money] NULL,
	[commission] [money] NULL,
	[tax] [money] NULL,
	[cr_amt] [money] NULL,
	[tr_amt] [money] NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_credit_card_transaction_header] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_ke_pay_transaction]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_ke_pay_transaction](
	[id] [varchar](32) NOT NULL,
	[batch_id] [bigint] NOT NULL,
	[branch_id] [varchar](10) NULL,
	[transaction_code] [varchar](50) NULL,
	[transaction_date] [datetime] NULL,
	[amount] [money] NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_payment_gateway_transaction] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_linepay_transaction]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_linepay_transaction](
	[id] [varchar](32) NOT NULL,
	[batch_id] [bigint] NOT NULL,
	[merchant_id] [varchar](20) NULL,
	[branch_id] [varchar](10) NULL,
	[transaction_id] [varchar](20) NULL,
	[transaction_ref] [varchar](20) NULL,
	[transaction_date] [datetime] NULL,
	[transaction_type] [varchar](20) NULL,
	[item_amount] [money] NULL,
	[discount_amount] [money] NULL,
	[payment_amount] [money] NULL,
	[payment_method] [varchar](50) NULL,
	[payment_status] [varchar](20) NULL,
	[capture_schema] [varchar](20) NULL,
	[device_id] [varchar](10) NULL,
	[order_number] [varchar](50) NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_linepay_transaction] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_no_bill_transaction]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_no_bill_transaction](
	[id] [varchar](32) NOT NULL,
	[batch_id] [bigint] NOT NULL,
	[merchant_id] [varchar](20) NULL,
	[branch_id] [varchar](10) NULL,
	[erp_id] [varchar](10) NULL,
	[transaction_date] [datetime] NULL,
	[value_date] [date] NULL,
	[description] [nvarchar](100) NULL,
	[transaction_code] [varchar](10) NULL,
	[cheque_no] [varchar](20) NULL,
	[debit] [money] NULL,
	[credit] [money] NULL,
	[balance] [money] NULL,
	[channel] [varchar](50) NULL,
	[terminal_id] [varchar](20) NULL,
	[branch] [varchar](100) NULL,
	[sender_bank] [varchar](10) NULL,
	[sender_barnch] [varchar](50) NULL,
	[sender_account_code] [varchar](20) NULL,
	[sender_account_name] [nvarchar](100) NULL,
	[remark] [nvarchar](100) NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_no_bill_transaction] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_payment_type]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_payment_type](
	[id] [varchar](10) NOT NULL,
	[description] [nvarchar](50) NULL,
 CONSTRAINT [PK_tb_reconcile_payment_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_qr_payment_transaction_detail]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[header_id] [varchar](32) NOT NULL,
	[transaction_date] [date] NULL,
	[transaction_time] [time](3) NULL,
	[transaction_type] [varchar](10) NULL,
	[amount] [money] NULL,
	[terminal_no] [varchar](20) NULL,
	[reference_no] [varchar](20) NULL,
	[approval_code] [varchar](20) NULL,
	[trace_no] [varchar](20) NULL,
	[biller_id] [varchar](20) NULL,
	[vat_no] [varchar](20) NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_qr_payment_transaction_detail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_qr_payment_transaction_header]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_qr_payment_transaction_header](
	[id] [varchar](32) NOT NULL,
	[batch_id] [bigint] NOT NULL,
	[merchant_id] [varchar](20) NULL,
	[branch_id] [varchar](10) NULL,
	[branch_name] [nvarchar](100) NULL,
	[account_no] [varchar](20) NULL,
	[report_date] [datetime] NULL,
	[value_date] [date] NULL,
	[items] [int] NULL,
	[commission] [money] NULL,
	[tax] [money] NULL,
	[cr_amt] [money] NULL,
	[tr_amt] [money] NULL,
	[vat_no] [varchar](20) NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_qr_payment_transaction_header] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_reference_master]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_reference_master](
	[reference_no] [varchar](20) NOT NULL,
	[description] [nvarchar](50) NULL,
 CONSTRAINT [PK_tb_reconcile_reference_master] PRIMARY KEY CLUSTERED 
(
	[reference_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_summary_history]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_summary_history](
	[id] [varchar](32) NOT NULL,
	[branch_id] [varchar](10) NOT NULL,
	[report_date] [date] NOT NULL,
	[type_id] [varchar](10) NOT NULL,
	[amount] [money] NOT NULL,
	[commission] [money] NULL,
	[tax] [money] NULL,
	[verified_flag] [bit] NULL,
	[verified_by] [varchar](20) NULL,
	[verified_date] [datetime] NULL,
	[confirm_flag] [bit] NULL,
	[confirm_by] [varchar](20) NULL,
	[confirm_date] [datetime] NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_summary_history] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tb_reconcile_summary_master]    Script Date: 2/8/2019 12:24:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tb_reconcile_summary_master](
	[branch_id] [varchar](10) NOT NULL,
	[report_date] [date] NOT NULL,
	[type_id] [varchar](10) NOT NULL,
	[amount] [money] NOT NULL,
	[commission] [money] NULL,
	[tax] [money] NULL,
	[verified_flag] [bit] NULL,
	[verified_date] [datetime] NULL,
	[verified_by] [varchar](20) NULL,
	[confirm_flag] [bit] NULL,
	[confirm_date] [datetime] NULL,
	[confirm_by] [varchar](20) NULL,
	[created_by] [varchar](20) NULL,
	[created_date] [datetime] NULL,
	[updated_by] [varchar](20) NULL,
	[updated_date] [datetime] NULL,
	[deleted_by] [varchar](20) NULL,
	[deleted_date] [datetime] NULL,
 CONSTRAINT [PK_tb_reconcile_summary_master] PRIMARY KEY CLUSTERED 
(
	[branch_id] ASC,
	[report_date] ASC,
	[type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tb_reconcile_batch_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_batch_master_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_batch_master] CHECK CONSTRAINT [FK_tb_reconcile_batch_master_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_batch_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_batch_master_tb_reconcile_payment_type] FOREIGN KEY([type_id])
REFERENCES [dbo].[tb_reconcile_payment_type] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_batch_master] CHECK CONSTRAINT [FK_tb_reconcile_batch_master_tb_reconcile_payment_type]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_Branch] FOREIGN KEY([branch_id])
REFERENCES [dbo].[tb_Branch] ([BranchID])
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] CHECK CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] CHECK CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] CHECK CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] CHECK CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_reconcile_batch_master] FOREIGN KEY([batch_id])
REFERENCES [dbo].[tb_reconcile_batch_master] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] CHECK CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_reconcile_reference_master] FOREIGN KEY([reference_no])
REFERENCES [dbo].[tb_reconcile_reference_master] ([reference_no])
GO
ALTER TABLE [dbo].[tb_reconcile_bill_payment_transaction] CHECK CONSTRAINT [FK_tb_reconcile_bill_payment_transaction_tb_reconcile_reference_master]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_reconcile_card_transaction_header] FOREIGN KEY([header_id])
REFERENCES [dbo].[tb_reconcile_card_transaction_header] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_by_card] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_by_card_tb_reconcile_card_transaction_header]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_reconcile_card_transaction_header] FOREIGN KEY([header_id])
REFERENCES [dbo].[tb_reconcile_card_transaction_header] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_detail] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_detail_tb_reconcile_card_transaction_header]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_Branch] FOREIGN KEY([branch_id])
REFERENCES [dbo].[tb_Branch] ([BranchID])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_reconcile_batch_master] FOREIGN KEY([batch_id])
REFERENCES [dbo].[tb_reconcile_batch_master] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_card_transaction_header] CHECK CONSTRAINT [FK_tb_reconcile_card_transaction_header_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_Branch] FOREIGN KEY([branch_id])
REFERENCES [dbo].[tb_Branch] ([BranchID])
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction] CHECK CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction] CHECK CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction] CHECK CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction] CHECK CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_reconcile_batch_master] FOREIGN KEY([batch_id])
REFERENCES [dbo].[tb_reconcile_batch_master] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_ke_pay_transaction] CHECK CONSTRAINT [FK_tb_reconcile_ke_pay_transaction_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_Branch] FOREIGN KEY([branch_id])
REFERENCES [dbo].[tb_Branch] ([BranchID])
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction] CHECK CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction] CHECK CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction] CHECK CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction] CHECK CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_reconcile_batch_master] FOREIGN KEY([batch_id])
REFERENCES [dbo].[tb_reconcile_batch_master] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_linepay_transaction] CHECK CONSTRAINT [FK_tb_reconcile_linepay_transaction_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_Branch] FOREIGN KEY([branch_id])
REFERENCES [dbo].[tb_Branch] ([BranchID])
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction] CHECK CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction] CHECK CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction] CHECK CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction] CHECK CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_reconcile_batch_master] FOREIGN KEY([batch_id])
REFERENCES [dbo].[tb_reconcile_batch_master] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_no_bill_transaction] CHECK CONSTRAINT [FK_tb_reconcile_no_bill_transaction_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail] CHECK CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail] CHECK CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail] CHECK CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_reconcile_qr_payment_transaction_header] FOREIGN KEY([header_id])
REFERENCES [dbo].[tb_reconcile_qr_payment_transaction_header] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_detail] CHECK CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_detail_tb_reconcile_qr_payment_transaction_header]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_Branch] FOREIGN KEY([branch_id])
REFERENCES [dbo].[tb_Branch] ([BranchID])
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header] CHECK CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header] CHECK CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header] CHECK CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header] CHECK CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_reconcile_batch_master] FOREIGN KEY([batch_id])
REFERENCES [dbo].[tb_reconcile_batch_master] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_qr_payment_transaction_header] CHECK CONSTRAINT [FK_tb_reconcile_qr_payment_transaction_header_tb_reconcile_batch_master]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_confirm_by] FOREIGN KEY([confirm_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] CHECK CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_confirm_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] CHECK CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] CHECK CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] CHECK CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_verified_by] FOREIGN KEY([verified_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] CHECK CONSTRAINT [FK_tb_reconcile_summary_history_tb_master_user_verified_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_history_tb_reconcile_payment_type] FOREIGN KEY([type_id])
REFERENCES [dbo].[tb_reconcile_payment_type] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] CHECK CONSTRAINT [FK_tb_reconcile_summary_history_tb_reconcile_payment_type]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_history_tb_reconcile_summary_master] FOREIGN KEY([branch_id], [report_date], [type_id])
REFERENCES [dbo].[tb_reconcile_summary_master] ([branch_id], [report_date], [type_id])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_history] CHECK CONSTRAINT [FK_tb_reconcile_summary_history_tb_reconcile_summary_master]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_Branch] FOREIGN KEY([branch_id])
REFERENCES [dbo].[tb_Branch] ([BranchID])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_Branch]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_daily_revenue] FOREIGN KEY([branch_id], [report_date])
REFERENCES [dbo].[tb_daily_revenue] ([BranchID], [ReportDate])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_daily_revenue]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_confirm_by] FOREIGN KEY([confirm_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_confirm_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_created_by] FOREIGN KEY([created_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_created_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_deleted_by] FOREIGN KEY([deleted_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_deleted_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_updated_by] FOREIGN KEY([updated_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_updated_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_verified_by] FOREIGN KEY([verified_by])
REFERENCES [dbo].[tb_master_user] ([userid])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_master_user_verified_by]
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master]  WITH CHECK ADD  CONSTRAINT [FK_tb_reconcile_summary_master_tb_reconcile_payment_type] FOREIGN KEY([type_id])
REFERENCES [dbo].[tb_reconcile_payment_type] ([id])
GO
ALTER TABLE [dbo].[tb_reconcile_summary_master] CHECK CONSTRAINT [FK_tb_reconcile_summary_master_tb_reconcile_payment_type]
GO
