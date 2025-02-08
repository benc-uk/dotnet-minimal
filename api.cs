public class Book {
  public int Id { get; set; }
  public required string Title { get; set; }
  public required string Author { get; set; }

  public int Pages { get; set; }

  public DateTime Published { get; set; }
}

public static class BookAPI {
  private static List<Book> _books = new List<Book>
  {
    new Book { Id = 1, Title = "Paradise Lost", Author = "John Milton", Pages = 400, Published = new DateTime(1667, 1, 1) },
    new Book { Id = 2, Title = "The Odyssey", Author = "Homer", Pages = 300, Published = new DateTime(800, 1, 1) },
    new Book { Id = 3, Title = "The Divine Comedy", Author = "Dante Alighieri", Pages = 500, Published = new DateTime(1320, 1, 1) },
    new Book { Id = 4, Title = "Tractatus Logico-Philosophicus", Author = "Ludwig Wittgenstein", Pages = 150, Published = new DateTime(1921, 1, 1) },
    new Book { Id = 5, Title = "The Republic", Author = "Plato", Pages = 350, Published = new DateTime(380, 1, 1) },
  };

  public static void AddRoutes(this WebApplication app) {
    app.MapGet("/api/books", () => {
      return Results.Ok(_books);
    });

    app.MapGet("/api/books/{id}", (int id) => {
      var book = _books.FirstOrDefault(b => b.Id == id);
      if (book == null) {
        return Results.NotFound();
      }

      return Results.Ok(book);
    });

    app.MapPost("/api/books", (Book book) => {
      book.Id = _books.Max(b => b.Id) + 1;
      _books.Add(book);
      return Results.Created($"/api/books/{book.Id}", book);
    });
  }
}
