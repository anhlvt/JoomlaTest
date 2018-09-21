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
        IWebDriver driver = new ChromeDriver();
        string url = "http://192.168.189.152/satt02/administrator/index.php";        

        //
        [TestInitialize]
        public void Init()
        {
            navigateToWebpage(url);
            driver.Manage().Window.Maximize();
            login("andyndh93", "andyndh93");
            
        }

        [TestMethod]
        public void TestCase1()
        {
            addNewArticle("Article Test 3", null, "Sample Data-Articles", null, null, "this is article content");
            Assert.AreEqual(driver.FindElement(By.XPath("//div[@class='alert alert-success']/div[@class='alert-message']")).Text, "Article saved.");
            ReadOnlyCollection<IWebElement> rowCollection = driver.FindElements(By.XPath("//tbody/tr"));
            int rowCount = rowCollection.Count;
            for (int i = 1; i <= rowCount; i++)
            {
                string titleActual = driver.FindElement(By.XPath("//tbody//tr[" + i + "]//a[@data-original-title='Edit']")).Text;
                Assert.AreEqual(titleActual,"Article Test 3");
                string categoryActual = driver.FindElement(By.XPath("//tbody//tr[" + i + "]//div[@class='small']")).Text;
                Assert.AreEqual(categoryActual,"Sample Data-Articles");

            }
        }
        [TestMethod]
        public void TestCase2()
        {
            addNewArticle("Article Test 2", null, "Sample Data-Articles", null, null, "this is article content");
            editArticle("Article Test_update", null, "- Park Site", null, null, null);
            Assert.AreEqual(driver.FindElement(By.XPath("//div[@class='alert alert-success']/div[@class='alert-message']")).Text, "Article saved.");
            //Assert.

        }
        [TestMethod]
        public void TestCase3()
        {
            addNewArticle("Article Test 3","Unpublished", "Sample Data-Articles", null, null, "this is article content");
            Assert.AreEqual(driver.FindElement(By.XPath("//div[@class='alert alert-success']/div[@class='alert-message']")).Text, "Article saved.");
            publishArticle();            
            Assert.AreEqual(driver.FindElement(By.XPath("//div[@class='alert alert-success']/div[@class='alert-message']")).Text, "1 article published.");
            //Assert.

        }

        [TestMethod]
        public void TestCase4()
        {
            addNewArticle("Article Test 4", "Published", "Sample Data-Articles", null, null, "this is article content");
            Assert.AreEqual(driver.FindElement(By.XPath("//div[@class='alert alert-success']/div[@class='alert-message']")).Text, "Article saved.");
            unPublishArticle();
            Assert.AreEqual(driver.FindElement(By.XPath("//div[@class='alert alert-success']/div[@class='alert-message']")).Text, "1 article unpublished.");
            //Assert.

        }

        [TestMethod]
        public void TestCase5()
        {
            addNewArticle("Article Test 6", "Published", "Sample Data-Articles", null, null, "this is article content");
            Assert.AreEqual(driver.FindElement(By.XPath("//div[@class='alert alert-success']/div[@class='alert-message']")).Text, "Article saved.");
            archiveArticle();            
            Assert.AreEqual(driver.FindElement(By.XPath("//div[@class='alert alert-success']/div[@class='alert-message']")).Text, "1 article archived.");
            selectFilterDropList("Archived", null, null,null);
            //Assert.

        }

        [TestCleanup]
        public void CleanUp()
        {
            logout();
            driver.Close();
        }
        //open website
        public void navigateToWebpage(string url)
        {
            driver.Navigate().GoToUrl(url);
        }
        //login joomla
        public void login(string username, string password)
        {
           driver.FindElement(By.Id("mod-login-username")).SendKeys(username); 
           driver.FindElement(By.Id("mod-login-password")).SendKeys(password);
           driver.FindElement(By.ClassName("login-button")).Click();            
        }
        //logout
        public void logout()
        {
            driver.FindElement(By.PartialLinkText("User Menu")).Click();
            driver.FindElement(By.LinkText("Logout")).Click();
        }

        //add new article
        public void addNewArticle(string articleTitle, string status, string category, string access,string language, string paragraph)
        {
            driver.FindElement(By.LinkText("Content")).Click();
            driver.FindElement(By.LinkText("Articles")).Click();
            driver.FindElement(By.ClassName("btn-success")).Click();
            driver.FindElement(By.Id("jform_title")).SendKeys(articleTitle);
            //select status dropdown list 
            if(status!=null)
            { 
                driver.FindElement(By.Id("jform_state_chzn")).Click();
                ReadOnlyCollection<IWebElement> statusItems = driver.FindElements(By.XPath("//div[@id='jform_state_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in statusItems)
                {
                    if (item.Text.Equals(status))
                    {
                        item.Click();
                        break;
                    }
                }
                
            }
            //select category dropdown list
            if (category!=null)
            { 
                driver.FindElement(By.Id("jform_catid_chzn")).Click();
                ReadOnlyCollection<IWebElement> categoryItems = driver.FindElements(By.XPath("//div[@id='jform_catid_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in categoryItems)
                {
                    if (item.Text.Equals(category))
                    {
                        item.Click();
                        break;
                    }
                }
            }

            //select access dropdown list
            if(access!=null)
            { 
                driver.FindElement(By.Id("jform_access_chzn")).Click();
                ReadOnlyCollection<IWebElement> accessItems = driver.FindElements(By.XPath("//div[@id='jform_access_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in accessItems)
                {
                    if (item.Text.Equals(access))
                    {
                        item.Click();
                        break;
                    }
                }
            }

            //select language dropdown list
            if(language!=null)
            { 
                driver.FindElement(By.Id("jform_language_chzn")).Click();
                ReadOnlyCollection<IWebElement> languageItems = driver.FindElements(By.XPath("//div[@id='jform_language_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in languageItems)
                {
                    if (item.Text.Equals(language))
                    {
                        item.Click();
                        break;
                    }
                }
            }
            //enter textarea
            if (paragraph != null)
            {
                IWebElement tinymceFrame = driver.FindElement(By.Id("jform_articletext_ifr"));
                driver.SwitchTo().Frame(tinymceFrame);
                IWebElement body = driver.FindElement(By.Id("tinymce"));
                body.SendKeys(paragraph);
                driver.SwitchTo().DefaultContent();
            }           

            //save and close
            driver.FindElement(By.CssSelector("#toolbar-save > button")).Click();            

        }

        public void editArticle(string titleModify, string statusModify, string categoryModify, string accessModify, string languageModify, string paragraphModify)
        {
            driver.FindElement(By.LinkText("Content")).Click();
            driver.FindElement(By.LinkText("Articles")).Click();
            driver.FindElement(By.Id("cb0")).Click();
            driver.FindElement(By.CssSelector("#toolbar-edit > button")).Click();

            driver.FindElement(By.Id("jform_title")).Clear();
            driver.FindElement(By.Id("jform_title")).SendKeys(titleModify);
            //select status dropdown list 
            if (statusModify != null)
            {
                driver.FindElement(By.Id("jform_state_chzn")).Click();
                ReadOnlyCollection<IWebElement> statusItems = driver.FindElements(By.XPath("//div[@id='jform_state_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in statusItems)
                {
                    if (item.Text.Equals(statusModify))
                    {
                        item.Click();
                        break;
                    }
                }

            }
            //select category dropdown list
            if (categoryModify != null)
            {
                driver.FindElement(By.Id("jform_catid_chzn")).Click();
                ReadOnlyCollection<IWebElement> categoryItems = driver.FindElements(By.XPath("//div[@id='jform_catid_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in categoryItems)
                {
                    if (item.Text.Equals(categoryModify))
                    {
                        item.Click();
                        break;
                    }
                }
            }

            //select access dropdown list
            if (accessModify != null)
            {
                driver.FindElement(By.Id("jform_access_chzn")).Click();
                ReadOnlyCollection<IWebElement> accessItems = driver.FindElements(By.XPath("//div[@id='jform_access_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in accessItems)
                {
                    if (item.Text.Equals(accessModify))
                    {
                        item.Click();
                        break;
                    }
                }
            }

            //select language dropdown list
            if (languageModify != null)
            {
                driver.FindElement(By.Id("jform_language_chzn")).Click();
                ReadOnlyCollection<IWebElement> languageItems = driver.FindElements(By.XPath("//div[@id='jform_language_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in languageItems)
                {
                    if (item.Text.Equals(languageModify))
                    {
                        item.Click();
                        break;
                    }
                }
            }
            //enter textarea
            if (paragraphModify != null)
            {
                IWebElement tinymceFrame = driver.FindElement(By.Id("jform_articletext_ifr"));
                driver.SwitchTo().Frame(tinymceFrame);
                IWebElement body = driver.FindElement(By.Id("tinymce"));
                body.Clear();
                body.SendKeys(paragraphModify);
                driver.SwitchTo().DefaultContent();
            }

            //save and close
            driver.FindElement(By.CssSelector("#toolbar-save > button")).Click();

        }
        //click on unpublish icon
        public void publishArticle()
        {
            driver.FindElement(By.Id("cb0")).Click();
            driver.FindElement(By.XPath("//input[@id='cb0']/ancestor::tr//span[@class='icon-unpublish']")).Click();
        }
        //click on publish icon
        public void unPublishArticle()
        {
            driver.FindElement(By.Id("cb0")).Click();
            driver.FindElement(By.XPath("//input[@id='cb0']/ancestor::tr//span[@class='icon-publish']")).Click();
        }

        public void archiveArticle()
        {
            driver.FindElement(By.Id("cb0")).Click();
            driver.FindElement(By.CssSelector("#toolbar-archive > button")).Click();
        }
        //select droplist filter
        public void selectFilterDropList(string statusFilter,string categoryFilter,string accessFilter,string authorFilter)
        {
            driver.FindElement(By.ClassName("js-stools-btn-filter")).Click();
            System.Threading.Thread.Sleep(1000);           

            if (statusFilter != null)
            {
                driver.FindElement(By.Id("filter_published_chzn")).Click();
                ReadOnlyCollection<IWebElement> statusFilterItems = driver.FindElements(By.XPath("//div[@id='filter_published_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in statusFilterItems)
                {
                    if (item.Text.Equals(statusFilter))
                    {
                        item.Click();
                        break;
                    }
                }
            }

            if (categoryFilter != null)
            {
                driver.FindElement(By.Id("filter_category_id_chzn")).Click();
                ReadOnlyCollection<IWebElement> categoryFilterItems = driver.FindElements(By.XPath("//div[@id='filter_category_id_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in categoryFilterItems)
                {
                    if (item.Text.Equals(categoryFilter))
                    {
                        item.Click();
                        break;
                    }
                }
            }

            if (accessFilter != null)
            {
                driver.FindElement(By.Id("filter_access_chzn")).Click();
                ReadOnlyCollection<IWebElement> accessFilterItems = driver.FindElements(By.XPath("//div[@id='filter_access_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in accessFilterItems)
                {
                    if (item.Text.Equals(accessFilter))
                    {
                        item.Click();
                        break;
                    }
                }
            }

            if (authorFilter != null)
            {
                driver.FindElement(By.Id("filter_author_id_chzn")).Click();
                ReadOnlyCollection<IWebElement> authorFilterItems = driver.FindElements(By.XPath("//div[@id='filter_author_id_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in authorFilterItems)
                {
                    if (item.Text.Equals(authorFilter))
                    {
                        item.Click();
                        break;
                    }
                }
            }
        }
    }
}
