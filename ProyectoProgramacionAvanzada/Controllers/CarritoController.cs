public class CarritoController : Controller
{
    private readonly ApplicationDbContext _db = new ApplicationDbContext();

    public ActionResult Index()
    {
        int userId = 1; // luego se cambia por Identity
        var items = _db.CartItems
                       .Include("Product")
                       .Where(x => x.UserId == userId)
                       .ToList();

        return View(items);
    }

    [HttpPost]
    public ActionResult Agregar(int productId, int quantity)
    {
        int userId = 1;

        var item = _db.CartItems
                      .FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);

        if (item == null)
        {
            item = new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                UserId = userId
            };
            _db.CartItems.Add(item);
        }
        else
        {
            item.Quantity += quantity;
        }

        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult Eliminar(int id)
    {
        var item = _db.CartItems.Find(id);
        if (item != null)
        {
            _db.CartItems.Remove(item);
            _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public ActionResult Checkout()
    {
        int userId = 1;
        var cart = _db.CartItems.Include("Product").Where(x => x.UserId == userId).ToList();

        if (!cart.Any())
            return RedirectToAction("Index");

        // Validar inventario
        foreach (var item in cart)
        {
            if (item.Quantity > item.Product.Stock)
            {
                TempData["error"] = $"No hay inventario suficiente para {item.Product.Name}";
                return RedirectToAction("Index");
            }
        }

        // Crear orden
        var order = new Order
        {
            UserId = userId,
            CreatedAt = DateTime.Now,
            Total = cart.Sum(x => x.Quantity * x.Product.Price),
            Details = cart.Select(x => new OrderDetail
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.Product.Price
            }).ToList()
        };

        _db.Orders.Add(order);

        // Actualizar inventario
        foreach (var item in cart)
        {
            item.Product.Stock -= item.Quantity;
        }

        // Vaciar carrito
        _db.CartItems.RemoveRange(cart);

        _db.SaveChanges();

        TempData["ok"] = "Compra completada";
        return RedirectToAction("Index");
    }
}
