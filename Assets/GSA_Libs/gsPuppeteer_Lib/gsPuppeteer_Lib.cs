using GpuScript;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using PuppeteerSharp;
using System.Xml;
#if UNITY_STANDALONE_WIN
using System.Security.Policy;
#endif //UNITY_STANDALONE_WIN

public class gsPuppeteer_Lib : gsPuppeteer_Lib_, IPuppeteer_Lib
{
	public IBrowser browser;
	public IPage page;
	public string ChromiumPath => @"D:\Unity6\Chromium\chrome-win\chrome.exe";
	public async Task Get_Browser_Async(bool isHeadless)
	{
		Close();
		BrowserFetcher browserFetcher = new();
		await browserFetcher.DownloadAsync();
		GS_Puppeteer_Lib.OpenChromium(this);
		browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = isHeadless, ExecutablePath = ChromiumPath, IgnoredDefaultArgs = new string[] { "--disable-extensions" } });
	}
	public async Task Get_Page_Async() { page?.Dispose(); page = (await browser.PagesAsync())[0]; }
	public async Task Get_Browser_Page_Async(bool isHeadless, string url = null, WaitUntilNavigation waitUntilNavigation = WaitUntilNavigation.DOMContentLoaded)
	{
		await Get_Browser_Async(isHeadless);
		await Get_Page_Async();
		if (url != null) await page.GoToAsync(url, waitUntilNavigation);
	}
	public async Task GoTo_Page_Async(string url = null, WaitUntilNavigation waitUntilNavigation = WaitUntilNavigation.DOMContentLoaded)
	{
		if (url != null) await page.GoToAsync(url, waitUntilNavigation);
	}
	public IEnumerator GoTo_Page_Sync(string url = null, WaitUntilNavigation waitUntilNavigation = WaitUntilNavigation.DOMContentLoaded)
	{
		Task t = GoTo_Page_Async(url, waitUntilNavigation);
		yield return new WaitUntil(() => t.IsCompleted);
	}
	public IEnumerator Get_Browser_Page_Sync(bool isHeadless, string url = null, WaitUntilNavigation waitUntilNavigation = WaitUntilNavigation.DOMContentLoaded)
	{
		Task t = Get_Browser_Page_Async(isHeadless, url, waitUntilNavigation);
		yield return new WaitUntil(() => t.IsCompleted);
	}
	public Coroutine Get_Browser_Page_Coroutine(bool isHeadless, string url = null, WaitUntilNavigation waitUntilNavigation = WaitUntilNavigation.DOMContentLoaded)
	{
		return StartCoroutine(Get_Browser_Page_Sync(isHeadless, url, waitUntilNavigation));
	}

	public string htmlText;
	//public async Task GetHtmlText(string url, WaitUntilNavigation waitUntilNavigation = WaitUntilNavigation.DOMContentLoaded)
	//{
	//	htmlText = "";
	//	await Get_Browser_Async(false);
	//	await Get_Page_Async();
	//	page.DefaultTimeout = 5000;
	//	if (url != null)
	//	{
	//		await page.GoToAsync(url, waitUntilNavigation);
	//		htmlText = await page.GetContentAsync();
	//	}
	//}
	public async Task GetHtmlText_Async() { htmlText = await page.GetContentAsync(); }
	public IEnumerator GetHtmlText_Sync() { Task t = GetHtmlText_Async(); yield return new WaitUntil(() => t.IsCompleted); }
	public Coroutine GetHtmlText_Coroutine() { return StartCoroutine(GetHtmlText_Sync()); }

	public async Task Take_Screenshot_Async(string outFile) { await page.ScreenshotAsync(outFile); }
	public async Task Take_Screenshot_Async(bool isHeadless, string url, string outFile) { await Get_Browser_Page_Async(isHeadless, url); await page.ScreenshotAsync(outFile); }
	public IEnumerator Take_Screenshot_Sync(string outFile) { Task t = Take_Screenshot_Async(outFile); yield return new WaitUntil(() => t.IsCompleted); }

	public async Task SendKeys_Async(params string[] keys)
	{
		foreach (var key in keys)
			if (key.EndsWith("_Down")) await page.Keyboard.DownAsync(key.Before("_Down"));
			else if (key.EndsWith("_Up")) await page.Keyboard.UpAsync(key.Before("_Up"));
			else await page.Keyboard.PressAsync(key);
	}
	public async Task Copy_Async() { await SendKeys_Async("Control_Down", "KeyC", "Control_Up"); }
	public async Task Paste_Async() { await SendKeys_Async("Control_Down", "KeyV", "Control_Up"); }
	public Coroutine Paste_Coroutine() { Task t = Paste_Async(); return StartCoroutine(new WaitUntil(() => t.IsCompleted)); }
	public async Task SelectAll_Async() { await SendKeys_Async("Control_Down", "KeyA", "Control_Up"); }
	public Coroutine SelectAll_Coroutine() { Task t = SelectAll_Async(); return StartCoroutine(new WaitUntil(() => t.IsCompleted)); }
	public async Task SelectAll_Copy_Async() { await SendKeys_Async("Control_Down", "KeyA", "KeyC", "Control_Up"); }
	public async Task SelectAll_Paste_Async() { await SendKeys_Async("Control_Down", "KeyA", "KeyV", "Control_Up"); }
	public Coroutine SelectAll_Copy_Coroutine() { Task t = SelectAll_Copy_Async(); return StartCoroutine(new WaitUntil(() => t.IsCompleted)); }
	public Coroutine SelectAll_Paste_Coroutine() { Task t = SelectAll_Paste_Async(); return StartCoroutine(new WaitUntil(() => t.IsCompleted)); }
	public IEnumerator Paste_Text_Sync(string text) { Clipboard = text; Task t = Paste_Async(); yield return new WaitUntil(() => t.IsCompleted); }

	public async Task Tab_Async(int n) { for (int i = 0; i < n; i++) await page.Keyboard.PressAsync("Tab"); }
	public IEnumerator Tab_Sync(int n) { for (int i = 0; i < n; i++) { Task t = page.Keyboard.PressAsync("Tab"); yield return new WaitUntil(() => t.IsCompleted); } }
	public Coroutine Tab_Coroutine(int n) => StartCoroutine(Tab_Sync(n));

	public async Task Press_Tab_Async() { await page.Keyboard.PressAsync("Tab"); }
	public IEnumerator Press_Tab_Sync() { Task t = Press_Tab_Async(); yield return new WaitUntil(() => t.IsCompleted); }
	public async Task Press_Enter_Async() { await page.Keyboard.PressAsync("Enter"); }
	public IEnumerator Press_Enter_Sync() { Task t = Press_Enter_Async(); yield return new WaitUntil(() => t.IsCompleted); }
	public async Task Delay_Async(int ms) { await Task.Delay(ms); }
	public IEnumerator Delay_Sync(int ms) { Task t = Delay_Async(ms); yield return new WaitUntil(() => t.IsCompleted); }

	public async Task MouseClick_Async(int x, int y) { await page.Mouse.ClickAsync(x - 15, y - 15); }
	public IEnumerator MouseClick_Sync(int x, int y) { Task t = MouseClick_Async(x, y); yield return new WaitUntil(() => t.IsCompleted); }
	public Coroutine MouseClick_Coroutine(int x, int y) { Task t = MouseClick_Async(x, y); return StartCoroutine(new WaitUntil(() => t.IsCompleted)); }
	public Coroutine Run(Func<Task> f) { Task t = Task.Run(f); return StartCoroutine(new WaitUntil(() => t.IsCompleted)); }

	public IEnumerator WaitForNetworkIdle_Sync() { Task t = page.WaitForNetworkIdleAsync(); yield return new WaitUntil(() => t.IsCompleted); }
	public Coroutine WaitForNetworkIdle_Coroutine() => StartCoroutine(WaitForNetworkIdle_Sync());

	string _translation;
	public string translation { get => _translation; set => _translation = value; }
	public IEnumerator Translate_Sync(string s)
	{
		yield return MouseClick_Coroutine(177, 224);
		Clipboard = s;
		yield return SelectAll_Paste_Coroutine();
		yield return WaitForNetworkIdle_Coroutine();
		yield return Tab_Coroutine(4);
		yield return SelectAll_Copy_Coroutine();
		GS_Puppeteer_Lib.GetTranslation(this, Clipboard);
	}
	public Coroutine GoogleTranslate(string s) => StartCoroutine(Translate_Sync(s));
	public IEnumerator Open_Google_Translate_Browser_Sync(string sl, string tl)
	{
		yield return StartCoroutine(Get_Browser_Page_Sync(false, $"https://translate.google.com/?sl={sl}&tl={tl}&op=translate"));
	}
	public Coroutine Open_Google_Language_Translate_Browser(string toLanguage) => StartCoroutine(Open_Google_Translate_Browser_Sync("en", GS_Puppeteer_Lib.GetLanguageCode(toLanguage)));
	public Coroutine Open_Google_Language_Translate_Browser(string fromLanguage, string toLanguage) => StartCoroutine(Open_Google_Translate_Browser_Sync(GS_Puppeteer_Lib.GetLanguageCode(fromLanguage), GS_Puppeteer_Lib.GetLanguageCode(toLanguage)));

	public Coroutine Open_Google_Translate_Browser(string tl) => StartCoroutine(Open_Google_Translate_Browser_Sync("en", tl));
	public Coroutine Open_Google_Translate_Browser(string sl, string tl) => StartCoroutine(Open_Google_Translate_Browser_Sync(sl, tl));

	private void OnDestroy() { Close(); }
	public void Close() { page?.Dispose(); page = null; browser?.Dispose(); browser = null; }
}