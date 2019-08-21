using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintCompany.Core;
using PrintCompany.Data;
using PrintCompany.ViewModels;

namespace PrintCompany.Controllers
{
    public class OrderCommunicationController : Controller
    {
        private readonly PrintCompanyDbContext _context;
        private readonly IMapper _mapper;

        public OrderCommunicationController(PrintCompanyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }       

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> Create(OrderCustomerContactViewModel orderCustomerContactViewModel)
        {
            if (ModelState.IsValid)
            {
                var orderCustomerContact = _mapper.Map<OrderCustomerContact>(orderCustomerContactViewModel);

                _context.OrderCustomerContacts.Add(orderCustomerContact);
                await _context.SaveChangesAsync();
                return Json("Success");
            }
            return View(orderCustomerContactViewModel);
        }

        public PartialViewResult Edit(int id)
        {
            var orderCommunication = _context.OrderCustomerContacts.
                Include("ContactType").
                SingleOrDefault(x => x.Id == id);

            var orderCommunicationViewModel = _mapper.Map<OrderCustomerContactViewModel>(orderCommunication);

            return PartialView("_PartialOrderCommunication-Edit", orderCommunicationViewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, OrderCustomerContactViewModel orderCustomerContactViewModel)
        {
            if (id != orderCustomerContactViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var orderCommunication = _context.OrderCustomerContacts.SingleOrDefault(e => e.Id == id);

                _mapper.Map(orderCustomerContactViewModel, orderCommunication);

                try
                {
                    _context.Update(orderCommunication);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderCommunicationExists(orderCustomerContactViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json("Success");
            }
            return View(orderCustomerContactViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var file = await _context.OrderCustomerContacts.FindAsync(id);
            if (file == null)
                return NotFound();
            _context.OrderCustomerContacts.Remove(file);
            await _context.SaveChangesAsync();
            return Json("Success");
        }

        public JsonResult GetContactTypeList(string searchTerm)
        {

            var contactTypeList = _context.ContactTypes.OrderBy(m => m.ContactTypeName).ToList();

            if (searchTerm != null)
            {
                contactTypeList = _context.ContactTypes.Where(c => c.ContactTypeName.Contains(searchTerm)).OrderBy(m => m.ContactTypeName).ToList();
            }

            var modifiedData = contactTypeList.Select(x => new
            {
                id = x.Id,
                text = x.ContactTypeName
            });

            return Json(modifiedData);
        }

        private bool OrderCommunicationExists(int id)
        {
            return _context.OrderCustomerContacts.Any(e => e.Id == id);
        }
    }
}