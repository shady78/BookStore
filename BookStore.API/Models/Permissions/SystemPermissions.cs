namespace BookStore.API.Models.Permissions
{
    public static class SystemPermissions
    {
        //public const string EmployeeView = "Employees.View";
        //public const string EmployeeView = "Employees.View";
        //public const string EmployeeView = "Employees.View";
        //public const string EmployeeView = "Employees.View";

        public static class Modules
        {
            public const string Authors = "Authors";
            public const string Books = "Books";
        }
        public static class Authors
        {
            public const string View = "Authors.View";
            public const string Create = "Authors.Create";
            public const string Update = "Authors.Update";
            public const string Delete = "Authors.Delete";
        }
        public static class Books
        {
            public const string View = "Books.View";
            public const string Create = "Books.Create";
            public const string Update = "Books.Update";
            public const string Delete = "Books.Delete";
        }

        // store permission in DB
        public static readonly PermissionDefinition[] All =
            [
            new (Authors.View,"Can view Authors",Modules.Authors),
            new (Authors.Create,"Can create Authors",Modules.Authors),
            new (Authors.Update,"Can Update Authors",Modules.Authors),
            new (Authors.Delete,"Can Delete Authors",Modules.Authors),
            new (Books.View,"Can view Books",Modules.Books),
            new (Books.Update,"Can Update Books",Modules.Books),
            new (Books.Create,"Can Create Books",Modules.Books),
            new (Books.Delete,"Can Delete Books",Modules.Books)
            ];
    }
}
