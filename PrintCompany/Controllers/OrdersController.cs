using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrintCompany.Core;
using PrintCompany.Data;
using PrintCompany.ViewModels;

namespace PrintCompany.Controllers
{
    public class OrdersController : Controller
    {
        private readonly PrintCompanyDbContext _context;
        public IHostingEnvironment _host { get; }

        public OrdersController(PrintCompanyDbContext context, IHostingEnvironment host)
        {
            _host = host;
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            List<OrderViewModel> orderViewModel = new List<OrderViewModel>();
            var orders = await _context.Orders.Include("Customer").ToListAsync();

            foreach (var order in orders)
            {
                var lineSums = (from o in _context.OrderLines
                                  where o.OrderId == order.Id
                                  select new
                                  {
                                      PrintTotal = (o.PrintRequired ? 1 : 0) * o.Quantity,
                                      EmbrTotal =  (o.EmbroideryRequired ? 1 : 0) * o.Quantity,
                                      o.PrintCompletedQuantity,
                                      o.EmbroideryCompletedQuantity
                                  }).ToList();
                orderViewModel.Add(
                    new OrderViewModel
                    {
                        Id = order.Id,
                        CustomerId = order.CustomerId,
                        CustomerName = order.Customer.Name,
                        OrderNumber = order.OrderNumber,
                        DueDate = order.DueDate,
                        OrderDate = order.OrderDate,
                        PrintQuantityTotalByOrder = lineSums.Select(x => x.PrintTotal).Sum(),
                        //EmbroideryQuantityTotalByOrder = _context.OrderLines.Where(o => o.OrderId == order.Id).
                        //                              Sum(x => (x.EmbroideryRequired ? 1 : 0) * x.Quantity),
                        EmbroideryQuantityTotalByOrder = lineSums.Select(x => x.EmbrTotal).Sum(),
                        PrintQuantityCompletedTotalByOrder = lineSums.Select(x => x.PrintCompletedQuantity).Sum().Value,
                        //EmbroideryQuantityCompletedTotalByOrder = _context.OrderLines.Where(o => o.OrderId == order.Id).
                        //                                    Sum(x => x.EmbroideryCompletedQuantity).Value
                        EmbroideryQuantityCompletedTotalByOrder = lineSums.Select(x => x.EmbroideryCompletedQuantity).Sum().Value
                    });
            }
            return View(orderViewModel);
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
                Order order = new Order
                {
                    Id = orderViewModel.Id,
                    DueDate = orderViewModel.DueDate,
                    OrderDate = orderViewModel.OrderDate,
                    CustomerId = orderViewModel.CustomerId,
                    OrderNumber = orderViewModel.OrderNumber
                };
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

            OrderViewModel orderViewModel = new OrderViewModel
            {
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
                DueDate = order.DueDate,
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderNumber = order.OrderNumber,
                InvoiceDate = order.InvoiceDate,
                InvoiceNumber = order.InvoiceNumber,
                orderLineViewModels = GetOrderLinesForOrderId(order.Id),
                FileUploads = GetFilesByOrderId(order.Id)
            };

            //var requestTypeAjax = HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";

            //if (requestTypeAjax)
            //{
            //    return PartialView("_PartialFileAttachmentList", GetFilesByOrderId(order.Id));
            //}
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
                order.CustomerId = orderViewModel.CustomerId;
                order.DueDate = orderViewModel.DueDate;
                order.OrderDate = orderViewModel.OrderDate;
                order.OrderNumber = orderViewModel.OrderNumber;
                order.InvoiceDate = orderViewModel.InvoiceDate;
                order.InvoiceNumber = orderViewModel.InvoiceNumber;
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

        // POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        //private SelectList GetItemTypes()
        //{
        //    var types = _context.ItemTypes
        //                .Select(x =>
        //                        new SelectListItem
        //                        {
        //                            Value = x.Id.ToString(),
        //                            Text = x.Type
        //                        });
        //    return new SelectList(types, "Value", "Text");
        //}       

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
                    orderLineViewModels.Add(new OrderLineViewModel
                    {
                        Id = orderLine.Id,
                        EmbroideryRequired = orderLine.EmbroideryRequired,
                        ItemColorId = orderLine.ItemColorId,
                        ItemSizeId = orderLine.ItemSizeId,
                        ItemTypeId = orderLine.ItemTypeId,
                        OrderId = orderLine.OrderId,
                        SupplierId = orderLine.SupplierId,
                        PrintRequired = orderLine.PrintRequired,
                        Quantity = orderLine.Quantity,
                        ItemColor = orderLine.ItemColor,
                        ItemSize = orderLine.ItemSize,
                        ItemType = orderLine.ItemType,
                        Supplier = orderLine.Supplier,
                        SupplierName = orderLine.Supplier.Name ?? "",
                        EmbroideryCompletedQuantity = orderLine.EmbroideryCompletedQuantity ?? 0,
                        PrintCompletedQuantity = orderLine.PrintCompletedQuantity ?? 0

                    });
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


    }
}