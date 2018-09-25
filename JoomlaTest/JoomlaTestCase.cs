using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace JoomlaTest
{
    [TestClass]
    public class JoomlaTestCase
    {
        IWebDriver driver;
        Random rdn;
       
        [TestInitialize]
        public void Init()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            navigateToWebpage("http://192.168.190.247/joomlatest/administrator/index.php");
            driver.Manage().Window.Maximize();
            login("lctp", "lctp");
        }

        [TestMethod]
        public void TC_JOOMLA_ARTICLE_001()
        {
            rdn = new Random();
            string title = "Article Test " + rdn.Next(10000);
            string categoryItem = "Sample Data-Articles";
            string paragraph = "this is article content";
            string successMessage = "Article saved.";
            string saveType = "Save&Close";

            createNewArticle(title, null, categoryItem, null, null, paragraph, saveType);
            checkSuccessMessageExist(successMessage);
            checkArticleExist(title, null, categoryItem);
        }
        [TestMethod]
        public void TC_JOOMLA_ARTICLE_002()
        {
            rdn = new Random();
            string title = "Article Test " + rdn.Next(10000);
            string titleEdit = title + "_update";
            string categoryItem = "Sample Data-Articles";
            string categoryItemEdit = "- Park Site";
            string paragraph = "this is article content";
            string successMessage = "Article saved.";
            string saveType = "Save&Close";

            createNewArticle(title, null, categoryItem, null, null, paragraph, saveType);
            checkArticleExist(title, null, categoryItem);
            editArticle(title,titleEdit, null, categoryItemEdit, null, null, null, saveType);
            checkSuccessMessageExist(successMessage);
            checkArticleExist(titleEdit, null, categoryItemEdit);
        }
        [TestMethod]
        public void TC_JOOMLA_ARTICLE_003()
        {
            rdn = new Random();
            string title = "Article Test " + rdn.Next(10000);
            string statusItem = "Unpublished";
            string categoryItem = "Sample Data-Articles";
            string paragraph = "this is article content";
            string saveType = "Save&Close";
            string statusChange = "Published";
            string successMessage = "1 article published.";

            createNewArticle(title, statusItem, categoryItem, null, null, paragraph, saveType);
            checkArticleExist(title, statusItem, categoryItem);
            changeStatusArticle(title, statusChange);
            checkSuccessMessageExist(successMessage);
            checkChangeStatus(title, statusChange);
        }

        [TestMethod]
        public void TC_JOOMLA_ARTICLE_004()
        {
            rdn = new Random();
            string title = "Article Test " + rdn.Next(10000);
            string statusItem = "Published";
            string categoryItem = "Sample Data-Articles";
            string paragraph = "this is article content";
            string saveType = "Save&Close";
            string successMessage = "1 article unpublished.";
            string statusChange = "Unpublished";

            createNewArticle(title, statusItem, categoryItem, null, null, paragraph, saveType);
            checkArticleExist(title, statusItem, categoryItem);
            changeStatusArticle(title, statusChange);
            checkSuccessMessageExist(successMessage);
            checkChangeStatus(title, statusChange);
        }

        [TestMethod]
        public void TC_JOOMLA_ARTICLE_005()
        {
            rdn = new Random();
            string title = "Article Test " + rdn.Next(10000);
            string statusItem = "Published";
            string categoryItem = "Sample Data-Articles";
            string paragraph = "this is article content";
            string saveType = "Save&Close";
            string successMessage = "1 article archived.";
            string statusFilter = "Archived";
            string statusChange = "Archived"; ;

            createNewArticle(title, statusItem, categoryItem, null, null, paragraph, saveType);
            checkArticleExist(title, statusItem, categoryItem);
            changeStatusArticle(title, statusChange);
            checkSuccessMessageExist(successMessage);
            selectFilterDropList(statusFilter, null, null, null);
            checkArticleExist(title, statusChange, categoryItem);
        }


        [TestCleanup]
        public void CleanUp()
        {
            driver.Close();
        }

        
        #region Navigate to Web
        public void navigateToWebpage(string url)
        {
            driver.Navigate().GoToUrl(url);
        }
        #endregion
        #region Login
        public void login(string username, string password)
        {
            driver.FindElement(By.Id("mod-login-username")).SendKeys(username);
            driver.FindElement(By.Id("mod-login-password")).SendKeys(password);
            driver.FindElement(By.ClassName("login-button")).Click();
        }
        #endregion
        #region Go To Article Page
        public void goToArticlePage()
        {
            driver.FindElement(By.LinkText("Content")).Click();
            driver.FindElement(By.LinkText("Articles")).Click();
        }
        #endregion
        #region Click on toolbar button
        public void clickToolbarButton(string buttonName)
        {

            switch (buttonName)
            {
                case "New":
                    driver.FindElement(By.Id("toolbar-new")).Click();
                    break;
                case "Edit":
                    driver.FindElement(By.Id("toolbar-edit")).Click();
                    break;
                case "Published":
                    driver.FindElement(By.Id("toolbar-publish")).Click();
                    break;
                case "Unpublished":
                    driver.FindElement(By.Id("toolbar-unpublish")).Click();
                    break;
                case "Archived":
                    driver.FindElement(By.Id("toolbar-archive")).Click();
                    break;
            }
        }
        #endregion
        #region Select drop list
        public void selectDropList(string statusItems, string categoryItems, string accessItems, string languageItems)
        {
            if (statusItems != null)
            {
                driver.FindElement(By.Id("jform_state_chzn")).Click();
                driver.FindElement(By.XPath("//div[@id='jform_state_chzn']//li[contains(@class,'active-result') and text()=\"" + statusItems + "\"]")).Click();
            }
            if (categoryItems != null)
            {
                driver.FindElement(By.Id("jform_catid_chzn")).Click();
                driver.FindElement(By.XPath("//div[@id='jform_catid_chzn']//li[contains(@class,'active-result') and text()=\"" + categoryItems + "\"]")).Click();
            }
            if (accessItems != null)
            {
                driver.FindElement(By.Id("jform_access_chzn")).Click();
                driver.FindElement(By.XPath("//div[@id='jform_access_chzn']//li[contains(@class,'active-result') and text()=\"" + accessItems + "\"]")).Click();
            }
            if (languageItems != null)
            {
                driver.FindElement(By.Id("jform_language_chzn")).Click();
                driver.FindElement(By.XPath("//div[@id='jform_language_chzn']//li[contains(@class,'active-result') and text()=\"" + languageItems + "\"]")).Click();
            }
        }
        #endregion
        #region Select save type
        public void selectSaveType(string saveType)
        {       
            switch(saveType)
            {
                case "Save":
                    driver.FindElement(By.CssSelector("#toolbar-apply > button")).Click();
                    break;
                case "Save&Close":
                    driver.FindElement(By.CssSelector("#toolbar-save > button")).Click();
                    break;
                case "Save&New":
                    driver.FindElement(By.CssSelector("#toolbar-save-new > button")).Click();
                    break;
                case "Cancel":
                    driver.FindElement(By.CssSelector("#toolbar-cancel > button")).Click();
                    break;
                default:
                    break;
            }       
        }
        #endregion
        #region Click on recently article
        public void checkOnRecentArticle(string title)
        {
            driver.FindElement(By.XPath("//a[normalize-space()=\"" + title + "\"]/ancestor::tr//input[@type='checkbox']")).Click();
        }
        #endregion
        #region Fill article information
        public void fillArticleInformation(string title, string statusItems, string categoryItems, string accessItems, string languageItems, string paragraph)
        {
            if (title != null)
            {
                driver.FindElement(By.Id("jform_title")).Clear();
                driver.FindElement(By.Id("jform_title")).SendKeys(title);
            }

            selectDropList(statusItems, categoryItems, accessItems, languageItems);

            if (paragraph != null)
            {
                IWebElement tinymceFrame = driver.FindElement(By.Id("jform_articletext_ifr"));
                driver.SwitchTo().Frame(tinymceFrame);
                IWebElement body = driver.FindElement(By.Id("tinymce"));
                body.Clear();
                body.SendKeys(paragraph);
                driver.SwitchTo().DefaultContent();
            }
        }
        #endregion
        #region Create new article
        public void createNewArticle(string title, string statusItems, string categoryItems, string accessItems, string languageItems, string paragraph, string saveType)
        {
            goToArticlePage();
            clickToolbarButton("New");
            fillArticleInformation(title, statusItems, categoryItems, accessItems, languageItems, paragraph);
            selectSaveType(saveType);
        }
        #endregion
        #region Edit article
        public void editArticle(string title,string titleEdit, string statusItems, string categoryItems, string accessItems, string languageItems, string paragraph, string saveType)
        {
            goToArticlePage();
            checkOnRecentArticle(title);
            clickToolbarButton("Edit");
            fillArticleInformation(titleEdit, statusItems, categoryItems, accessItems, languageItems, paragraph);
            selectSaveType(saveType);
        }
        #endregion
        #region Change status article
        public void changeStatusArticle(string title, string statusChange)
        {
            checkOnRecentArticle(title);
            clickToolbarButton(statusChange);
        }
        #endregion
        #region Check article exist
        public void checkArticleExist(string title, string statusItem, string categoryItem)
        {
            try
            {
                Assert.IsTrue(driver.FindElement(By.XPath("//a[normalize-space()=\"" + title + "\"]")).Displayed);
                Assert.AreEqual(driver.FindElement(By.XPath("//a[normalize-space()=\"" + title + "\"]/parent::div//div[@class='small']")).Text, "Category: " + categoryItem);
                if (statusItem != null)
                {
                    switch (statusItem)
                    {
                        case "Published":
                            Assert.IsTrue(driver.FindElement(By.XPath("//a[normalize-space()=\"" + title + "\"]/ancestor::tr//span[@class='icon-publish']")).Displayed);
                            break;
                        case "Unpublished":
                            Assert.IsTrue(driver.FindElement(By.XPath("//a[normalize-space()=\"" + title + "\"]/ancestor::tr//span[@class='icon-unpublish']")).Displayed);
                            break;
                        case "Archived":
                            Assert.IsTrue(driver.FindElement(By.XPath("//a[normalize-space()=\"" + title + "\"]/ancestor::tr//span[@class='icon-archive']")).Displayed);
                            break;
                        case "Trashed":
                            Assert.IsTrue(driver.FindElement(By.XPath("//a[normalize-space()=\"" + title + "\"]/ancestor::tr//span[@class='icon-trash']")).Displayed);
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        #endregion
        #region Check change of article status
        public void checkChangeStatus(string title, string status)
        {
            try
            {
                switch (status)
                {
                    case "Published":
                        Assert.IsTrue(driver.FindElement(By.XPath("//a[normalize-space()=\"" + title + "\"]/ancestor::tr//span[@class='icon-publish']")).Displayed);
                        break;
                    case "Unpublished":
                        Assert.IsTrue(driver.FindElement(By.XPath("//a[normalize-space()=\"" + title + "\"]/ancestor::tr//span[@class='icon-unpublish']")).Displayed);
                        break;
                    case "Archived":
                        Assert.IsTrue(driver.FindElement(By.XPath("//a[normalize-space()=\"" + title + "\"]/ancestor::tr//span[@class='icon-archive']")).Displayed);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
        #region Select filter droplist
        public void selectFilterDropList(string statusItems, string categoryItems, string accessItems, string authorItems)
        {
            driver.FindElement(By.ClassName("js-stools-btn-filter")).Click();
            System.Threading.Thread.Sleep(1000);
            if (statusItems != null)
            {
                driver.FindElement(By.Id("filter_published_chzn")).Click();
                driver.FindElement(By.XPath("//div[@id='filter_published_chzn']//li[contains(@class,'active-result') and text()=\"" + statusItems + "\"]")).Click();
            }
            if (categoryItems != null)
            {
                driver.FindElement(By.Id("filter_catid_chzn")).Click();
                driver.FindElement(By.XPath("//div[@id='filter_catid_chzn']//li[contains(@class,'active-result') and text()=\"" + statusItems + "\"]")).Click();
            }
            if (accessItems != null)
            {
                driver.FindElement(By.Id("filter_access_chzn")).Click();
                driver.FindElement(By.XPath("//div[@id='filter_access_chzn']//li[contains(@class,'active-result') and text()=\"" + statusItems + "\"]")).Click();
            }
            if (authorItems != null)
            {
                driver.FindElement(By.Id("filter_author_chzn")).Click();
                driver.FindElement(By.XPath("//div[@id='filter_author_chzn']//li[contains(@class,'active-result') and text()=\"" + statusItems + "\"]")).Click();
            }
        }
        #endregion
        #region Check success message display
        public void checkSuccessMessageExist(string messageSuccess)
        {
            try
            {
                Assert.IsTrue(driver.FindElement(By.XPath("//div[@class='alert alert-success']/div[@class='alert-message']")).Text.Equals(messageSuccess));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        #endregion  
    }
}
