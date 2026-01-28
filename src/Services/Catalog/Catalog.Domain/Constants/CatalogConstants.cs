namespace Catalog.Domain.Constants;

/// <summary>
/// Catalog domain için iş kuralı sabitleri
/// </summary>
public static class CatalogConstants
{
    #region Product Constants
    
    public static class Product
    {
        /// <summary>
        /// Ürün adı maksimum uzunluğu
        /// Business: SEO ve UX için 100 karakter limit
        /// </summary>
        public const int NameMaxLength = 100;
        
        public const int NameMinLength = 3;

        /// <summary>
        /// Ürün açıklaması maksimum uzunluğu
        /// Business: Detaylı açıklama için yeterli alan
        /// </summary>
        public const int DescriptionMaxLength = 500;
        
        public const int DescriptionMinLength = 10;

        /// <summary>
        /// Resim dosya adı maksimum uzunluğu
        /// Technical: Dosya sistemi limitleri
        /// </summary>
        public const int PictureFileNameMaxLength = 255;

        /// <summary>
        /// Maksimum ürün fiyatı (TL)
        /// Business: Fraud önleme ve sistem limiti
        /// </summary>
        public const decimal MaxPrice = 1_000_000m;
        
        /// <summary>
        /// Minimum ürün fiyatı (TL)
        /// Business: Operasyonel maliyet
        /// </summary>
        public const decimal MinPrice = 0.01m;

        /// <summary>
        /// Maksimum stok miktarı
        /// Business: Depo kapasitesi ve gerçekçilik
        /// </summary>
        public const int MaxStock = 100_000;
        
        public const int MinStock = 0;
    }
    
    #endregion

    #region Category Constants
    
    public static class Category
    {
        /// <summary>
        /// Kategori adı maksimum uzunluğu
        /// </summary>
        public const int NameMaxLength = 100;
        
        public const int NameMinLength = 2;

        /// <summary>
        /// Maksimum kategori ağacı derinliği
        /// Business: UX ve performans için limit
        /// Ana > Alt > Alt-Alt > ... (max 5 seviye)
        /// </summary>
        public const int MaxDepth = 5;

        /// <summary>
        /// Bir kategoride olabilecek maksimum alt kategori sayısı
        /// Business: UX karmaşıklığını önleme
        /// </summary>
        public const int MaxSubCategories = 50;
    }
    
    #endregion

    #region Pagination Constants
    
    public static class Pagination
    {
        /// <summary>
        /// Varsayılan sayfa boyutu
        /// </summary>
        public const int DefaultPageSize = 10;
        
        /// <summary>
        /// Maksimum sayfa boyutu
        /// Business: DB ve API performansı
        /// </summary>
        public const int MaxPageSize = 100;
        
        public const int MinPageSize = 1;
    }
    
    #endregion
}
