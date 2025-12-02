using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Core.Security.Constants;

public static class GeneralOperationClaims
{
    // Temel Roller
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Moderator = "Moderator";

    // Ürün Yönetimi
    public const string ProductAdd = "Product.Add";
    public const string ProductUpdate = "Product.Update";
    public const string ProductDelete = "Product.Delete";
    public const string ProductRead = "Product.Read";

    // Kategori Yönetimi
    public const string CategoryAdd = "Category.Add";
    public const string CategoryUpdate = "Category.Update";
    public const string CategoryDelete = "Category.Delete";
    public const string CategoryRead = "Category.Read";

    // Sipariş Yönetimi
    public const string OrderAdd = "Order.Add";
    public const string OrderUpdate = "Order.Update";
    public const string OrderDelete = "Order.Delete";
    public const string OrderRead = "Order.Read";
    public const string OrderApprove = "Order.Approve";

    // Kullanıcı Yönetimi
    public const string UserAdd = "User.Add";
    public const string UserUpdate = "User.Update";
    public const string UserDelete = "User.Delete";
    public const string UserRead = "User.Read";

    // Rol Yönetimi
    public const string RoleAdd = "Role.Add";
    public const string RoleUpdate = "Role.Update";
    public const string RoleDelete = "Role.Delete";
    public const string RoleRead = "Role.Read";
}
