public class Book {
  public int Id { get; set; }
  public required string Title { get; set; }
  public required string Author { get; set; }

  public int Pages { get; set; }

  public DateTime Published { get; set; }
}

public static class BookAPI {
  private static readonly List<Book> _books = [
    new() { Id = 1, Title = "Paradise Lost", Author = "John Milton", Pages = 400, Published = new DateTime(1667, 1, 1) },
    new() { Id = 2, Title = "The Odyssey", Author = "Homer", Pages = 300, Published = new DateTime(800, 1, 1) },
    new() { Id = 3, Title = "The Divine Comedy", Author = "Dante Alighieri", Pages = 500, Published = new DateTime(1320, 1, 1) },
    new() { Id = 4, Title = "Tractatus Logico-Philosophicus", Author = "Ludwig Wittgenstein", Pages = 150, Published = new DateTime(1921, 1, 1) },
    new() { Id = 5, Title = "The Republic", Author = "Plato", Pages = 350, Published = new DateTime(380, 1, 1) },
  ];

  public static void AddRoutes(this WebApplication app) {
    _ = app.MapGet("/api/books", () => {
      return Results.Ok(_books);
    });

    _ = app.MapGet("/api/books/{id}", (int id) => {
      var book = _books.FirstOrDefault(b => b.Id == id);
      return book == null ? Results.NotFound() : Results.Ok(book);
    });

    _ = app.MapPost("/api/books", (Book book) => {
      book.Id = _books.Max(b => b.Id) + 1;
      _books.Add(book);
      return Results.Created($"/api/books/{book.Id}", book);
    });
  }
}
