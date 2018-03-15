namespace BikeTouringGISApp.UnitTests.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Toolkit.Uwp.Services.Facebook;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FaceBookServiceTest
    {
        [TestMethod]
        public async Task GetPagePictureUrl()
        {
            Assert.Inconclusive();
            await Login();
            var pages = await FacebookService.Instance.RequestAsync<FacebookPage>(FacebookDataConfig.MyPages);
            var pageId = pages.First().Id;
//            var imageData = await FacebookService.Instance.GetPagePictureInfoAsync(pageId);
//            Assert.AreEqual("https://scontent.xx.fbcdn.net/v/t1.0-1/p50x50/14568257_1795295790740226_1853951626961530489_n.png?oh=61ce1acf6bdfd89bc35b49aef19d8d55&oe=59FC7FAF", imageData.Url);
        }

        [TestMethod]
        public async Task GetPagesForUser()
        {
            await Login();
            var pages = await FacebookService.Instance.RequestAsync<FacebookPage>(FacebookDataConfig.MyPages);
            Assert.AreEqual(6, pages.Count);
        }

        [TestMethod]
        public async Task GetPlaces()
        {
            var latitude = 53.196981;
            var longitude = 6.584016;
            await Login();
            var places = await FacebookService.Instance.GetPlacesByCoordinatesAsync(latitude, longitude);
            Assert.AreEqual(10, places.Count);
        }

        [TestMethod]
        public async Task GetUserPictureUrl()
        {
            await Login();
            var imageData = await FacebookService.Instance.GetUserPictureInfoAsync();
            Assert.AreEqual("https://scontent.xx.fbcdn.net/v/t1.0-1/p50x50/16387445_10154864101373529_7907991150593038797_n.jpg?oh=c3287ae14b9550a2376318fe8ec77259&oe=59F344B1", imageData.Url);
        }

        private async Task Login()
        {
            FacebookService.Instance.Initialize("687964081409306", FacebookPermissions.PublicProfile | FacebookPermissions.UserPosts | FacebookPermissions.PublishActions | FacebookPermissions.UserPhotos | FacebookPermissions.ManagePages);
            await FacebookService.Instance.LoginAsync();
        }
    }
}