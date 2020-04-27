using AppMvcBasica.Data;
using AppMvcBasica.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvcBasica.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [AllowAnonymous]
        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = this._context.Produtos.Include(p => p.Fornecedor);
            return this.View(await applicationDbContext.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var produto = await this._context.Produtos
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return this.NotFound();
            }

            return this.View(produto);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            this.ViewData["FornecedorId"] = new SelectList(this._context.Fornecedores, "Id", "Nome");
            return this.View();
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            if (this.ModelState.IsValid)
            {
                this._context.Add(produto);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(Index));
            }
            this.ViewData["FornecedorId"] = new SelectList(this._context.Fornecedores, "Id", "Nome", produto.FornecedorId);
            return this.View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var produto = await this._context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return this.NotFound();
            }
            this.ViewData["FornecedorId"] = new SelectList(this._context.Fornecedores, "Id", "Nome", produto.FornecedorId);
            return this.View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Produto produto)
        {
            if (id != produto.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this._context.Update(produto);
                    await this._context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.ProdutoExists(produto.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return this.RedirectToAction(nameof(Index));
            }
            this.ViewData["FornecedorId"] = new SelectList(this._context.Fornecedores, "Id", "Nome", produto.FornecedorId);
            return this.View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var produto = await this._context.Produtos
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return this.NotFound();
            }

            return this.View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await this._context.Produtos.FindAsync(id);
            this._context.Produtos.Remove(produto);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(Guid id)
        {
            return this._context.Produtos.Any(e => e.Id == id);
        }
    }
}
