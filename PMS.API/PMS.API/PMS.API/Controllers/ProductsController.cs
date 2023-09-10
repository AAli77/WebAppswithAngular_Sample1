using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS.API.Data;
using PMS.API.Models;

namespace PMS.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProductsController : Controller
    {

        private readonly PMSDbContext _pmsDbContext;

        public ProductsController(PMSDbContext pmsDbContext)
        { 
            this._pmsDbContext = pmsDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _pmsDbContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        { 
            product.Id = Guid.NewGuid();
            _pmsDbContext.Products.AddAsync(product);
            _pmsDbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _pmsDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) 
                return NotFound();
            return Ok(product);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, Product updateProductRq)
        {
            var product = await _pmsDbContext.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.Name = updateProductRq.Name;
            product.Type = updateProductRq.Type;
            product.Color = updateProductRq.Color;
            product.Price = updateProductRq.Price;

            await _pmsDbContext.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _pmsDbContext.Products.FindAsync(id);

            if(product == null)
                return NotFound();

            _pmsDbContext.Products.Remove(product);
            await _pmsDbContext.SaveChangesAsync();

            return Ok(product);
        }
    }
}
