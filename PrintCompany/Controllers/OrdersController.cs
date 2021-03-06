﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrintCompany.Core;
using PrintCompany.Data;
using PrintCompany.Extensions;
using PrintCompany.ViewModels;
using Rotativa.AspNetCore;

namespace PrintCompany.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly PrintCompanyDbContext _context;
        public IHostingEnvironment _host { get; }

        public OrdersController(PrintCompanyDbContext context, IHostingEnvironment host, IMapper mapper)
        {
            _host = host;
            _context = context;
            _mapper = mapper;
        }

        // GET: Orders
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                var order = _mapper.Map<Order>(orderViewModel);

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction("Edit", "Orders", new { id = order.Id });
            }
            return View(orderViewModel);
        }

        // GET: Orders/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _context.Orders.Include(x => x.Customer).SingleOrDefault(i => i.Id == id.Value);
            if (order == null)
            {
                return NotFound();
            }

            var orderViewModel = _mapper.Map<OrderViewModel>(order);
            orderViewModel.orderLineViewModels = GetOrderLinesForOrderId(order.Id);
            orderViewModel.FileUploads = GetFilesByOrderId(order.Id);
            orderViewModel.orderCustomerContactViewModels = GetCustomerContactsByOrderId(order.Id);

            return View(orderViewModel);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrderViewModel orderViewModel)
        {
            if (id != orderViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var order = _context.Orders.Include(x => x.OrderLines).SingleOrDefault(e => e.Id == id);

                _mapper.Map(orderViewModel, order);

                try
                {
                    _context.Update(order);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orderViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Orders");
            }
            return View(orderViewModel);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Orders");

        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        public PartialViewResult GetOrderLinesForOrderIdPartial(int id)
        {
            List<OrderLineViewModel> orderLineViewModels = GetOrderLinesForOrderId(id);

            return PartialView("_OrderLinesByOrderId", orderLineViewModels);
        }

        private List<OrderLineViewModel> GetOrderLinesForOrderId(int id)
        {
            var orderLinesInOrder = _context.OrderLines
                .Include("ItemType").Include("ItemColor")
                .Include("ItemSize").Include("Supplier")
                .Where(x => x.OrderId == id).ToList();
            List<OrderLineViewModel> orderLineViewModels = new List<OrderLineViewModel>();

            if (orderLinesInOrder.Count > 0)
            {
                foreach (var orderLine in orderLinesInOrder)
                {
                    var orderLineViewModel = _mapper.Map<OrderLineViewModel>(orderLine);

                    orderLineViewModels.Add(orderLineViewModel);
                }
            }
            return orderLineViewModels;
        }

        private IList<FileUpload> GetFilesByOrderId(int id)

        {
            var filesInOrder = _context.FileUploads.Where(x => x.OrderId == id).ToList();
            return filesInOrder;
        }

        public PartialViewResult ReturnFileListByOrderId(int Id)
        {
            var filesInOrder = _context.FileUploads.Where(x => x.OrderId == Id).ToList();
            return PartialView("_PartialFileAttachmentList", filesInOrder);
        }
        private List<OrderCustomerContactViewModel> GetCustomerContactsByOrderId(int id)
        {
            var customerContactsInOrder = _context.OrderCustomerContacts
                .Include("ContactType")
                .Where(x => x.OrderId == id).ToList();
            List<OrderCustomerContactViewModel> customerContactsViewModels = new List<OrderCustomerContactViewModel>();

            if (customerContactsInOrder.Count > 0)
            {
                foreach (var customerContact in customerContactsInOrder)
                {
                    var customerContactViewModel = _mapper.Map<OrderCustomerContactViewModel>(customerContact);

                    customerContactsViewModels.Add(customerContactViewModel);
                }
            }
            return customerContactsViewModels;
        }

        public PartialViewResult GetCustomerContactsByOrderIdPartial(int id)
        {
            List<OrderCustomerContactViewModel> orderCustomerContactViewModels = GetCustomerContactsByOrderId(id);

            return PartialView("_OrderCustomerContactsByOrderId", orderCustomerContactViewModels);
        }


        public IActionResult PrintToPDF(int? id)
        {
            var order = _context.Orders.Include(x => x.Customer).SingleOrDefault(i => i.Id == id.Value);
            if (order == null)
            {
                return NotFound();
            }

            var orderViewModel = _mapper.Map<OrderViewModel>(order);
            orderViewModel.orderLineViewModels = GetOrderLinesForOrderId(order.Id);
            return new ViewAsPdf(orderViewModel);
        }

        public IActionResult CompleteEmbroideryForOrder(int id)
        {
            var orderLinesInOrder = _context.OrderLines
                .Where(x => x.OrderId == id).ToList();

            if (orderLinesInOrder.Count > 0)
            {
                foreach (var orderLine in orderLinesInOrder)
                {
                    orderLine.EmbroideryCompletedQuantity = orderLine.Quantity * orderLine.EmbroideryQuantity;

                    _context.Update(orderLine);
                    _context.SaveChanges();
                }
            }
            return Json("Success");
        }

        public IActionResult CompletePrintForOrder(int id)
        {
            var orderLinesInOrder = _context.OrderLines
                .Where(x => x.OrderId == id).ToList();

            if (orderLinesInOrder.Count > 0)
            {
                foreach (var orderLine in orderLinesInOrder)
                {
                    orderLine.PrintCompletedQuantity = orderLine.Quantity * orderLine.PrintQuantity;

                    _context.Update(orderLine);
                    _context.SaveChanges();
                }
            }
            return Json("Success");
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable2([FromBody]DTParameters dtParameters)
        {
            List<OrderViewModel> orderViewModel = new List<OrderViewModel>();
            var orders = await _context.Orders.Include("Customer").Include("ContactType").ToListAsync();

            foreach (var order in orders)
            {
                var lineSums = (from o in _context.OrderLines
                                where o.OrderId == order.Id
                                select new
                                {
                                    o.Quantity,
                                    PrintTotal = (o.PrintRequired ? 1 : 0) * o.Quantity * o.PrintQuantity,
                                    EmbrTotal = (o.EmbroideryRequired ? 1 : 0) * o.Quantity * o.EmbroideryQuantity,
                                    o.PrintCompletedQuantity,
                                    o.EmbroideryCompletedQuantity
                                }).ToList();
                var orderView = _mapper.Map<OrderViewModel>(order);

                orderView.Quantity = lineSums.Select(x => x.Quantity).Sum();
                orderView.PrintQuantityTotalByOrder = lineSums.Select(x => x.PrintTotal).Sum();
                orderView.EmbroideryQuantityTotalByOrder = lineSums.Select(x => x.EmbrTotal).Sum();
                orderView.PrintQuantityCompletedTotalByOrder = lineSums.Select(x => x.PrintCompletedQuantity).Sum().Value;
                orderView.EmbroideryQuantityCompletedTotalByOrder = lineSums.Select(x => x.EmbroideryCompletedQuantity).Sum().Value;

                if ((orderView.PrintQuantityTotalByOrder - orderView.PrintQuantityCompletedTotalByOrder) == 0 &&
                    (orderView.EmbroideryQuantityTotalByOrder - orderView.EmbroideryQuantityCompletedTotalByOrder) == 0)
                {
                    orderView.OrderStatus = "Completed";
                }
                else
                {
                    orderView.OrderStatus = "Open";
                }

                orderViewModel.Add(orderView);
            }
            //return View(orderViewModel);

            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Name";
                orderAscendingDirection = true;
            }

            var result = orderViewModel;

            //TO DO; Do a generic filtering for each column
            var xxx = "";
            for (int i = 0; i < dtParameters.Columns.Length; i++)
            {
                if (dtParameters.Columns[i].Data == "orderStatus")
                {
                    xxx = dtParameters.Columns[i].Search.Value;
                    if (xxx != "")
                    {
                        xxx = xxx.Substring(0, (xxx.Length - 1));
                        xxx = xxx.Substring(1, (xxx.Length - 1));
                        result = result.Where(r => r.OrderStatus == xxx).ToList();
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Id.ToString().Contains(searchBy) || 
                                           r.CustomerName != null && r.CustomerName.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.OrderDate != null && r.OrderDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.DueDate != null && r.DueDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.InvoiceNumber != null && r.InvoiceNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.InvoiceDate != null && r.InvoiceDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.OrderStatus != null && r.OrderStatus.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            //Sorting  
            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.Orders.CountAsync();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                      .Skip(dtParameters.Start)
                      .Take(dtParameters.Length)
                      .ToList()
            });
        }

        //public IActionResult PrintToPDF2(int? id)
        //{
        //    var order = _context.Orders.Include(x => x.Customer).SingleOrDefault(i => i.Id == id.Value);
        //    var orderViewModel = _mapper.Map<OrderViewModel>(order);
        //    orderViewModel.orderLineViewModels = GetOrderLinesForOrderId(order.Id);
        //    return View(orderViewModel);
        //}

        public IActionResult TestOrders()
        {
            var model = _context.SQLViewOrders.ToList();
            return View(model);
        }

        public async Task<IActionResult> LoadTable([FromBody] DTParameters dtParameters)
        {
            var orderViewModel = _context.SQLViewOrders.ToList();           

            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Name";
                orderAscendingDirection = true;
            }

            var result = orderViewModel;

            //TO DO; Do a generic filtering for each column
            var xxx = "";
            for (int i = 0; i < dtParameters.Columns.Length; i++)
            {
                if (dtParameters.Columns[i].Data == "orderStatus")
                {
                    xxx = dtParameters.Columns[i].Search.Value;
                    if (xxx != "")
                    {
                        xxx = xxx.Substring(0, (xxx.Length - 1));
                        xxx = xxx.Substring(1, (xxx.Length - 1));
                        result = result.Where(r => r.OrderStatus == xxx).ToList();
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.OrderNo.ToString().Contains(searchBy) ||
                                           r.CustomerName != null && r.CustomerName.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.OrderDate != null && r.OrderDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.DueDate != null && r.DueDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.InvoiceNumber != null && r.InvoiceNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.InvoiceDate != null && r.InvoiceDate.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.OrderStatus != null && r.OrderStatus.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }

            //Sorting  
            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc).ToList() : result.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc).ToList();

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.Orders.CountAsync();

            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                      .Skip(dtParameters.Start)
                      .Take(dtParameters.Length)
                      .ToList()
            });
        }

    }
}