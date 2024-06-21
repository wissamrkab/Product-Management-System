using PMS.Persistence.Contexts;

namespace PMS.Persistence;

public static class SeedData
{
    public static async Task Initialize(ApplicationDbContext context
        )
    {
        await context.Database.EnsureCreatedAsync();
        
        SeedProduct(context);
        
        await context.SaveChangesAsync();

    }
    private static void SeedProduct(ApplicationDbContext context)
    {
        // Check if ProductOption table is already seeded
        // if (!context.Products.ToList().Exists(x => x.IsDefault))
        // {
        //     // Seed ProductOptionValue table for Pattern
        //     // var warrantyOption = context.Products.FirstOrDefault(o => o.Title == "Warranty");
        //     // if (warrantyOption != null)
        //     // {
        //     //     context.ProductOptionValues.AddRange(
        //     //         new ProductOptionValue("Standard Warranty (1 year)", warrantyOption.Id, true),
        //     //         new ProductOptionValue("Extended Warranty (2 years)", warrantyOption.Id, true),
        //     //         new ProductOptionValue("Premium Warranty (3 years)", warrantyOption.Id, true),
        //     //         new ProductOptionValue("No Warranty", warrantyOption.Id, true)
        //     //     );
        //     // }
        // }
    }

}