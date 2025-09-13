# MyShop - Modern .NET 9 Clean Architecture Project

یک پروژه فروشگاهی مدرن با معماری Clean Architecture و CQRS که با .NET 9 ساخته شده است.

## 🏗️ معماری پروژه

پروژه از **Clean Architecture** و **Domain-Driven Design (DDD)** استفاده می‌کند:

```
MyShop/
├── MyShop.Domain/           # Domain Layer - منطق تجاری اصلی
├── MyShop.Contracts/        # Contracts Layer - اینترفیس‌ها و DTOs
├── MyShop.Application/      # Application Layer - Commands, Queries, Handlers
├── MyShop.API/             # API Layer - Controllers و Middleware
├── MyShop.Client.MVC/      # Presentation Layer - رابط کاربری MVC
├── MyShop.Persistence/      # Infrastructure Layer - دسترسی به داده
├── MyShop.Identity/        # Infrastructure Layer - مدیریت هویت
├── MyShop.ExternalServices/ # Infrastructure Layer - سرویس‌های خارجی
└── MyShop.xUnitTest.Domain/ # Test Layer - تست‌های واحد
```

## 🚀 ویژگی‌های کلیدی

### 1. **Result Pattern**
- کلاس‌های `Result` و `Result<T>` برای مدیریت موفقیت/شکست عملیات
- پشتیبانی از Functional Programming با متدهای `Map`, `Bind`, `Match`
- تبدیل خودکار بین `T` و `Result<T>`

### 2. **ApiResponse**
- پاسخ‌های استاندارد API با فرمت یکسان
- پشتیبانی از TraceId برای ردیابی درخواست‌ها
- مدیریت خطاها و Metadata

### 3. **Pagination**
- `PaginationParams` برای پارامترهای صفحه‌بندی
- `PagedResult<T>` با Metadata کامل
- Extension Methods برای `IQueryable`

### 4. **Sorting**
- `SortDto` برای مرتب‌سازی داینامیک
- پشتیبانی از چندین فیلد مرتب‌سازی
- استفاده از `System.Linq.Dynamic.Core`

### 5. **Filtering**
- `FilterDto` با اپراتورهای مختلف (Equals, Contains, GreaterThan, etc.)
- فیلترینگ داینامیک روی `IQueryable`
- پشتیبانی از فیلترهای پیچیده

### 6. **Exception Middleware**
- مدیریت مرکزی خطاها
- تبدیل خودکار Exception ها به ApiResponse
- پشتیبانی از انواع مختلف خطا

### 7. **CQRS با MediatR**
- جداسازی Commands و Queries
- Handler های مجزا برای هر عملیات
- پشتیبانی از Result Pattern

## 🛠️ تکنولوژی‌های استفاده شده

- **.NET 9** - آخرین نسخه .NET
- **MediatR** - پیاده‌سازی CQRS
- **Entity Framework Core** - ORM
- **System.Linq.Dynamic.Core** - کوئری‌های داینامیک
- **xUnit** - تست‌های واحد
- **FluentAssertions** - Assertion های خوانا

## 📋 Domain Model

### Customer Aggregate
- **Entity**: `Customer` با منطق تجاری پیچیده
- **Value Objects**: `Email`, `PhoneNumber`, `Address`
- **Business Rules**: قوانین تجاری قابل تست
- **Domain Events**: رویدادهای دامنه برای تغییرات مهم

### Value Objects
- **Email**: تشخیص نوع ایمیل، اعتبارسنجی، تبدیل فرمت
- **PhoneNumber**: پشتیبانی از فرمت‌های ایرانی، تشخیص موبایل/ثابت
- **Address**: آدرس کامل ایرانی، مقایسه و به‌روزرسانی

## 🔧 نحوه اجرا

### پیش‌نیازها
- .NET 9 SDK
- Visual Studio 2022 یا VS Code

### اجرای پروژه
```bash
# کلون کردن پروژه
git clone <repository-url>
cd MyShop

# اجرای API
cd MyShop.API
dotnet run

# اجرای MVC Client
cd MyShop.Client.MVC
dotnet run
```

### تست API
پروژه شامل فایل `Customers.http` برای تست API است:

```http
### Get all customers
GET https://localhost:7000/api/customers?PageNumber=1&PageSize=10

### Create customer
POST https://localhost:7000/api/customers
Content-Type: application/json

{
  "firstName": "احمد",
  "lastName": "محمدی",
  "email": "ahmad@example.com",
  "mobileNumber": "09151234567",
  "createdBy": "API"
}
```

## 📊 نمونه پاسخ API

### موفقیت
```json
{
  "success": true,
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "firstName": "احمد",
    "lastName": "محمدی",
    "fullName": "احمد محمدی",
    "email": "ahmad@example.com",
    "status": "Active"
  },
  "errors": [],
  "meta": {
    "createdAt": "2025-01-27T10:30:00Z",
    "operation": "CreateCustomer",
    "version": "1.0"
  },
  "traceId": "0HMQ8VQKJQJQJ"
}
```

### خطا
```json
{
  "success": false,
  "data": null,
  "errors": ["Email 'ahmad@example.com' is already registered."],
  "meta": null,
  "traceId": "0HMQ8VQKJQJQJ"
}
```

## 🧪 تست‌ها

پروژه شامل تست‌های جامع برای Domain Layer است:

```bash
# اجرای تست‌ها
cd MyShop.xUnitTest.Domain
dotnet test
```

### پوشش تست‌ها
- **CustomerTests**: 720 خط تست برای Customer Aggregate
- **BusinessRulesTests**: 372 خط تست برای قوانین تجاری
- **ValueObjectsTests**: تست‌های کامل برای Email, PhoneNumber, Address

## 🔄 CQRS Commands & Queries

### Commands
- `CreateCustomerCommand` - ایجاد مشتری جدید
- `UpdateCustomerCommand` - به‌روزرسانی مشتری
- `DeleteCustomerCommand` - حذف مشتری

### Queries
- `GetCustomersQuery` - لیست مشتریان با Pagination/Sorting/Filtering
- `GetCustomerByIdQuery` - دریافت مشتری بر اساس ID

## 🎯 ویژگی‌های آینده

- [ ] پیاده‌سازی Entity Framework Core
- [ ] اضافه کردن Authentication & Authorization
- [ ] پیاده‌سازی Caching (Redis)
- [ ] اضافه کردن Logging (Serilog)
- [ ] پیاده‌سازی Health Checks
- [ ] اضافه کردن Validation (FluentValidation)
- [ ] پیاده‌سازی Background Jobs
- [ ] اضافه کردن API Documentation (Swagger)

## 📝 نکات مهم

1. **Result Pattern**: تمام عملیات از Result Pattern استفاده می‌کنند
2. **Exception Handling**: خطاها به صورت مرکزی مدیریت می‌شوند
3. **Domain Events**: تغییرات مهم در Domain Events ثبت می‌شوند
4. **Business Rules**: قوانین تجاری قابل تست و قابل استفاده مجدد هستند
5. **Clean Architecture**: جداسازی مناسب لایه‌ها و وابستگی‌ها
6. **Performance Optimization**: استفاده از `Array.Empty<string>()` به جای `new List<string>()`
7. **Rich Metadata**: پشتیبانی کامل از Meta در تمام پاسخ‌های API
8. **Traceability**: ردیابی کامل درخواست‌ها با TraceId

## 🚀 بهبودهای جدید

### ApiResponse بهینه‌سازی شده
- **Errors**: استفاده از `Array.Empty<string>()` برای بهینه‌سازی حافظه
- **Meta**: پشتیبانی کامل از Metadata در تمام متدهای `FromResult`
- **TraceId**: ردیابی کامل درخواست‌ها

### نمونه استفاده از Meta
```csharp
// در Controller
var meta = new { 
    CreatedAt = DateTime.UtcNow,
    Operation = "CreateCustomer",
    Version = "1.0"
};
return Ok(result, meta);

// در BaseController
protected ActionResult<ApiResponse<T>> Ok<T>(Result<T> result, object? meta = null)
{
    return base.Ok(ApiResponse<T>.FromResult(result, meta, TraceId));
}
```

### فیلترینگ پیشرفته با Type Safety
```csharp
// فیلتر با نوع داده صحیح
var filter = new FilterDto 
{ 
    Field = "Age", 
    Operator = FilterOperator.GreaterThan, 
    Value = "25", 
    ValueType = "int" 
};

// فیلتر تاریخ
var dateFilter = new FilterDto 
{ 
    Field = "CreatedAt", 
    Operator = FilterOperator.GreaterThanOrEqual, 
    Value = "2024-01-01T00:00:00Z", 
    ValueType = "datetime" 
};

// استفاده در Query
var customers = await _customerRepository.GetQueryableAsync()
    .ApplyFiltering(filter)
    .ApplyFiltering(dateFilter)
    .ToPagedResultAsync(paginationParams);
```

### نمونه‌های API با فیلترینگ پیشرفته
```http
# فیلتر سن (int)
GET /api/customers?Filtering[0].Field=Age&Filtering[0].Operator=GreaterThan&Filtering[0].Value=25&Filtering[0].ValueType=int

# فیلتر تاریخ (datetime)
GET /api/customers?Filtering[0].Field=CreatedAt&Filtering[0].Operator=GreaterThanOrEqual&Filtering[0].Value=2024-01-01T00:00:00Z&Filtering[0].ValueType=datetime

# فیلتر چندگانه
GET /api/customers?Filtering[0].Field=Status&Filtering[0].Operator=Equals&Filtering[0].Value=Active&Filtering[1].Field=Age&Filtering[1].Operator=GreaterThan&Filtering[1].Value=20&Filtering[1].ValueType=int
```

## 🤝 مشارکت

برای مشارکت در پروژه:
1. Fork کنید
2. Branch جدید بسازید
3. تغییرات را Commit کنید
4. Pull Request ارسال کنید

## 📄 مجوز

این پروژه تحت مجوز MIT منتشر شده است.