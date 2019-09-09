using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KE_PDC
{
    public class Pagination
    {
        public int Page = 1;
        public int PerPage = 10;
        public string SearchPhrase = null;
        public List<WhereSearch> WhereSearch = new List<WhereSearch>();
        public List<Order> Orders = new List<Order>();
        public string OrderRequest = null;
        public string DirectionRequest = null;
        public bool HasSearch = false;
        public StringBuilder SQLStringSelect = new StringBuilder();
        public StringBuilder SQLStringWhere = new StringBuilder();
        public StringBuilder SQLStringOrder = new StringBuilder();
        public StringBuilder SQLStringLimit = new StringBuilder();

        public Pagination(HttpContext context)
        {
            Int32.TryParse(context.Request.Method.Equals("POST") ? context.Request.Form["page"] : context.Request.Query["page"], out Page);
            Int32.TryParse(context.Request.Method.Equals("POST") ? context.Request.Form["perPage"] : context.Request.Query["perPage"], out PerPage);
            SearchPhrase = context.Request.Method.Equals("POST") ? context.Request.Form["searchPhrase"] : context.Request.Query["searchPhrase"];
            OrderRequest = context.Request.Method.Equals("POST") ? context.Request.Form["order"].ToString().ToLower() : context.Request.Query["order"].ToString().ToLower();
            DirectionRequest = String.IsNullOrEmpty(context.Request.Query["direction"].ToString()) ? "desc" : context.Request.Query["direction"].ToString();
            HasSearch = String.IsNullOrEmpty(SearchPhrase) ? false : true;
        }

        public int From()
        {
            return (PerPage > 0 ? PerPage : 0) * (Page - 1);
        }

        public int To()
        {
            return PerPage;
        }

        public string GetDirection(string direction)
        {
            string _direction;

            switch (direction.ToLower())
            {
                case "asc":
                    _direction = "ASC";
                    break;
                default:
                    _direction = "DESC";
                    break;
            }

            return _direction;
        }

        public void AddColumnSearch(string[] where)
        {
            for (int i = 0; i < where.Length; i++)
            {
                WhereSearch.Add(new WhereSearch { column = where[i] });
            }
        }

        public StringBuilder AddSQLFrom(string sql)
        {
            SQLStringSelect.Append(sql);
            return SQLStringSelect;
        }


        public StringBuilder AddSQLWhere(string sql)
        {
            if (!string.IsNullOrEmpty(SQLStringWhere.ToString()))
            {
                SQLStringWhere.Append(" AND ");
            }

            SQLStringWhere.Append(sql);
            return SQLStringWhere;
        }

        public StringBuilder SQLSelect()
        {
            return SQLStringSelect;
        }

        public StringBuilder SQLWhere()
        {
            StringBuilder StringBuilder = new StringBuilder();

            if (WhereSearch.Count > 0 && !string.IsNullOrEmpty(SearchPhrase))
            {
                WhereSearch.ForEach(ws => {
                    StringBuilder.AppendFormat(" OR {0} LIKE '%{1}%'", ws.column, SearchPhrase);
                });

                StringBuilder.Remove(0, 4);

                StringBuilder.Insert(0, "(").Append(")");
            }

            if (!string.IsNullOrEmpty(SQLStringWhere.ToString()))
            {
                if (!string.IsNullOrEmpty(StringBuilder.ToString()))
                {
                    StringBuilder.Insert(0, " AND ");
                }

                StringBuilder.Insert(0, SQLStringWhere.ToString());
            }


            if (!string.IsNullOrEmpty(StringBuilder.ToString()))
            {
                StringBuilder.Insert(0, " WHERE ");
            }

            return StringBuilder;
        }

        public StringBuilder SQLOrder()
        {
            SQLStringOrder.Clear();
            SQLStringOrder.Append(" ORDER BY (SELECT 1)");


            if (!string.IsNullOrEmpty(OrderRequest))
            {
                Order Order = Orders.FirstOrDefault(o => o.key.Equals(OrderRequest));

                if (Order != null)
                {
                    SQLStringOrder.Clear();
                    SQLStringOrder.AppendFormat(" ORDER BY {0} {1}", Order.column, GetDirection(DirectionRequest));
                }
            }

            return SQLStringOrder;
        }

        public StringBuilder SQLLimit()
        {
            SQLStringLimit.Clear().AppendFormat(" OFFSET {0} ROWS", From());

            if (PerPage > 0)
            {
                SQLStringLimit.AppendFormat(" FETCH NEXT {0} ROWS ONLY", To());
            }

            return SQLStringLimit;
        }

        public string SQLString()
        {
            return SQLSelect().ToString() + SQLWhere().ToString() + SQLOrder().ToString() + SQLLimit().ToString();
        }

        public string SQLTotalString()
        {
            return SQLSelect().ToString() + SQLWhere().ToString();
        }
    }

    public class WhereSearch
    {
        public string column { get; set; }
    }

    public class Order
    {
        public string key { get; set; }
        public string column { get; set; }
    }
}
