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
        Random rdn = new Random();
        

        //
        [TestInitialize]
        public void Init()
        {
            driver=new ChromeDriver();
            navigateToWebpage("http://192.168.190.247/joomlatest/administrator/index.php");
            driver.Manage().Window.Maximize();
            login("lctp", "lctp");
            
        }

        [TestMethod]
        public void TC_JOOMLA_ARTICLE_001()
        {
            string title = "Article Test " + rdn.Next(10000);
            string status = "Published";
            string category = "Sample Data-Articles";
            string paragraph = "this is article content";
            string messageSuccess = "Article saved.";

            addNewArticle(title, status, category , null, null, paragraph );
            checkMessageSuccessExist(messageSuccess);
            Assert.IsTrue(checkArticleExist(title, status, category));
                        
        }
        [TestMethod]
        public void TC_JOOMLA_ARTICLE_002()
        { 
            
            string title = "Article Test " + rdn.Next(10000);
            string titleModify = title + "_update";
            string status = "Published";
            string category = "Sample Data-Articles";
            string categoryModify= "- Park Site";
            string paragraph = "this is article content";
            string messageSuccess = "Article saved.";
        
            addNewArticle(title, status, category, null, null, paragraph);            
            Assert.IsTrue(checkArticleExist(title, status, category));
            editArticle(titleModify, null,categoryModify , null, null, null);
            checkMessageSuccessExist(messageSuccess);
            Assert.IsTrue(checkArticleExist(titleModify, status, categoryModify));
        }
        [TestMethod]
        public void TC_JOOMLA_ARTICLE_003()
        {
            string title = "Article Test " + rdn.Next(10000);            
            string status = "Unpublished";
            string category = "Sample Data-Articles";
            string paragraph = "this is article content";
            string statusChange = "Published";
            string messageSuccess = "1 article published.";            

            addNewArticle(title, status, category, null, null, paragraph);            
            Assert.IsTrue(checkArticleExist(title, status, category));
            changeStatusArticle(title, statusChange);
            checkMessageSuccessExist(messageSuccess);
            Assert.IsTrue(checkChangeStatus(title, statusChange));
        }

        [TestMethod]
        public void TC_JOOMLA_ARTICLE_004()
        {
            string title = "Article Test " + rdn.Next(10000);
            string status = "Published";
            string category = "Sample Data-Articles";
            string paragraph = "this is article content";
            string messageSuccess = "1 article unpublished.";
            string statusChange = "Unpublished";

            addNewArticle(title, status, category, null, null, paragraph);
            Assert.IsTrue(checkArticleExist(title, status, category));
            changeStatusArticle(title, statusChange);
            checkMessageSuccessExist(messageSuccess);
            Assert.IsTrue(checkChangeStatus(title, statusChange));

        }

        [TestMethod]
        public void TC_JOOMLA_ARTICLE_005()
        {
            string title = "Article Test " + rdn.Next(10000);
            string status = "Published";
            string category = "Sample Data-Articles";
            string paragraph = "this is article content";
            string messageSuccess = "1 article archived.";
            string statusFilter = "Archived";
            string statusChange= "Archived"; ;

            addNewArticle(title, status, category, null, null, paragraph);            
            Assert.IsTrue(checkArticleExist(title, status, category));
            changeStatusArticle(title, statusChange);
            checkMessageSuccessExist(messageSuccess);
            selectFilterDropList(statusFilter, null, null,null);
            Assert.IsTrue(checkArticleExist(title, statusChange, category));            

        }

        [TestCleanup]
        public void CleanUp()
        {
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

        public void goToAriclePage()
        {
            driver.FindElement(By.LinkText("Content")).Click();
            driver.FindElement(By.LinkText("Articles")).Click();
        }

        public void clickNewArticleButton()
        {            
            driver.FindElement(By.Id("toolbar-new")).Click();           
        }

        public void selectStatusDropList(string item)
        {
            driver.FindElement(By.Id("jform_state_chzn")).Click();
            driver.FindElement(By.XPath("//div[@id='jform_state_chzn']//li[contains(@class,'active-result') and text()=\"" + item + "\"]")).Click();

        }
        public void selectCategoryDropList(string item)
        {
            driver.FindElement(By.Id("jform_catid_chzn")).Click();
            driver.FindElement(By.XPath("//div[@id='jform_catid_chzn']//li[contains(@class,'active-result') and text()=\"" + item + "\"]")).Click();
        }
        public void selectAccessDropList(string item)
        {
            driver.FindElement(By.Id("jform_access_chzn")).Click();
            driver.FindElement(By.XPath("//div[@id='jform_access_chzn']//li[contains(@class,'active-result') and text()=\"" + item + "\"]")).Click();
        }
        public void selectLanguageDropList(string item)
        {
            driver.FindElement(By.Id("jform_language_chzn")).Click();
            driver.FindElement(By.XPath("//div[@id='jform_language_chzn']//li[contains(@class,'active-result') and text()=\"" + item + "\"]")).Click();
        }        
       

        //add new article
        public void fillArticleInformation(string articleTitle, string item,string paragraph)
        {            
            driver.FindElement(By.Id("jform_title")).SendKeys(articleTitle);
            selectDropList();
            
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
        public bool checkArticleExist(string articleTitle, string status, string category)
        {
            if (status == "Published")
            {
                status = "icon-publish";
            }
            else if (status == "Unpublished")
            {
                status = "icon-unpublish";
            }
            else if (status == "Archived")
            {
                status = "icon-archive";
            }
            try
            {
                return (driver.FindElement(By.XPath("//a[normalize-space()=\"" + articleTitle + "\"]")).Displayed &&
                    driver.FindElement(By.XPath("//a[normalize-space()=\"" + articleTitle + "\"]/parent::div//div[@class='small']")).Text.Equals("Category: " + category) &&
                    driver.FindElement(By.XPath("//a[normalize-space()=\"" + articleTitle + "\"]/ancestor::tr//span[@class=\"" + status + "\"]")).Displayed);
            }
            catch(Exception)
            {
                return false;
            }   
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

        public void changeStatusArticle(string articleTitle,string statusChange)
        {           
            try
            {
                driver.FindElement(By.XPath("//a[normalize-space()=\"" + articleTitle + "\"]/ancestor::tr//input[@type='checkbox']")).Click();
                switch (statusChange)
                {
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


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

        public void selectFilterDropList(string filter)
        {
            driver.FindElement(By.ClassName("js-stools-btn-filter")).Click();
            System.Threading.Thread.Sleep(1000);           

            if (filter != null)
            {
                driver.FindElement(By.Id("filter_published_chzn")).Click();
                ReadOnlyCollection<IWebElement> statusFilterItems = driver.FindElements(By.XPath("//div[@id='filter_published_chzn']//li[contains(@class,'active-result')]"));
                foreach (IWebElement item in statusFilterItems)
                {
                    if (item.Text.Equals(filter))
                    {
                        item.Click();
                        break;
                    }
                }
            }            
        }

        public void checkMessageSuccessExist(string messageSuccess)
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


    }
}
