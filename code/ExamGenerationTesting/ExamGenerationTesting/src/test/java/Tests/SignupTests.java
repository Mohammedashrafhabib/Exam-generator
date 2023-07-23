package Tests;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Parameters;
import org.testng.annotations.Test;

import dev.failsafe.internal.util.Assert;
import pages.SignupPage;
import pages.LoginPage;

public class SignupTests 
{
	WebDriver driver;
    @BeforeMethod
    public void setup() throws InterruptedException
    {
    	driver =new ChromeDriver();
    	driver.get("http://41.38.127.223:2999");
    	driver.manage().window().maximize();
    	JavascriptExecutor js = (JavascriptExecutor) driver;  
        js.executeScript("window.scrollBy(0,1000)");
        Thread.sleep(2000);
        driver.findElement(By.xpath("/html/body/app-root/div/app-login/section/div/form/div/div[5]/div[2]/p/span[2]/span")).click();
        Thread.sleep(2000);
    }
    @Test (priority = 1)
    @Parameters ({"firstname","secondname","email"})
    public void validsignup(String firstname, String secondname, String email) throws InterruptedException 
    {
		SignupPage signuppage =new SignupPage(driver);
		signuppage.Enterfirstname(firstname);
		signuppage.Entersecondname(secondname);
		signuppage.Enteremail(email);
		Thread.sleep(2000);
		signuppage.clickcontinue();
		Thread.sleep(2000);
		signuppage.Login();
		Thread.sleep(2000);
		LoginPage loginpage =new LoginPage(driver);
		loginpage.Enteremail(email);
		loginpage.Enterpassword("EG@123456");
		Thread.sleep(2000);
		loginpage.Login();
		Thread.sleep(2000);
		String hello=driver.findElement(By.xpath("/html/body/app-root/div/app-nav-bar/header/div/div/div/div[2]")).getText();
		String target="hello,"+firstname;
		Assert.isTrue(hello.equalsIgnoreCase(target),"Login failed");
	}
    @Test (priority = 2)
    @Parameters ({"wrongfirstname","wrongsecondname","wrongemail"})
    public void invalidsignup(String firstname, String secondname, String email) throws InterruptedException 
    {
		SignupPage signuppage =new SignupPage(driver);
		signuppage.Enterfirstname(firstname);
		signuppage.Entersecondname(secondname);
		signuppage.Enteremail(email);
		Thread.sleep(2000);
		signuppage.clickcontinue();
		Thread.sleep(2000);
		signuppage.Login();
		Thread.sleep(2000);
		LoginPage loginpage =new LoginPage(driver);
		loginpage.Enteremail(email);
		loginpage.Enterpassword("EG@123456");
		Thread.sleep(2000);
		loginpage.Login();
		Thread.sleep(2000);
		String logintxt=driver.findElement(By.xpath("/html/body/app-root/div/app-login/section/div/form/div/h1")).getText();
        Assert.isTrue(logintxt.equalsIgnoreCase("Log in to ExamGenerator"),"invalid login failed");
	}
    @AfterMethod
    public void quit()
    {
    	driver.quit();
    }
    

}
