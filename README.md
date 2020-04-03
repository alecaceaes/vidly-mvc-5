# Vidly
An ASP.NET MVC demo project from [The Complete ASP.NET MVC 5 Course](https://www.udemy.com/course/the-complete-aspnet-mvc-5-course/) by Mosh Hamedani
<br>
<br>  

Libraries/Extensions used:
* [Entity Framework 6](https://docs.microsoft.com/en-us/ef/ef6/get-started)  
* [AutoMapper](https://automapper.org/) *ver(4.1) was used on the demo, I used 6.0*
* [jQuery](https://jquery.com/)
* [Bootbox](http://bootboxjs.com/)
* [DataTables](https://datatables.net/)
* [Glimpse](https://github.com/glimpse/glimpse)
* [Typehead.js](https://twitter.github.io/typeahead.js/)
* [Toastr](https://github.com/CodeSeven/toastr)
* [Elmah](https://elmah.github.io/a/mvc/)

## ASP.NET MVC Fundamentals
### Action Results
| Type                  | Helper Method          | 
| --------------------- | ---------------------- |
| ViewResult            | **View()**             |
| PartialViewResult     | **PartialView()**      |
| ContentResult         | **Content()**          |
| RedirectResult        | **Redirect()**         |
| RedirectToRouteResult | **RedirectToAction()** |
| JsonResult            | **Json()**             |
| FileResult            | **File()**             |
| HttpNotFoundResult    | **HttpNotFound()**     |
| EmptyResult           |                        |

### Action Parameters Sources
* Embedded in the URL: /movies/edit/1
* In the query string: /movies/edit?id=1
* In the form data

### Convention-based Routes
```C#
routes.MapRoute(
  "MoviesByReleaseDate",
  "movies/released/{year}/{month}",
  new {
    controller = "Movies",
    action = "MoviesReleaseByDate"
  },
  new {
    year = @"\d{4}",
    month = @"\d{2}"
  }
  isFavorite = false;
}
```

### Attribute Routes
```C#
[Route("movies/released/{year}/{month}")
public ActionResult MoviesByReleaseDate(int year, int month)
{
}
```

To apply a constraint use a colon:
```C#
month:regex(\\d{2}):range(1, 12)
```

### Passing Data to Views
Avoid using ViewData and ViewBag because they are fragile. Plus, you have to do extra
casting, which makes your code ugly. Pass a model (or a view model) directly to a view:
```C#
return View(movie);
```

### Razor Views
```Razor
@if (…)
{
  // C# code or HTML
}

@foreach (…)
{
}
```
Render a class (or any attributes) conditionally:
```Razor
@{
  var className = Model.Customers.Count > 5 ? "popular" : null;
}
<h2 class="@className">…</h2>
```

## Entity Framework
### Code-first Migration
```npm
enable-migrations
add-migration <name>
add-migration <name> -force (to overwrite the last migration)
update-database
```
### Seeding the Database
Create a new empty migration and use the Sql method:
```C#
Sql("INSERT INTO …")
```

### Overriding Conventions
```C#
[Required]
[StringLength(255)]
public string Name { get; set; }
```
### Querying Objects
```C#
public class MoviesController
{
  private ApplicationDbContext _context;

  public MoviesController()
  {
    _context = new ApplicationDbContext();
  }
  
  protected override Dispose()
  {
    _context.Dispose();
  }
  
  public ActionResult Index()
  {
    var movies = _context.Movies.ToList();
    …
  }
}
```
### LINQ Extension Methods
```C#
_context.Movies.Where(m => m.GenreId == 1)
_context.Movies.Single(m => m.Id == 1);
_context.Movies.SingleOrDefault(m => m.Id == 1);
_context.Movies.ToList();
```
### Eager Loading
```
_context.Movies.Include(m => m.Genre);
```
## Building Forms
### View
```Razor
@using (Html.BeginForm("action", "controller"))
{
  <div class="form-group">
  @Html.LabelFor(m => m.Name)
  @Html.TextBoxFor(m => m.Name, new { @class = "form-control")
  </div>
  <button type="submit" class="btn btn-primary">Save</button>
}
```
### Markup for Checkbox Fields
```#
<div class="checkbox">
  @Html.CheckBoxFor(m => m.IsSubscribed) Subscribed?
</div>
```
### Drop-down Lists
```C#
@Html.DropDownListFor(m => m.TypeId, new SelectList(Model.Types, "Id", "Name"), "", new { @class = "form‐control" }
```
### Overriding Labels
```C#
Display(Name = "Date of Birth")
public DateTime? Birthdate { get; set;}
```
### Saving Data
```C#
[HttpPost]
public ActionResult Save(Customer customer)
{
  if (customer.Id == 0)
    _context.Customers.Add(customer);
  else
  {
    var customerInDb = _context.Customers.Single(c.Id == customer.Id);
    //…  update properties
  }
  _context.SaveChanges();
  return RedirectToAction("Index", "Customers")
}
```
### Hidden Fields
Required when updating data.
```C#
@Html.HiddenFor(m => m.Customer.Id)
```

## Implementing Validation
### Adding Validation
Decorate properties of your model with data annotations. Then, in the controller:
```C#
if (!ModelState.IsValid)
  return View(…);
```
And in the view:
```Razor
@Html.ValidationMessageFor(m => m.Name)
```
Styling Validation Errors
In site.css:
```css
.input-validation‐error {
  color:  red;
}
.field‐validation‐error {
  border:2px solid red;
}
```
### Data Annotations
* [Required]
* [StringLength(255)]
* [Range(1, 10)]
* [Compare("OtherProperty")]
* [Phone]
* [EmailAddress]
* [Url]
* [RegularExpression("…")]

### Custom Validation
```C#
public class Min18IfAMember : ValidationAttribute
{
  protected override ValidationResult IsValid(object value, ValidationContext validationContext)
  {
    …
    if (valid) 
      return ValidationResult.Success;
    else
      return new ValidationResult("error message");
   }
}
```
### Validation Summary
```Razor
@Html.ValidationSummary(true, "Please fix the following errors");
```
### Client-side Validation
```Razor
@section scripts
{
  @Scripts.Render(“~/bundles/jqueryval”)
}
```
### Anti-forgery Tokens
In the view:
```Razor
@Html.AntiForgeryToken()
```
In the controller:
```C#
[ValidateAntiForgeryToken]
public ActionResult Save()
{
}
```
## Building Web APIs
### RESTful Convention
| Request | Description                                                                        |
| ----------- | ------------------------------------------------------------------------------ |
| **GET**     | /api/customers Get all customers                                               |
| **GET**     | /api/customers/1 Get customer with ID 1                                        |
| **POST**    | /api/customers Add a new customer (customer data in the request body)          |
| **PUT**     | /api/customers/1 Update customer with ID 1 (customer data in the request body) |
| **DELETE**  | /api/customers/1 Delete customer with ID 1                                     |

### Building an API
```C#
public IHttpActionResult GetCustomers()
{}

[HttpPost]
public IHttpActionResult CreateCustomer(CustomerDto customer)
{}

[HttpPut]
public IHttpActionResult UpdateCustomer(int id, CustomerDto customer)
{}

[HttpDelete]
public IHttpActionResult DeleteCustomer(int id)
{}
```
### Helper methods
* NotFound()
* Ok()
* Created()
* Unauthorized()
### AutoMapper
Create a mapping profile first:
```C#
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    Mapper.CreateMap<Customer,   CustomerDto>();
  }
}
```
Load the mapping profile during application startup (in global.asax.cs):
```C#
protected void Application_Start()
{
  Mapper.Initialize(c => c.AddProfile<MappingProfile>());
}
```
To map objects:
```C#
var customerDto = Mapper.Map<Customer, CustomerDto>(customer);
```
Or to map to an existing object:
```C#
Mapper.Map(customer, customerDto);
```
### Enabling camel casing
In WebApiConfig:
```C#
public static void Register(HttpConfiguration config)
{
  var settings = config.Formatters.JsonFormatter.SerializerSettings;
  settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
  settings.Formatting = Formatting.Indented;
}
```
## Client-side Development
### Calling an API Using jQuery
```javascript
$.ajax({
  url: "…",
  method: "",  // DELETE, POST, PUT, optional for GET
  success: function(result) {
    …
  }
});
```
### Bootbox
```javascript
bootbox.confirm("Are you sure?", function(result){
  if (result) {
  }
});
```
### DataTables - Zero Configuration
```javascript
$(“#customers”).DataTable();
```
### DataTables - Ajax Source
```javascript
$("#customers").DataTable({
  ajax: {
    url: "…",
    dataSrc: "",
  },
  columns: [
    { data: "name" },
    { data: "id", render: function(data, type, row) {
      return "…";
    }
  }]
});
```
### DataTables - Removing Records
```javascript
var table = $("…").DataTable(…);
var tr = $("…"); 
table.rows(tr).remove().draw();
```

## Authentication/Authorization
### ASP.NET Identity Classes
* **API**: UserManager, RoleManager, SignInManager
* **Domain**: IdentityUser, IdentityRole
* **Persistence**: UserStore, RoleStore
### Restricting Access
#### Declaratively
**[Authorize]:** apply to an action, a controller or globally (in FilterConfig).
```C#
[Authorize(Roles = "CanManageMovies")]
```
#### Programatically
In an action:
```C#
if (User.Identity.IsAuthenticated)
{
…
}
if
(User.IsInRole("CanManageMovies")
{
…
}
```
### Seeding Users and Roles
Populate your database with the default user(s) and role(s).
Create an empty migration.
Script the data in existing users and roles tables and add them to the migration.
Remove the records from your database.
Run the migration.
### Assigning a User to a Role
```C#
var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
var roleManager = new RoleManager<IdentityRole>(roleStore);
await roleManager.CreateAsync(new IdentityRole("CanManageMovies"));
await UserManager.AddToRoleAsync(user.Id, "CanManageMovies");
```

### Adding Profile Data
Always start with the domain. Add the new properties to **ApplicationUser**.

Create a migration and update the database.

Modify the views: **Register.cshtml** and **ExternalLoginConfirmation.cshtml**.

When the registration form is posted, set the properties of the **ApplicationUser** object
using view model properties. In **AccountController**, you need to modify two actions:
**Register** and **ExternalLoginConfirmation**.

### Enabling Social Logins
Enable SSL: select the project, F4, set **SSL Enabled** to true. Copy **SSL Url**, select the
project, go to **Properties**, in the **Web** tab, set the **Startup URL**.

Apply **RequireSsl** filter globally (in FilterConfig).

Register your app with an external authentication provider to get a key/secret. In
**App_Start/Startup.Auth.cs**, remove the comment for the corresponding providers and
add your key/secret.

## Performance Optimization
### Rules of thumb
* Do not sacrifice the maintainability of your code to premature optimization.
* Be realistic and think like an "engineer".
* Be pragmatic and ensure your efforts have observable results and give value.<br>
And remember: **premature optimization is the root of all evils.**
### Database tier
#### Schema
* Every table must have a primary key.
* Tables should have relationships.
* Put indexes on columns where you filter records on. But remember: too many
indexes can have an adverse impact on the performance.
* Avoid Entity-Attribute-Value (EAV) pattern.
#### Queries
* Keep an eye on EF queries using Glimpse. If a query is slow, use a stored
procedure.
* Use Execution Plan in SQL Server to find performance bottlenecks in your
queries.
If after all your optimizations, you still have slow queries, consider creating a
denormalised “read” database for your queries. But remember, this comes with the cost
of maintaining two databases in sync. A simpler approach is to use caching.

### Application tier
* On pages where you have costly queries on data that doesn’t change frequently,
use **OutputCache** to cache the rendered HTML.
* You can also store the results of the query in cache (using **MemoryCache**), but
use this approach only in actions that are used for displaying data, not modifying
it.
* Async does not improve performance. It improves scalability given that you’re
not using a single instance of SQL Server on the back-end. You should be using
a SQL cluster, or a NoSQL database (eg MongoDB, RavenDB, etc) or SQL
Azure.
* Disable session in **web.config**.
* Use release builds in production.

### Client Tier
* Put JS and CSS files in bundles.
* Put the script bundles near the end of the <body> element. Modernizr is an
exception. It needs to be in the head.
* Return small, lightweight DTOs from your APIs.
* Render HTML markup on the client. That’s the case with single page
applications (SPA).
* Compress images.
* Use image sprites. This is beyond the scope of the course and you need to read
about them yourself.
* Reduce the data you store in cookies because they’re sent back and forth with
every request.
* Use content delivery networks (CDN). Again, beyond the scope of the course.
Implementations vary depending on where you host your application.

## Deployment
### Deploying the Application
Solution Explorer > Publish > Custom
  * Enter Profile Name
  * Press OK
  * Connection Tab > Publish Method = File System
    * Target Location
    * Settings > Configuration = Release > Publish
### Deploying the Database
```
update-database -script (firsttime)
update-database -script -SourceMigration:<nameOfMigrationClass>
```
### Securing Configuration Settings
Open *Developer Command Prompt for VS*
```
aspnet _regiis -pef "appSettings" "C:\Deploy" -prov "DataProtectionConfigurationProvider"
aspnet _regiis -pdf "appSettings" "C:\Deploy"
```
