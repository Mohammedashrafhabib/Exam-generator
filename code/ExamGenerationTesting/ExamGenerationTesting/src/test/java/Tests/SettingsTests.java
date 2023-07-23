package Tests;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeClass;
import org.testng.annotations.Parameters;
import org.testng.annotations.Test;

import dev.failsafe.internal.util.Assert;
import pages.SignupPage;
import pages.SettingsPage;
import pages.LoginPage;

public class SettingsTests 
{
	WebDriver driver;
	String email;
	String newemail;
    @BeforeClass
    @Parameters ({"tempemail","tempnewemail"})
    public void setup(String tempemail,String tempnewemail) throws InterruptedException
    {
    	this.email=tempemail;
    	this.newemail=tempnewemail;
    	driver =new ChromeDriver();
    	driver.get("http://41.38.127.223:2999");
    	driver.manage().window().maximize();
    	JavascriptExecutor js = (JavascriptExecutor) driver;  
        js.executeScript("window.scrollBy(0,1000)");
        Thread.sleep(1000);
        driver.findElement(By.xpath("/html/body/app-root/div/app-login/section/div/form/div/div[5]/div[2]/p/span[2]/span")).click();
        Thread.sleep(1000);
        SignupPage signupPage=new SignupPage(driver);
        signupPage.Enterfirstname("temp");
        signupPage.Entersecondname("temp");
        signupPage.Enteremail(email);
        Thread.sleep(1000);
        signupPage.clickcontinue();
        Thread.sleep(2000);
        signupPage.Login();
        LoginPage login=new LoginPage(driver);
        login.Enteremail(email);
        login.Enterpassword("EG@123456");
        Thread.sleep(2000);
        login.Login();
        Thread.sleep(1000);
        driver.findElement(By.xpath("//*[@id=\"main-wrapper\"]/div[2]/div/app-footer/footer/div/div/a[1]/div")).click();
        Thread.sleep(1000);
    }
    @Test (priority = 1)
    public void modifyemail() throws InterruptedException
    {
    	
    	SettingsPage settingsPage=new SettingsPage(driver);
    	settingsPage.modifyemail(newemail);
    	Thread.sleep(1000);
    	settingsPage.clickmodify();
    	driver.quit();
    	driver =new ChromeDriver();
    	driver.get("http://41.38.127.223:2999");
    	driver.manage().window().maximize();
    	JavascriptExecutor js = (JavascriptExecutor) driver;  
        js.executeScript("window.scrollBy(0,1000)");
        LoginPage loginpage=new LoginPage(driver);
        loginpage.Enteremail(newemail);
        loginpage.Enterpassword("EG@123456");
        Thread.sleep(2000);
        loginpage.Login();
        Thread.sleep(2000);
        String hello=driver.findElement(By.xpath("/html/body/app-root/div/app-nav-bar/header/div/div/div/div[2]")).getText();
    	Assert.isTrue(hello.equalsIgnoreCase("hello,temp"),"Login failed");
    }
    @Test (priority = 2)
    public void modifypassword() throws InterruptedException
    {
    	driver =new ChromeDriver();
    	driver.get("http://41.38.127.223:2999");
    	driver.manage().window().maximize();
    	JavascriptExecutor js = (JavascriptExecutor) driver;  
        js.executeScript("window.scrollBy(0,1000)");
        LoginPage loginpage=new LoginPage(driver);
        loginpage.Enteremail(newemail);
        loginpage.Enterpassword("EG@123456");
        Thread.sleep(2000);
        loginpage.Login();
        Thread.sleep(2000);
        driver.findElement(By.xpath("//*[@id=\"main-wrapper\"]/div[2]/div/app-footer/footer/div/div/a[1]/div")).click();
        Thread.sleep(1000);
        SettingsPage settingsPage=new SettingsPage(driver);
        settingsPage.modifypassword();
        Thread.sleep(2000);
        settingsPage.enteroldpassword("EG@123456");
        settingsPage.enternewpassword("123456789");
        settingsPage.enterconfirmpassword("123456789");
        Thread.sleep(1000);
        settingsPage.confirm();
        Thread.sleep(2000);
        LoginPage loginpage2=new LoginPage(driver);
        loginpage2.Enteremail(newemail);
        loginpage2.Enterpassword("123456789");
        Thread.sleep(2000);
        loginpage2.Login();
        Thread.sleep(2000);
        String hello=driver.findElement(By.xpath("/html/body/app-root/div/app-nav-bar/header/div/div/div/div[2]")).getText();
    	Assert.isTrue(hello.equalsIgnoreCase("hello,temp"),"Login failed");
    }
    @AfterMethod
    public void quit()
    {
    	driver.quit();
    }
    

}
