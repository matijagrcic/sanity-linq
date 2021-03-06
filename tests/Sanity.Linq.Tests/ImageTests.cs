using Sanity.Linq.CommonTypes;
using Sanity.Linq.Demo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sanity.Linq.Tests
{
    public class ImageTests : TestBase
    {
        

        [Fact]
        public async Task Image_Test()
        {

            var sanity = new SanityDataContext(Options);

            // Clear existing records
            await sanity.DocumentSet<Post>().Delete().CommitAsync();
            await sanity.DocumentSet<Author>().Delete().CommitAsync();
            await sanity.DocumentSet<Category>().Delete().CommitAsync();

            // Delete images
            await sanity.Images.Delete().CommitAsync();

            // Upload new image
            var imageUri = new Uri("https://www.sanity.io/static/images/opengraph/social.png");
            var image = (await sanity.Images.UploadAsync(imageUri)).Document;

            var category = new Category
            {
                CategoryId = "AUTHORS",
                Description = "Category for popular authors",
                Title = "Popular Authors",
                MainImage = new SanityImage
                {
                    Asset = new SanityReference<SanityImageAsset> { Ref = image.Id },
                }
            };
            await sanity.DocumentSet<Category>().Create(category).CommitAsync();

            // Link image to new author
            var author = new Author()
            {
                Name = "Joe Bloggs",
                Images = new SanityImage[] {
                    new SanityImage
                    {
                        Asset = new SanityReference<SanityImageAsset> { Ref = image.Id },
                    }
                },
                FavoriteCategories = new List<SanityReference<Category>>
                {
                    new SanityReference<Category>
                    {
                        Value = category
                    }
                }
            };

            await sanity.DocumentSet<Author>().Create(author).CommitAsync();

            var retrievedDoc = await sanity.DocumentSet<Author>().ToListAsync();

            Assert.True(retrievedDoc.FirstOrDefault()?.Images?.FirstOrDefault()?.Asset?.Value?.Extension != null);

            Assert.True(retrievedDoc.FirstOrDefault()?.FavoriteCategories?.FirstOrDefault().Value?.MainImage?.Asset?.Value?.Extension != null);

        }
    }
}
