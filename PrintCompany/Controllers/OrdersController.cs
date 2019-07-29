using System;
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
using PrintCompany.ViewModels;

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
        public async Task<IActionResult> Index()
        {
            List<OrderViewModel> orderViewModel = new List<OrderViewModel>();
            var orders = await _context.Orders.Include("Customer").Include("ContactType").ToListAsync();

            foreach (var order in orders)
            {
                var lineSums = (from o in _context.OrderLines
                                  where o.OrderId == order.Id
                                  select new
                                  {
                                      PrintTotal = (o.PrintRequired ? 1 : 0) * o.Quantity * o.PrintQuantity,
                                      EmbrTotal =  (o.EmbroideryRequired ? 1 : 0) * o.Quantity * o.EmbroideryQuantity,
                                      o.PrintCompletedQuantity,
                                      o.EmbroideryCompletedQuantity
                                  }).ToList();
                var orderView = _mapper.Map<OrderViewModel>(order);

                orderView.PrintQuantityTotalByOrder = lineSums.Select(x => x.PrintTotal).Sum();
                orderView.EmbroideryQuantityTotalByOrder = lineSums.Select(x => x.EmbrTotal).Sum();
                orderView.PrintQuantityCompletedTotalByOrder = lineSums.Select(x => x.PrintCompletedQuantity).Sum().Value;
                orderView.EmbroideryQuantityCompletedTotalByOrder = lineSums.Select(x => x.EmbroideryCompletedQuantity).Sum().Value;

                if ((orderView.PrintQuantityTotalByOrder - orderView.PrintQuantityCompletedTotalByOrder) == 0 && 
                    (orderView.EmbroideryQuantityTotalByOrder - orderView.EmbroideryQuantityCompletedTotalByOrder) == 0 )
                {
                    orderView.OrderStatus = "Completed";
                }
                else
                {
                    orderView.OrderStatus = "Open";
                }

                orderViewModel.Add(orderView);
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


    }
}