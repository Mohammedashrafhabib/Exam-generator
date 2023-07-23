package Tests;

import java.util.Set;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;

import org.openqa.selenium.chrome.ChromeDriver;

import org.testng.annotations.AfterMethod;

import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import dev.failsafe.internal.util.Assert;
import pages.LoginPage;

public class LoginTests {
    WebDriver driver;
    @BeforeMethod
    public void setup()
    {
    	driver =new ChromeDriver();
    	driver.get("http://41.38.127.223:2999");
    	driver.manage().window().maximize();
    	JavascriptExecutor js = (JavascriptExecutor) driver;  
        js.executeScript("window.scrollBy(0,1000)");
    }
    @Test (priority = 1)
    public void validlogin() throws InterruptedException
    {
    	LoginPage loginpage=new LoginPage(driver);
    	loginpage.Enteremail("admin");
    	loginpage.Enterpassword("P@ssw0rd");
    	Thread.sleep(4000);
    	loginpage.Login();
    	Thread.sleep(5000);
    	String hello=driver.findElement(By.xpath("/html/body/app-root/div/app-nav-bar/header/div/div/div/div[2]")).getText();
    	Assert.isTrue(hello.equalsIgnoreCase("hello,admin"),"Login failed");
    }
    @Test (priority = 2)
    public void invalidloginbyemail() throws InterruptedException
    {
    	LoginPage loginpage=new LoginPage(driver);
    	loginpage.Enteremail("adminn");
    	loginpage.Enterpassword("P@ssw0rd");
    	Thread.sleep(1000);
    	loginpage.Login();
    	String logintxt=driver.findElement(By.xpath("/html/body/app-root/div/app-login/section/div/form/div/h1")).getText();
        Assert.isTrue(logintxt.equalsIgnoreCase("Log in to ExamGenerator"),"invalid login failed");
    }
    @Test (priority = 3)
    public void invalidloginbypassword() throws InterruptedException
    {
    	LoginPage loginpage=new LoginPage(driver);
    	loginpage.Enteremail("admin");
    	loginpage.Enterpassword("P@ssw0rdd");
    	Thread.sleep(1000);
    	loginpage.Login();
    	String logintxt=driver.findElement(By.xpath("/html/body/app-root/div/app-login/section/div/form/div/h1")).getText();
        Assert.isTrue(logintxt.equalsIgnoreCase("Log in to ExamGenerator"),"invalid login failed");
    }
    @Test (priority = 4)
    private void opengoogle() throws InterruptedException 
    {
    	LoginPage loginpage=new LoginPage(driver);
    	loginpage.opengoogle();
    	Thread.sleep(2000);
    	Set<String>pages = driver.getWindowHandles();
    	String cururl=driver.getCurrentUrl();
    	String nwurl="";
        for(String url:pages)
        {
        	if(!url.equalsIgnoreCase(cururl))
        	{
        		nwurl=url;
        	}
        }
        driver.switchTo().window(nwurl);
        System.out.println(driver.getCurrentUrl());
    	Assert.isTrue(driver.getCurrentUrl().equalsIgnoreCase("https://www.google.com/account/about/?hl=en-US"),"invalid Google Icon");
	}
    @Test (priority = 5)
    private void openfacebook() throws InterruptedException 
    {
    	LoginPage loginpage=new LoginPage(driver);
    	loginpage.openfacebook();
    	Thread.sleep(2000);
    	Set<String>pages = driver.getWindowHandles();
    	String cururl=driver.getCurrentUrl();
    	String nwurl="";
        for(String url:pages)
        {
        	if(!url.equalsIgnoreCase(cururl))
        	{
        		nwurl=url;
        	}
        }
        driver.switchTo().window(nwurl);
    	Assert.isTrue(driver.getCurrentUrl().equalsIgnoreCase("https://www.facebook.com/"),"invalid Google Icon");
	}
    @AfterMethod
    public void quit()
    {
    	driver.quit();
    }
}
