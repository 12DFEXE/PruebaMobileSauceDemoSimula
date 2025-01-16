using Microsoft.Playwright;
using System.Threading.Tasks;
using Xunit;

namespace playwrightTestsMobile
{
    public class SauceDemoTests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IBrowserContext _iphoneContext;
        private IBrowserContext _samsungContext;
        private IPage _iphonePage;
        private IPage _samsungPage;

        // Configuración antes de las pruebas
        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions 
            { Headless = false
            });



            // Contexto para iPhone 12 Pro Max
            _iphoneContext = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 390, Height = 844 },
                UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/537.36 (KHTML, like Gecko) Safari/604.1"
            });
            _iphonePage = await _iphoneContext.NewPageAsync();

            // Contexto para Samsung Galaxy S24
            _samsungContext = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize { Width = 412, Height = 915 },
                UserAgent = "Mozilla/5.0 (Linux; Android 13; SM-S24) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Mobile Safari/537.36"
            });
            _samsungPage = await _samsungContext.NewPageAsync();
        }

        // Limpieza después de las pruebas
        public async Task DisposeAsync()
        {
            await _iphonePage.CloseAsync();
            await _iphoneContext.CloseAsync();
            await _samsungPage.CloseAsync();
            await _samsungContext.CloseAsync();//cambio para ejecucion automatica
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [Fact]
        [Trait("Category", "CrossDevice")]
        public async Task ValidationSafari()
        {
            await _iphonePage.GotoAsync("https://www.saucedemo.com/");
            await _iphonePage.FillAsync("#user-name", "standard_user");
            await _iphonePage.FillAsync("#password", "secret_sauce");
            await _iphonePage.ClickAsync("#login-button");
            await Task.Delay(2000); // Espera 2 segundos


            // Validar inventario
            var inventoryItems = await _iphonePage.Locator(".inventory_item").CountAsync();
            Assert.True(inventoryItems > 0, "Inventory items are not loaded.");
        }

        [Fact]
        [Trait("Category", "CrossDevice")]
        public async Task LoginAndInventoryValidation_Samsung()
        {
            await _samsungPage.GotoAsync("https://www.saucedemo.com/");
            await _samsungPage.FillAsync("#user-name", "standard_user");
            await _samsungPage.FillAsync("#password", "secret_sauce");
            await _samsungPage.ClickAsync("#login-button");
            await Task.Delay(2000); // Espera 2 segundos


            // Validar inventario
            var inventoryItems = await _samsungPage.Locator(".inventory_item").CountAsync();
            Assert.True(inventoryItems > 0, "Inventory items are not loaded.");
        }
    }
}
