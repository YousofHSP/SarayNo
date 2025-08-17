namespace Entity;

public static class Permissions
{
    public static List<Permission> All =
    [
        
        new() { Controller = "Dashboard", ControllerLabel = "داشبورد",  Action = "Index", ActionLabel = "نمایش" },
        
        new() { Controller = "Role", ControllerLabel = "نقش",  Action = "Index", ActionLabel = "نمایش" },
        new() { Controller = "Role", ControllerLabel = "نقش", Action = "Create",  ActionLabel = "ایجاد" },
        new() { Controller = "Role", ControllerLabel = "نقش", Action = "Edit",  ActionLabel = "ویرایش" },
        new() { Controller = "Role", ControllerLabel = "نقش", Action = "Delete", ActionLabel = "حذف" },
        
        new() { Controller = "User", Action = "Index", ControllerLabel = "کاربر", ActionLabel = "نمایش" },
        new() { Controller = "User", Action = "Create", ControllerLabel = "کاربر", ActionLabel = "ایجاد" },
        new() { Controller = "User", Action = "Edit", ControllerLabel = "کاربر", ActionLabel = "ویرایش" },
        new() { Controller = "User", Action = "Delete", ControllerLabel = "کاربر", ActionLabel = "حذف" },
        
        new() { Controller = "Activity", Action = "Index", ControllerLabel = "فعالیت", ActionLabel = "نمایش" },
        new() { Controller = "Activity", Action = "Create", ControllerLabel = "فعالیت", ActionLabel = "ایجاد" },
        new() { Controller = "Activity", Action = "Edit", ControllerLabel = "فعالیت", ActionLabel = "ویرایش" },
        new() { Controller = "Activity", Action = "Delete", ControllerLabel = "فعالیت", ActionLabel = "حذف" },
        new() { Controller = "Activity", Action = "Details", ControllerLabel = "فعالیت", ActionLabel = "جزئیات" },
        new() { Controller = "Activity", Action = "AddPayoff", ControllerLabel = "فعالیت", ActionLabel = "افزودن علی الحساب" },
        new() { Controller = "Activity", Action = "DeletePayoff", ControllerLabel = "فعالیت", ActionLabel = "حذف علی الحساب" },
        new() { Controller = "Activity", Action = "ApprovedPayoff", ControllerLabel = "فعالیت", ActionLabel = "تایید علی الحساب" },
        new() { Controller = "Activity", Action = "AddImage", ControllerLabel = "فعالیت", ActionLabel = "افزودن تصویر" },
        new() { Controller = "Activity", Action = "RemoveImage", ControllerLabel = "فعالیت", ActionLabel = "حذف تصویر" },
        new() { Controller = "Activity", Action = "SetTotalAmount", ControllerLabel = "فعالیت", ActionLabel = "ثبت مبلغ کل فاکتور" },
        new() { Controller = "Activity", Action = "Done", ControllerLabel = "فعالیت", ActionLabel = "اتمام فعالیت و ارسال به فاکتور تسویه نشده" },
        
        new() { Controller = "CostGroup", Action = "Index", ControllerLabel = "گروه هزینه", ActionLabel = "نمایش" },
        new() { Controller = "CostGroup", Action = "Create", ControllerLabel = "گروه هزینه", ActionLabel = "ایجاد" },
        new() { Controller = "CostGroup", Action = "Edit", ControllerLabel = "گروه هزینه", ActionLabel = "ویرایش" },
        new() { Controller = "CostGroup", Action = "Delete", ControllerLabel = "گروه هزینه", ActionLabel = "حذف" },
        
        new() { Controller = "Creditor", Action = "Index", ControllerLabel = "بستانکار", ActionLabel = "نمایش" },
        new() { Controller = "Creditor", Action = "Create", ControllerLabel = "بستانکار", ActionLabel = "ایجاد" },
        new() { Controller = "Creditor", Action = "Edit", ControllerLabel = "بستانکار", ActionLabel = "ویرایش" },
        new() { Controller = "Creditor", Action = "Delete", ControllerLabel = "بستانکار", ActionLabel = "حذف" },
        
        new() { Controller = "EmployerPayment", Action = "Index", ControllerLabel = "پرداختی کارفرما", ActionLabel = "دستمزد پیمانکار" },
        new() { Controller = "EmployerPayment", Action = "Delete", ControllerLabel = "پرداختی کارفرما", ActionLabel = "حذف دستمزد پیمانکار" },
        new() { Controller = "EmployerPayment", Action = "ProjectCosts", ControllerLabel = "پرداختی کارفرما", ActionLabel = "هزینه پروژه" },
        new() { Controller = "EmployerPayment", Action = "Create", ControllerLabel = "پرداختی کارفرما", ActionLabel = "افزودن پرداخت" },
        new() { Controller = "EmployerPayment", Action = "AddImage", ControllerLabel = "پرداختی کارفرما", ActionLabel = "افزودن تصویر" },
        
        new() { Controller = "Project", Action = "All", ControllerLabel = "پروژه", ActionLabel = "نمایش تمام پروژه ها" },
        new() { Controller = "Project", Action = "Index", ControllerLabel = "پروژه", ActionLabel = "نمایش" },
        new() { Controller = "Project", Action = "Create", ControllerLabel = "پروژه", ActionLabel = "ایجاد" },
        new() { Controller = "Project", Action = "Edit", ControllerLabel = "پروژه", ActionLabel = "ویرایش" },
        new() { Controller = "Project", Action = "Delete", ControllerLabel = "پروژه", ActionLabel = "حذف" },
        new() { Controller = "Project", Action = "ProjectDetail", ControllerLabel = "پروژه", ActionLabel = "متراژ و برآورد" },
        new() { Controller = "Project", Action = "ProjectDetailItem", ControllerLabel = "پروژه", ActionLabel = "جزئیات متراژ و برآورد" },
        new() { Controller = "Project", Action = "AddProjectDetail", ControllerLabel = "پروژه", ActionLabel = "افزودن متراژ و برآورد" },
        new() { Controller = "Project", Action = "EditProjectDetail", ControllerLabel = "پروژه", ActionLabel = "ویرایش متراژ و برآورد" },
        new() { Controller = "Project", Action = "AddProjectDetailItem", ControllerLabel = "پروژه", ActionLabel = "افزودن جزئیات متراژ و برآورد" },
        new() { Controller = "Project", Action = "DeleteProjectDetailItem", ControllerLabel = "پروژه", ActionLabel = "حذف جزئیات متراژ و برآورد" },
        new() { Controller = "Project", Action = "Images", ControllerLabel = "پروژه", ActionLabel = "گالری" },
        new() { Controller = "Project", Action = "AddImages", ControllerLabel = "پروژه", ActionLabel = "افزودن تصویر" },
        new() { Controller = "Project", Action = "AddAlbum", ControllerLabel = "پروژه", ActionLabel = "افزودن آلبوم" },
        
        new() { Controller = "UnsettledInvoice", Action = "Index", ControllerLabel = "فاکتور های تسویه نشده", ActionLabel = "نمایش" },
        new() { Controller = "UnsettledInvoice", Action = "Edit", ControllerLabel = "فاکتور های تسویه نشده", ActionLabel = "ویرایش" },
        new() { Controller = "UnsettledInvoice", Action = "AddPayoff", ControllerLabel = "فاکتور های تسویه نشده", ActionLabel = "افزودن پرداخت" },
        new() { Controller = "UnsettledInvoice", Action = "DeletePayoff", ControllerLabel = "فاکتور های تسویه نشده", ActionLabel = "حذف پرداخت" },
        new() { Controller = "UnsettledInvoice", Action = "Done", ControllerLabel = "فاکتور های تسویه نشده", ActionLabel = "تسویه" },
        
        new() { Controller = "UnverifiedInvoice", Action = "Index", ControllerLabel = "فاکتورهای تایید نشده", ActionLabel = "نمایش" },
        new() { Controller = "UnverifiedInvoice", Action = "Create", ControllerLabel = "فاکتورهای تایید نشده", ActionLabel = "ایجاد" },
        new() { Controller = "UnverifiedInvoice", Action = "Edit", ControllerLabel = "فاکتورهای تایید نشده", ActionLabel = "ویرایش" },
        new() { Controller = "UnverifiedInvoice", Action = "Delete", ControllerLabel = "فاکتورهای تایید نشده", ActionLabel = "حذف" },
        new() { Controller = "UnverifiedInvoice", Action = "AddImage", ControllerLabel = "فاکتورهای تایید نشده", ActionLabel = "افزودن عکس" },
        new() { Controller = "UnverifiedInvoice", Action = "ChangeStatusToApproved", ControllerLabel = "فاکتورهای تایید نشده", ActionLabel = "تایید" },
        new() { Controller = "UnverifiedInvoice", Action = "ChangeStatusToRejected", ControllerLabel = "فاکتورهای تایید نشده", ActionLabel = "رد" },
        new() { Controller = "UnverifiedInvoice", Action = "AddToUnsettledInvoice", ControllerLabel = "فاکتورهای تایید نشده", ActionLabel = "افزودن به فاکتور های تسویه نشده" },
        
        new() { Controller = "ProjectCost", Action = "CashCart", ControllerLabel = "هزینه پروژه", ActionLabel = "کارت نقدی" },
        new() { Controller = "ProjectCost", Action = "CreateCashCart", ControllerLabel = "هزینه پروژه", ActionLabel = "افزودن کارت نقدی" },
        new() { Controller = "ProjectCost", Action = "DeleteCashCart", ControllerLabel = "هزینه پروژه", ActionLabel = "حذف کارت نقدی" },
        new() { Controller = "ProjectCost", Action = "Check", ControllerLabel = "هزینه پروژه", ActionLabel = "چک" },
        new() { Controller = "ProjectCost", Action = "DirectPayment", ControllerLabel = "هزینه پروژه", ActionLabel = "پرداخت مستقیم کارفرما" },
        
    ];
}

public class Permission
{
    public required string Controller { get; set; }
    public required string ControllerLabel { get; set; }
    public required string Action { get; set; }
    public required string ActionLabel { get; set; }
}